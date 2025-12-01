using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DBP_team.UI
{
    public class MultiProfilesForm : Form
    {
        private readonly int _ownerUserId;
        private ListView _list;
        private Button _btnAdd;
        private Button _btnEdit;
        private Button _btnDelete;
        private ImageList _imageList;

        public MultiProfilesForm(int ownerUserId)
        {
            _ownerUserId = ownerUserId;
            InitializeComponent();
            LoadList();
        }

        private void InitializeComponent()
        {
            this.Text = "멀티프로필 관리";
            this.Size = new Size(580, 440);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            _list = new ListView
            {
                Dock = DockStyle.Top,
                Height = 340,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false
            };
            _list.Columns.Add("대상자", 180);
            _list.Columns.Add("표시 이름", 180);
            _list.Columns.Add("생성", 90);
            _list.Columns.Add("수정", 90);
            _imageList = new ImageList { ImageSize = new Size(40, 40), ColorDepth = ColorDepth.Depth32Bit };
            _list.SmallImageList = _imageList;

            _btnAdd = new Button { Text = "추가", Width = 80, Left = 10, Top = 350 };
            _btnEdit = new Button { Text = "수정", Width = 80, Left = 100, Top = 350 };
            _btnDelete = new Button { Text = "삭제", Width = 80, Left = 190, Top = 350 };

            _btnAdd.Click += (s, e) => AddOrEdit(null);
            _btnEdit.Click += (s, e) =>
            {
                if (_list.SelectedItems.Count == 0) { MessageBox.Show("선택된 항목이 없습니다."); return; }
                var id = (int)_list.SelectedItems[0].Tag;
                AddOrEdit(id);
            };
            _btnDelete.Click += (s, e) =>
            {
                if (_list.SelectedItems.Count == 0) { MessageBox.Show("선택된 항목이 없습니다."); return; }
                var id = (int)_list.SelectedItems[0].Tag;
                if (MessageBox.Show("삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MultiProfileService.DeleteMapping(_ownerUserId, id);
                    LoadList();
                }
            };

            this.Controls.Add(_list);
            this.Controls.Add(_btnAdd);
            this.Controls.Add(_btnEdit);
            this.Controls.Add(_btnDelete);
        }

        private void LoadList()
        {
            try
            {
                _imageList.Images.Clear();
                _list.Items.Clear();
                var dt = MultiProfileService.GetMappings(_ownerUserId);
                foreach (DataRow r in dt.Rows)
                {
                    int id = Convert.ToInt32(r["id"]);
                    string displayName = r["display_name"] == DBNull.Value ? "(기본)" : r["display_name"].ToString();
                    string targetName = r["target_name"] == DBNull.Value ? "-" : r["target_name"].ToString();
                    string created = r["created_at"] == DBNull.Value ? "-" : Convert.ToDateTime(r["created_at"]).ToString("yyyy-MM-dd");
                    string updated = r["updated_at"] == DBNull.Value ? "-" : Convert.ToDateTime(r["updated_at"]).ToString("yyyy-MM-dd");

                    var imgBytes = MultiProfileService.GetProfileImageForViewer(_ownerUserId, (int)r["target_user_id"]);
                    Image img = null;
                    if (imgBytes != null)
                    {
                        try { using (var ms = new MemoryStream(imgBytes)) img = Image.FromStream(ms); } catch { }
                    }
                    if (img != null) _imageList.Images.Add(id.ToString(), img);
                    var lvi = new ListViewItem(targetName) { Tag = id };
                    if (img != null) lvi.ImageKey = id.ToString();
                    lvi.SubItems.Add(displayName);
                    lvi.SubItems.Add(created);
                    lvi.SubItems.Add(updated);
                    _list.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("목록 로드 오류: " + ex.Message);
            }
        }

        private void AddOrEdit(int? mappingId)
        {
            using (var dlg = new MultiProfileEditForm(_ownerUserId, mappingId))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadList();
                }
            }
        }
    }
}
