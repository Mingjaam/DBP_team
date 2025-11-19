using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;
using System.Text;

namespace DBP_team
{
    public class AdminForm : Form
    {
        private readonly User _me;
        private readonly int _companyId;

        private TabControl _tabs;

        // Dept
        private TextBox _txtDeptName, _txtDeptSearch;
        private Button _btnDeptAdd, _btnDeptUpdate, _btnDeptSearch, _btnSeedDept;
        private DataGridView _gridDept;

        // User-Dept
        private TextBox _txtUserSearch;
        private ComboBox _cboDeptForUser;
        private Button _btnUserSearch, _btnApplyDept;
        private DataGridView _gridUsers;

        // Chat search
        private DateTimePicker _dtFrom, _dtTo;
        private TextBox _txtKeyword;
        private ComboBox _cboUserFilter;
        private Button _btnChatSearch;
        private DataGridView _gridChat;

        public AdminForm(User me)
        {
            _me = me ?? throw new ArgumentNullException(nameof(me));
            _companyId = me.CompanyId ?? 0;

            if (!AdminGuard.IsAdmin(me))
            {
                MessageBox.Show("관리자 권한이 없습니다.", "권한 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            Text = "관리자 콘솔";
            Width = 1024;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            LoadDeptGrid();
            LoadDeptComboForUser();
            LoadUsersGrid();
            LoadUserFilterCombo();
        }

        private void BuildUi()
        {
            _tabs = new TabControl { Dock = DockStyle.Fill };

            // Tab 1: 부서관리
            var pageDept = new TabPage("부서관리");
            var pnlDeptTop = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(8) };
            _txtDeptName = new TextBox { Width = 220 };
            _btnDeptAdd = new Button { Text = "등록", Left = 230, Width = 80 };
            _btnDeptUpdate = new Button { Text = "변경", Left = 315, Width = 80 };
            _btnSeedDept = new Button { Text = "기본부서 생성", Left = 400, Width = 110 };
            _txtDeptSearch = new TextBox { Left = 520, Width = 220 };
            _btnDeptSearch = new Button { Text = "검색", Left = 745, Width = 80 };

            _btnDeptAdd.Click += (s, e) => AddDepartment();
            _btnDeptUpdate.Click += (s, e) => UpdateDepartment();
            _btnDeptSearch.Click += (s, e) => LoadDeptGrid(_txtDeptSearch.Text?.Trim());
            _btnSeedDept.Click += (s, e) => SeedDepartments();

            pnlDeptTop.Controls.AddRange(new Control[] { _txtDeptName, _btnDeptAdd, _btnDeptUpdate, _btnSeedDept, _txtDeptSearch, _btnDeptSearch });
            _gridDept = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            _gridDept.MultiSelect = false;
            _gridDept.CellClick += (s, e) => { if (e.RowIndex >= 0) _txtDeptName.Text = Convert.ToString(_gridDept.Rows[e.RowIndex].Cells["name"].Value); };
            pageDept.Controls.Add(_gridDept);
            pageDept.Controls.Add(pnlDeptTop);

            // Tab 2: 사용자 소속 변경
            var pageUserDept = new TabPage("사용자 소속 변경");
            var pnlUserTop = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(8) };
            _txtUserSearch = new TextBox { Width = 240 };
            _btnUserSearch = new Button { Text = "검색", Left = 245, Width = 80 };
            _btnUserSearch.Click += (s, e) => LoadUsersGrid(_txtUserSearch.Text?.Trim());
            _cboDeptForUser = new ComboBox { Left = 330, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            _btnApplyDept = new Button { Text = "선택 사용자 소속 변경", Left = 575, Width = 180 };
            _btnApplyDept.Click += (s, e) => ApplyUserDepartment();
            pnlUserTop.Controls.AddRange(new Control[] { _txtUserSearch, _btnUserSearch, _cboDeptForUser, _btnApplyDept });

            _gridUsers = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            _gridUsers.MultiSelect = true;
            pageUserDept.Controls.Add(_gridUsers);
            pageUserDept.Controls.Add(pnlUserTop);

            // Tab 3: 대화 검색
            var pageChat = new TabPage("대화 검색");
            var pnlChatTop = new Panel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(8) };
            _dtFrom = new DateTimePicker { Width = 140, Value = DateTime.Now.Date.AddDays(-7) };
            _dtTo = new DateTimePicker { Left = 150, Width = 140, Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1) };
            _txtKeyword = new TextBox { Left = 300, Width = 220 };
            _cboUserFilter = new ComboBox { Left = 525, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            _btnChatSearch = new Button { Text = "검색", Left = 730, Width = 80 };
            _btnChatSearch.Click += (s, e) => LoadChatGrid();
            pnlChatTop.Controls.AddRange(new Control[] { _dtFrom, _dtTo, _txtKeyword, _cboUserFilter, _btnChatSearch });

            _gridChat = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            pageChat.Controls.Add(_gridChat);
            pageChat.Controls.Add(pnlChatTop);

            _tabs.TabPages.Add(pageDept);
            _tabs.TabPages.Add(pageUserDept);
            _tabs.TabPages.Add(pageChat);
            Controls.Add(_tabs);
        }

        // 부서 목록 로드
        private void LoadDeptGrid(string keyword = null)
        {
            var sql = "SELECT id, name FROM departments WHERE company_id = @cid " +
                      (string.IsNullOrWhiteSpace(keyword) ? string.Empty : "AND name LIKE @kw ") +
                      "ORDER BY name";
            MySqlParameter[] pars = string.IsNullOrWhiteSpace(keyword)
                ? new[] { new MySqlParameter("@cid", _companyId) }
                : new[] { new MySqlParameter("@cid", _companyId), new MySqlParameter("@kw", "%" + keyword + "%") };
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars);
            _gridDept.DataSource = dt;
        }

        private void AddDepartment()
        {
            var name = _txtDeptName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "INSERT INTO departments (company_id, name) VALUES (@cid, @name)",
                new MySqlParameter("@cid", _companyId), new MySqlParameter("@name", name));
            LoadDeptGrid();
            LoadDeptComboForUser();
        }

        private void UpdateDepartment()
        {
            if (_gridDept.CurrentRow == null) { MessageBox.Show("행을 선택하세요."); return; }
            var id = Convert.ToInt32((_gridDept.CurrentRow.DataBoundItem as DataRowView)["id"]);
            var name = _txtDeptName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "UPDATE departments SET name = @name WHERE id = @id AND company_id = @cid",
                new MySqlParameter("@name", name), new MySqlParameter("@id", id), new MySqlParameter("@cid", _companyId));
            LoadDeptGrid();
            LoadDeptComboForUser();
        }

        private void SeedDepartments()
        {
            var names = new[] { "인사부서", "개발부서", "마케팅부서" };
            foreach (var n in names)
            {
                DBManager.Instance.ExecuteNonQuery(
                    "INSERT INTO departments (company_id, name) SELECT @cid, @name FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM departments WHERE company_id=@cid AND name=@name)",
                    new MySqlParameter("@cid", _companyId), new MySqlParameter("@name", n));
            }
            LoadDeptGrid();
            LoadDeptComboForUser();
        }

        private void LoadDeptComboForUser()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));
            _cboDeptForUser.DataSource = dt;
            _cboDeptForUser.DisplayMember = "name";
            _cboDeptForUser.ValueMember = "id";
        }

        private void LoadUsersGrid(string keyword = null)
        {
            var sql = "SELECT id, COALESCE(full_name,email) AS name, email, department_id FROM users WHERE company_id=@cid";
            var pars = new System.Collections.Generic.List<MySqlParameter> { new MySqlParameter("@cid", _companyId) };
            if (!string.IsNullOrWhiteSpace(keyword)) { sql += " AND (full_name LIKE @kw OR email LIKE @kw)"; pars.Add(new MySqlParameter("@kw", "%" + keyword + "%")); }
            sql += " ORDER BY name";
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars.ToArray());
            _gridUsers.DataSource = dt;
        }

        private void ApplyUserDepartment()
        {
            if (_gridUsers.SelectedRows.Count == 0) { MessageBox.Show("사용자를 선택하세요."); return; }
            if (!int.TryParse(Convert.ToString(_cboDeptForUser.SelectedValue), out int deptId)) { MessageBox.Show("부서를 선택하세요."); return; }

            foreach (DataGridViewRow row in _gridUsers.SelectedRows)
            {
                var drv = row.DataBoundItem as DataRowView;
                if (drv == null) continue;
                int uid = Convert.ToInt32(drv["id"]);
                DBManager.Instance.ExecuteNonQuery(
                    "UPDATE users SET department_id = @did WHERE id = @uid AND company_id = @cid",
                    new MySqlParameter("@did", deptId), new MySqlParameter("@uid", uid), new MySqlParameter("@cid", _companyId));
            }

            LoadUsersGrid(_txtUserSearch.Text?.Trim());
        }

        private void LoadUserFilterCombo()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, COALESCE(full_name,email) AS name FROM users WHERE company_id=@cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));
            _cboUserFilter.DataSource = dt;
            _cboUserFilter.DisplayMember = "name";
            _cboUserFilter.ValueMember = "id";
            _cboUserFilter.SelectedIndex = -1;
        }

        private void LoadChatGrid()
        {
            var from = _dtFrom.Value;
            var to = _dtTo.Value;
            string kw = _txtKeyword.Text?.Trim();
            int? userId = _cboUserFilter.SelectedIndex >= 0 ? (int?)Convert.ToInt32(_cboUserFilter.SelectedValue) : null;

            var sql = new StringBuilder();
            sql.Append("SELECT c.id, c.sender_id, s.full_name AS sender, c.receiver_id, r.full_name AS receiver, c.message, c.created_at ");
            sql.Append("FROM chat c ");
            sql.Append("LEFT JOIN users s ON s.id = c.sender_id ");
            sql.Append("LEFT JOIN users r ON r.id = c.receiver_id ");
            sql.Append("WHERE c.created_at BETWEEN @from AND @to ");
            sql.Append("AND (s.company_id = @cid OR r.company_id = @cid) ");
            var pars = new System.Collections.Generic.List<MySqlParameter> {
                new MySqlParameter("@from", from), new MySqlParameter("@to", to), new MySqlParameter("@cid", _companyId)
            };
            if (!string.IsNullOrWhiteSpace(kw)) { sql.Append("AND c.message LIKE @kw "); pars.Add(new MySqlParameter("@kw", "%" + kw + "%")); }
            if (userId.HasValue) { sql.Append("AND (c.sender_id = @uid OR c.receiver_id = @uid) "); pars.Add(new MySqlParameter("@uid", userId.Value)); }
            sql.Append("ORDER BY c.created_at DESC");

            var dt = DBManager.Instance.ExecuteDataTable(sql.ToString(), pars.ToArray());
            _gridChat.DataSource = dt;
        }
    }
}
