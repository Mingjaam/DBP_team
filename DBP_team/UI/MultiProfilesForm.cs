using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DBP_team.UI
{
    public class MultiProfilesForm : Form
    {
        // 그룹(표시 이름) 단위만 관리하는 간단한 폼 (개별 보기 제거)
        private readonly int _ownerUserId;
        private ListView _list;
        private Button _btnAddGroup;
        private Button _btnEditGroup;
        private Button _btnDeleteGroup;
        private ImageList _imageList;

        public MultiProfilesForm(int ownerUserId)
        {
            _ownerUserId = ownerUserId;
            InitializeComponent();
            LoadGroups();
        }

        private void InitializeComponent()
        {
            this.Text = "DBP Talk - 멀티프로필 그룹 관리";
            this.Size = new Size(680, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("맑은 고딕", 9F);

            _list = new ListView
            {
                Left = 15,
                Top = 15,
                Width = 640,
                Height = 430,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("맑은 고딕", 9F)
            };
            _imageList = new ImageList { ImageSize = new Size(40, 40), ColorDepth = ColorDepth.Depth32Bit };
            _list.SmallImageList = _imageList;
            ConfigureGroupColumns();

            _btnAddGroup = new Button
            {
                Text = "추가",
                Width = 90,
                Height = 32,
                Left = 15,
                Top = 460,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 144, 226),
                ForeColor = Color.White
            };
            _btnAddGroup.FlatAppearance.BorderSize = 0;

            _btnEditGroup = new Button
            {
                Text = "수정",
                Width = 90,
                Height = 32,
                Left = 115,
                Top = 460,
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnEditGroup.FlatAppearance.BorderSize = 0;

            _btnDeleteGroup = new Button
            {
                Text = "삭제",
                Width = 90,
                Height = 32,
                Left = 215,
                Top = 460,
                Font = new Font("맑은 고딕", 9F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            _btnDeleteGroup.FlatAppearance.BorderSize = 0;

            _btnAddGroup.Click += (s, e) => AddOrEditGroup(null);
            _btnEditGroup.Click += (s, e) =>
            {
                if (_list.SelectedItems.Count == 0) { MessageBox.Show("선택된 그룹이 없습니다."); return; }
                var groupName = _list.SelectedItems[0].Text;
                AddOrEditGroup(groupName == "(기본)" ? null : groupName);
            };
            _btnDeleteGroup.Click += (s, e) =>
            {
                if (_list.SelectedItems.Count == 0) { MessageBox.Show("선택된 그룹이 없습니다."); return; }
                var groupName = _list.SelectedItems[0].Text;
                if (MessageBox.Show("그룹을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MultiProfileService.DeleteGroup(_ownerUserId, groupName);
                    LoadGroups();
                }
            };

            this.Controls.Add(_list);
            this.Controls.Add(_btnAddGroup);
            this.Controls.Add(_btnEditGroup);
            this.Controls.Add(_btnDeleteGroup);
        }

        private void ConfigureGroupColumns()
        {
            _list.Columns.Clear();
            _list.Columns.Add("그룹 이름", 200);
            _list.Columns.Add("대상 수", 100, HorizontalAlignment.Right);
            _list.Columns.Add("대표 사진", 100);
            _list.Columns.Add("생성", 120);
            _list.Columns.Add("수정", 120);
        }

        private void LoadGroups()
        {
            try
            {
                _imageList.Images.Clear();
                _list.Items.Clear();
                var dt = MultiProfileService.GetGroups(_ownerUserId);
                foreach (DataRow r in dt.Rows)
                {
                    string groupName = r["group_name"].ToString();
                    int count = r["target_count"] == DBNull.Value ? 0 : Convert.ToInt32(r["target_count"]);
                    string created = r["first_created"] == DBNull.Value ? "-" : Convert.ToDateTime(r["first_created"]).ToString("yyyy-MM-dd");
                    string updated = r["last_updated"] == DBNull.Value ? "-" : Convert.ToDateTime(r["last_updated"]).ToString("yyyy-MM-dd");
                    int photoRows = r["photo_rows"] == DBNull.Value ? 0 : Convert.ToInt32(r["photo_rows"]);

                    byte[] imgBytes = null;
                    if (photoRows > 0 && r["target_ids"] != DBNull.Value)
                    {
                        var firstTargetStr = r["target_ids"].ToString().Split(',').FirstOrDefault();
                        if (int.TryParse(firstTargetStr, out int firstTarget))
                        {
                            imgBytes = MultiProfileService.GetProfileImageForViewer(_ownerUserId, firstTarget);
                        }
                    }
                    Image img = null;
                    if (imgBytes != null)
                    {
                        try { using (var ms = new MemoryStream(imgBytes)) img = Image.FromStream(ms); } catch { }
                    }
                    if (img != null) _imageList.Images.Add(groupName, img);
                    var lvi = new ListViewItem(groupName);
                    if (img != null) lvi.ImageKey = groupName;
                    lvi.SubItems.Add(count.ToString());
                    lvi.SubItems.Add(photoRows > 0 ? "있음" : "없음");
                    lvi.SubItems.Add(created);
                    lvi.SubItems.Add(updated);
                    _list.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("그룹 로드 오류: " + ex.Message);
            }
        }

        private void AddOrEditGroup(string groupName)
        {
            using (var dlg = new MultiProfileGroupEditForm(_ownerUserId, groupName))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK) LoadGroups();
            }
        }
    }
}