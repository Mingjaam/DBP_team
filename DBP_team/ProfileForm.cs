using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class ProfileForm : Form
    {
        private readonly int _userId;

        public ProfileForm(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // 디자이너 컨트롤 확인, 디버그 텍스트는 제거하여 라벨을 덮어쓰지 않음
            if (pictureProfile == null || labelFullName == null)
            {
                MessageBox.Show("디자이너 컨트롤이 초기화되지 않았습니다. Designer 파일과 namespace를 확인하세요.", "디버그", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // 사진 배경만 표시
                pictureProfile.BackColor = Color.LightGray;
            }

            // btnClose가 디자이너에 없을 수 있으므로 안전하게 연결
            try { this.btnClose.Click -= btnClose_Click; this.btnClose.Click += btnClose_Click; } catch { }

            // btnChangeImage가 디자이너에 없을 수 있으므로 안전하게 연결
            try { this.btnChangeImage.Click -= btnChangeImage_Click; this.btnChangeImage.Click += btnChangeImage_Click; } catch { }

            // save 버튼이 있으면 연결
            try { this.btnSave.Click -= btnSave_Click; this.btnSave.Click += btnSave_Click; } catch { }
        }

        // 디자이너에서 Load 이벤트로 연결되어 있음
        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void LoadProfile()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT u.id, u.full_name, u.nickname, u.email, u.phone, u.profile_image, " +
                    "c.name AS company_name, d.name AS department_name, t.name AS team_name " +
                    "FROM users u " +
                    "LEFT JOIN companies c ON u.company_id = c.id " +
                    "LEFT JOIN departments d ON u.department_id = d.id " +
                    "LEFT JOIN teams t ON u.team_id = t.id " +
                    "WHERE u.id = @id",
                    new MySqlParameter("@id", _userId));

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("사용자 정보를 불러오지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                var row = dt.Rows[0];

                // 라벨들은 디자이너에서 고정 텍스트("이름 :", "별명 :")를 유지하도록 한다.
                // 텍스트박스에만 실제 값을 채워준다.
                var fullName = row.Table.Columns.Contains("full_name") ? row["full_name"]?.ToString() : string.Empty;
                if (txtFullName != null)
                    txtFullName.Text = fullName ?? string.Empty;

                labelEmail.Text = "이메일: " + (row["email"]?.ToString() ?? "(없음)");
                labelCompany.Text = "회사: " + (row["company_name"]?.ToString() ?? "(없음)");
                labelDepartment.Text = "부서: " + (row["department_name"]?.ToString() ?? "(없음)");
                labelTeam.Text = "팀: " + (row["team_name"]?.ToString() ?? "(없음)");

                // nickname 표시: 라벨은 고정, 텍스트박스에만 할당
                try
                {
                    var nick = row.Table.Columns.Contains("nickname") ? row["nickname"]?.ToString() : null;
                    if (txtNickname != null)
                    {
                        txtNickname.Text = !string.IsNullOrWhiteSpace(nick) ? nick : string.Empty;
                    }
                }
                catch { }

                // 프로필 이미지 로드 (BLOB -> Image)
                if (row["profile_image"] != DBNull.Value && row["profile_image"] is byte[])
                {
                    var bytes = (byte[])row["profile_image"];
                    using (var ms = new MemoryStream(bytes))
                    {
                        try
                        {
                            var img = Image.FromStream(ms);
                            // make copy to avoid stream dependency
                            var bmp = new Bitmap(img);
                            pictureProfile.Image = bmp;
                        }
                        catch
                        {
                            pictureProfile.Image = null;
                        }
                    }
                }
                else
                {
                    pictureProfile.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로필 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 이미지 변경 버튼 클릭 핸들러
        private void btnChangeImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일|*.*";
                ofd.Title = "프로필 이미지 선택";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // 안전하게 파일을 열어 Image를 로드한 뒤 독립적인 Bitmap으로 복사
                    Image loadedImage;
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    using (var tmp = Image.FromStream(fs))
                    {
                        loadedImage = new Bitmap(tmp); // copy so it doesn't depend on the stream
                    }

                    // 이전 이미지가 있으면 Dispose 해서 파일 잠금/리소스 문제 방지
                    try { pictureProfile.Image?.Dispose(); } catch { }

                    // 미리보기 적용
                    pictureProfile.Image = loadedImage;

                    // 이미지 바이트로 변환 (PNG로 저장)
                    byte[] imgBytes;
                    using (var ms = new MemoryStream())
                    {
                        loadedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imgBytes = ms.ToArray();
                    }

                    // DB에 저장: profile_image 컬럼이 BLOB 타입으로 존재한다고 가정
                    var sql = "UPDATE users SET profile_image = @img WHERE id = @id";
                    var p1 = new MySqlParameter("@img", MySqlDbType.Blob) { Value = imgBytes };
                    var p2 = new MySqlParameter("@id", _userId);

                    var rows = DBManager.Instance.ExecuteNonQuery(sql, p1, p2);
                    if (rows > 0)
                    {
                        MessageBox.Show("프로필 이미지가 변경되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("이미지 업데이트에 실패했습니다. 사용자 레코드를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("이미지 적용 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 저장 버튼: 이름/별명 업데이트
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newName = txtFullName?.Text?.Trim() ?? string.Empty;
                var newNick = txtNickname?.Text?.Trim();

                var sql = "UPDATE users SET full_name = @name, nickname = @nick WHERE id = @id";
                var pName = new MySqlParameter("@name", string.IsNullOrWhiteSpace(newName) ? (object)DBNull.Value : newName);
                var pNick = new MySqlParameter("@nick", string.IsNullOrWhiteSpace(newNick) ? (object)DBNull.Value : newNick);
                var pId = new MySqlParameter("@id", _userId);

                var rows = DBManager.Instance.ExecuteNonQuery(sql, pName, pNick, pId);
                if (rows > 0)
                {
                    MessageBox.Show("프로필 정보가 저장되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProfile();
                }
                else
                {
                    MessageBox.Show("저장에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelFullName_Click(object sender, EventArgs e)
        {

        }
    }
}
