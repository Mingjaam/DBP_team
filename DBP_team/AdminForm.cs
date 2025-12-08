using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;
using System.Text;
using System.ComponentModel;

namespace DBP_team
{
    public partial class AdminForm : Form
    {
        private readonly User _me;
        private readonly int _companyId;

        // Designer-friendly parameterless constructor
        public AdminForm() : this(new User { Id = 0, CompanyId = 0, FullName = "관리자" })
        {
        }

        public AdminForm(User me)
        {
            _me = me ?? new User { Id = 0, CompanyId = 0, FullName = "관리자" };
            _companyId = _me.CompanyId ?? 0;

            InitializeComponent();

            // Detect design-time reliably
            bool isDesignTime = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            if (!isDesignTime)
            {
                if (!AdminGuard.IsAdmin(_me))
                {
                    MessageBox.Show("관리자만 접근 가능합니다.", "권한 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Load += (s, e) => this.Close(); // 폼 로드 후 바로 닫기
                    return;
                }

                // DateTimePicker 값 설정
                _dtFrom.Value = DateTime.Now.Date.AddDays(-7);
                _dtTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadDeptGrid();
                LoadDeptComboForUser();
                LoadUsersGrid();
                LoadUserFilterCombo();
                SearchAccessLogs(); // 초기 접속 로그 로드
            }
        }

        private void DeptAdd_Click(object sender, EventArgs e) => AddDepartment();
        private void DeptUpdate_Click(object sender, EventArgs e) => UpdateDepartment();
        private void DeptSearch_Click(object sender, EventArgs e) => LoadDeptGrid(_txtDeptSearch.Text?.Trim());
        private void DeptGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) _txtDeptName.Text = Convert.ToString(_gridDept.Rows[e.RowIndex].Cells["name"].Value);
        }
        private void UserSearch_Click(object sender, EventArgs e) => LoadUsersGrid(_txtUserSearch.Text?.Trim());
        private void ApplyDept_Click(object sender, EventArgs e) => ApplyUserDepartment();
        private void ChatSearch_Click(object sender, EventArgs e) => LoadChatGrid();
        private void SearchLog_Click(object sender, EventArgs e) => SearchAccessLogs();
        private void GridChat_CellClick(object sender, DataGridViewCellEventArgs e) { /* 필요한 경우 구현 */ }
        private void GridLogs_CellClick(object sender, DataGridViewCellEventArgs e) { /* 필요한 경우 구현 */ }

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
            if (_gridDept.Columns.Contains("id")) _gridDept.Columns["id"].Visible = false;
            if (_gridDept.Columns.Contains("name")) _gridDept.Columns["name"].HeaderText = "부서명";
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
            if (_gridDept.CurrentRow == null) { MessageBox.Show("수정할 부서를 선택하세요."); return; }
            var id = Convert.ToInt32((_gridDept.CurrentRow.DataBoundItem as DataRowView)["id"]);
            var name = _txtDeptName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("부서명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "UPDATE departments SET name = @name WHERE id = @id AND company_id = @cid",
                new MySqlParameter("@name", name), new MySqlParameter("@id", id), new MySqlParameter("@cid", _companyId));
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
            var sql = "SELECT u.id, COALESCE(u.full_name,u.email) AS name, u.email, u.department_id, d.name AS department " +
                      "FROM users u LEFT JOIN departments d ON d.id = u.department_id " +
                      "WHERE u.company_id=@cid";
            var pars = new System.Collections.Generic.List<MySqlParameter> { new MySqlParameter("@cid", _companyId) };
            if (!string.IsNullOrWhiteSpace(keyword)) { sql += " AND (u.full_name LIKE @kw OR u.email LIKE @kw)"; pars.Add(new MySqlParameter("@kw", "%" + keyword + "%")); }
            sql += " ORDER BY name";
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars.ToArray());
            _gridUsers.DataSource = dt;
            if (_gridUsers.Columns.Contains("id")) _gridUsers.Columns["id"].Visible = false;
            if (_gridUsers.Columns.Contains("department_id")) _gridUsers.Columns["department_id"].Visible = false;
            if (_gridUsers.Columns.Contains("name")) _gridUsers.Columns["name"].HeaderText = "이름";
            if (_gridUsers.Columns.Contains("email")) _gridUsers.Columns["email"].HeaderText = "이메일";
            if (_gridUsers.Columns.Contains("department")) _gridUsers.Columns["department"].HeaderText = "부서";
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
            sql.Append("SELECT s.full_name AS sender, r.full_name AS receiver, c.message AS message, c.created_at AS created_at ");
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
            if (_gridChat.Columns.Contains("sender")) _gridChat.Columns["sender"].HeaderText = "보낸사람";
            if (_gridChat.Columns.Contains("receiver")) _gridChat.Columns["receiver"].HeaderText = "받는사람";
            if (_gridChat.Columns.Contains("message")) _gridChat.Columns["message"].HeaderText = "내용";
            if (_gridChat.Columns.Contains("created_at")) _gridChat.Columns["created_at"].HeaderText = "시간";
        }

        private void SearchAccessLogs()
        {
            if (_gridLogs == null) return;

            DataTable GetAccessLogs(DateTime start, DateTime end, string keyword)
            {
                string sql = @"
                    SELECT l.created_at, u.full_name, u.email, l.activity_type, l.user_id
                    FROM user_activity_logs l
                    JOIN users u ON l.user_id = u.id
                    WHERE l.created_at >= @start_date AND l.created_at < @end_date
                      AND u.company_id = @company_id";

                var parameters = new System.Collections.Generic.List<MySqlParameter>
                {
                    new MySqlParameter("@start_date", start),
                    new MySqlParameter("@end_date", end),
                    new MySqlParameter("@company_id", _companyId)
                };

                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " AND (u.full_name LIKE @keyword OR u.email LIKE @keyword OR CAST(u.id AS CHAR) LIKE @keyword)";
                    parameters.Add(new MySqlParameter("@keyword", $"%{keyword}%"));
                }

                sql += " ORDER BY l.created_at DESC";

                return DBManager.Instance.ExecuteDataTable(sql, parameters.ToArray());
            }

            try
            {
                DateTime startDate = dtpStart.Value.Date;
                DateTime endDate = dtpEnd.Value.Date.AddDays(1);
                string searchKeyword = txtSearchUser.Text.Trim();

                var dt = GetAccessLogs(startDate, endDate, searchKeyword);

                _gridLogs.DataSource = dt;

                if (_gridLogs.Columns.Contains("created_at"))
                {
                    _gridLogs.Columns["created_at"].HeaderText = "시간";
                    _gridLogs.Columns["created_at"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }
                if (_gridLogs.Columns.Contains("full_name")) _gridLogs.Columns["full_name"].HeaderText = "사용자명";
                if (_gridLogs.Columns.Contains("activity_type")) _gridLogs.Columns["activity_type"].HeaderText = "활동";
                if (_gridLogs.Columns.Contains("user_id")) _gridLogs.Columns["user_id"].HeaderText = "사용자 ID";

                if (_gridLogs.Columns.Contains("email")) _gridLogs.Columns["email"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그 검색 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}