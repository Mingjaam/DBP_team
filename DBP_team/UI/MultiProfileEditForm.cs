using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DBP_team.UI
{
    public class MultiProfileEditForm : Form
    {
        private readonly int _ownerUserId;
        private readonly int? _mappingId;

        private TextBox _txtName;
        private PictureBox _pic;
        private Button _btnImage;
        private CheckedListBox _lstTargets;
        private Button _btnSave;
        private Button _btnCancel;

        private byte[] _photoBytes;

        public MultiProfileEditForm(int ownerUserId, int? mappingId)
        {
            _ownerUserId = ownerUserId;
            _mappingId = mappingId;
            InitializeComponent();
            LoadTargets();
            if (_mappingId != null) LoadExisting();
        }

        private void InitializeComponent()
        {
            this.Text = _mappingId == null ? "멀티프로필 추가" : "멀티프로필 수정";
            this.Size = new Size(560, 580);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("맑은 고딕", 9F);

            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(15)
            };

            var lblN = new Label
            {
                Text = "이름",
                Left = 15,
                Top = 15,
                Width = 60,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
            };
            _txtName = new TextBox
            {
                Left = 80,
                Top = 12,
                Width = 450,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("맑은 고딕", 10F)
            };

            _pic = new PictureBox
            {
                Left = 15,
                Top = 50,
                Width = 130,
                Height = 130,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _btnImage = new Button
            {
                Left = 155,
                Top = 50,
                Width = 130,
                Height = 32,
                Text = "사진 선택",
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnImage.FlatAppearance.BorderSize = 0;
            _btnImage.Click += (s, e) => ChooseImage();

            pnlTop.Controls.Add(lblN);
            pnlTop.Controls.Add(_txtName);
            pnlTop.Controls.Add(_pic);
            pnlTop.Controls.Add(_btnImage);

            var lblT = new Label
            {
                Text = "보이는 사람 선택",
                Left = 15,
                Top = 215,
                Width = 200,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
            };

            _lstTargets = new CheckedListBox
            {
                Left = 15,
                Top = 240,
                Width = 520,
                Height = 240,
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("맑은 고딕", 9F)
            };

            _btnSave = new Button
            {
                Text = "저장",
                Left = 350,
                Top = 495,
                Width = 85,
                Height = 35,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 144, 226),
                ForeColor = Color.White
            };
            _btnSave.FlatAppearance.BorderSize = 0;

            _btnCancel = new Button
            {
                Text = "취소",
                Left = 445,
                Top = 495,
                Width = 85,
                Height = 35,
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnCancel.FlatAppearance.BorderSize = 0;

            _btnSave.Click += (s, e) => Save();
            _btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(pnlTop);
            this.Controls.Add(lblT);
            this.Controls.Add(_lstTargets);
            this.Controls.Add(_btnSave);
            this.Controls.Add(_btnCancel);
        }

        private void LoadTargets()
        {
            try
            {
                var dt = MultiProfileService.GetCompanyUsersExceptOwner(_ownerUserId);
                _lstTargets.Items.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    int id = Convert.ToInt32(r["id"]);
                    string name = r["name"].ToString();
                    _lstTargets.Items.Add(new TargetItem { Id = id, Name = name }, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("대상자 로드 오류: " + ex.Message);
            }
        }

        private void LoadExisting()
        {
            try
            {
                var (displayName, photo, targetUserId) = MultiProfileService.GetMapping(_mappingId.Value);
                _txtName.Text = displayName ?? string.Empty;
                _photoBytes = photo;
                if (_photoBytes != null)
                {
                    try { using (var ms = new MemoryStream(_photoBytes)) _pic.Image = Image.FromStream(ms); } catch { }
                }

                for (int i = 0; i < _lstTargets.Items.Count; i++)
                {
                    var it = (TargetItem)_lstTargets.Items[i];
                    if (it.Id == targetUserId) _lstTargets.SetItemChecked(i, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("불러오기 오류: " + ex.Message);
            }
        }

        private void ChooseImage()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일|*.*";
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    using (var img = Image.FromStream(fs))
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        _photoBytes = ms.ToArray();
                        _pic.Image = new Bitmap(img);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("이미지 로드 오류: " + ex.Message);
                }
            }
        }

        private void Save()
        {
            try
            {
                var selected = new List<int>();
                foreach (var obj in _lstTargets.CheckedItems)
                {
                    var ti = obj as TargetItem; if (ti != null) selected.Add(ti.Id);
                }

                var name = _txtName.Text?.Trim();
                MultiProfileService.SaveMapping(_ownerUserId, _mappingId, name, _photoBytes, selected);

                MessageBox.Show("저장되었습니다.");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 오류: " + ex.Message);
            }
        }

        private class TargetItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }
    }
}