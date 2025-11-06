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

            // 디버그: 디자이너에서 만든 컨트롤이 null인지 확인하려면 아래 임시 코드로 확인
            if (pictureProfile == null || labelFullName == null)
            {
                MessageBox.Show("디자이너 컨트롤이 초기화되지 않았습니다. Designer 파일과 namespace를 확인하세요.", "디버그", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // 보이는지 확인용 임시 값
                labelFullName.Text = "디버그 이름 (디자이너 OK)";
                labelFullName.ForeColor = Color.Red;
                pictureProfile.BackColor = Color.LightGray;
            }

            // btnClose가 디자이너에 없을 수 있으므로 안전하게 연결
            try { this.btnClose.Click -= btnClose_Click; this.btnClose.Click += btnClose_Click; } catch { }

            // btnChangeImage가 디자이너에 없을 수 있으므로 안전하게 연결
            try { this.btnChangeImage.Click -= btnChangeImage_Click; this.btnChangeImage.Click += btnChangeImage_Click; } catch { }
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
                    "SELECT u.id, u.full_name, u.email, u.phone, u.profile_image, " +
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

                labelFullName.Text = "이름: " + (row["full_name"]?.ToString() ?? "(없음)");
                labelEmail.Text = "이메일: " + (row["email"]?.ToString() ?? "(없음)");
                labelCompany.Text = "회사: " + (row["company_name"]?.ToString() ?? "(없음)");
                labelDepartment.Text = "부서: " + (row["department_name"]?.ToString() ?? "(없음)");
                labelTeam.Text = "팀: " + (row["team_name"]?.ToString() ?? "(없음)");

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
    }
}
