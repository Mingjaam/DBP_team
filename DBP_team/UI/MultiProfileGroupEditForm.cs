using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DBP_team.UI
{
    public class MultiProfileGroupEditForm : Form
    {
        private readonly int _ownerUserId;
        private readonly string _groupName;
        private TextBox _txtGroupName;
        private PictureBox _pic;
        private Button _btnImage;
        private CheckedListBox _lstTargets;
        private Button _btnSave;
        private Button _btnCancel;
        private byte[] _photoBytes;

        public MultiProfileGroupEditForm(int ownerUserId, string groupName)
        {
            _ownerUserId = ownerUserId;
            _groupName = groupName;
            InitializeComponent();
            LoadTargets();
            LoadExisting();
        }

        private void InitializeComponent()
        {
            this.Text = _groupName == null ? "그룹 추가" : "그룹 수정";
            this.Size = new Size(600, 620);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("맑은 고딕", 9F);

            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(15)
            };

            var lblName = new Label
            {
                Text = "그룹 이름 (비워두면 기본 그룹)",
                Left = 15,
                Top = 15,
                Width = 250,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
            };

            _txtGroupName = new TextBox
            {
                Left = 15,
                Top = 40,
                Width = 550,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("맑은 고딕", 10F)
            };
            if (_groupName != null) _txtGroupName.Text = _groupName;

            _pic = new PictureBox
            {
                Left = 15,
                Top = 75,
                Width = 140,
                Height = 140,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _btnImage = new Button
            {
                Left = 165,
                Top = 75,
                Width = 140,
                Height = 32,
                Text = "사진 선택",
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnImage.FlatAppearance.BorderSize = 0;
            _btnImage.Click += (s, e) => ChooseImage();

            pnlTop.Controls.Add(lblName);
            pnlTop.Controls.Add(_txtGroupName);
            pnlTop.Controls.Add(_pic);
            pnlTop.Controls.Add(_btnImage);

            var lblTargets = new Label
            {
                Text = "대상 사용자",
                Left = 15,
                Top = 235,
                Width = 120,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
            };

            _lstTargets = new CheckedListBox
            {
                Left = 15,
                Top = 260,
                Width = 560,
                Height = 260,
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("맑은 고딕", 9F)
            };

            _btnSave = new Button
            {
                Text = "저장",
                Left = 390,
                Top = 535,
                Width = 90,
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
                Left = 490,
                Top = 535,
                Width = 90,
                Height = 35,
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnCancel.FlatAppearance.BorderSize = 0;

            _btnSave.Click += (s, e) => SaveGroup();
            _btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(pnlTop);
            this.Controls.Add(lblTargets);
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
                var g = MultiProfileService.GetGroup(_groupName ?? "(기본)", _ownerUserId);
                if (g.Photo != null)
                {
                    _photoBytes = g.Photo;
                    try { using (var ms = new MemoryStream(_photoBytes)) _pic.Image = Image.FromStream(ms); } catch { }
                }
                var targets = g.Targets;
                for (int i = 0; i < _lstTargets.Items.Count; i++)
                {
                    var ti = (TargetItem)_lstTargets.Items[i];
                    if (targets.Contains(ti.Id)) _lstTargets.SetItemChecked(i, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("그룹 로드 오류: " + ex.Message);
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

        private void SaveGroup()
        {
            try
            {
                var selected = new List<int>();
                foreach (var obj in _lstTargets.CheckedItems)
                {
                    var ti = obj as TargetItem; if (ti != null) selected.Add(ti.Id);
                }
                var gName = _txtGroupName.Text?.Trim();
                if (string.IsNullOrEmpty(gName)) gName = "(기본)";
                MultiProfileService.SaveGroup(_ownerUserId, gName, _photoBytes, selected);
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