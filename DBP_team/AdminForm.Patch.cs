using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class AdminForm
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                // Ensure event subscriptions exist for combos so designer remains happy and runtime works
                if (_cboDeptForUser != null)
                {
                    _cboDeptForUser.SelectedIndexChanged -= CboDeptForUser_SelectedIndexChanged;
                    _cboDeptForUser.SelectedIndexChanged += CboDeptForUser_SelectedIndexChanged;
                    // Initialize team combo for selected dept
                    CboDeptForUser_SelectedIndexChanged(_cboDeptForUser, EventArgs.Empty);
                }
                if (_cboDeptForTeam != null)
                {
                    _cboDeptForTeam.SelectedIndexChanged -= TeamDept_SelectedIndexChanged;
                    _cboDeptForTeam.SelectedIndexChanged += TeamDept_SelectedIndexChanged;
                    // Initialize team management tab contents
                    LoadDeptComboForTeamTab();
                    LoadTeamsGridForSelectedDept();
                }
            }
            catch { }
        }

        private void CboDeptForUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_cboDeptForUser.SelectedValue == null) { _cboTeamForUser.DataSource = null; return; }
                int did;
                if (!int.TryParse(Convert.ToString(_cboDeptForUser.SelectedValue), out did)) { _cboTeamForUser.DataSource = null; return; }
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM teams WHERE department_id=@did ORDER BY name",
                    new MySqlParameter("@did", did));
                _cboTeamForUser.DataSource = dt;
                _cboTeamForUser.DisplayMember = "name";
                _cboTeamForUser.ValueMember = "id";
                _cboTeamForUser.SelectedIndex = -1;
            }
            catch { }
        }

        // Team management tab handlers
        private void TeamDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeamsGridForSelectedDept();
            _txtTeamName.Text = string.Empty;
        }

        private void TeamGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var nameObj = _gridTeam.Rows[e.RowIndex].Cells["name"].Value;
                _txtTeamName.Text = Convert.ToString(nameObj);
            }
        }

        private void TeamAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cboDeptForTeam.SelectedValue == null) { MessageBox.Show("부서를 선택하세요."); return; }
                int did;
                if (!int.TryParse(Convert.ToString(_cboDeptForTeam.SelectedValue), out did)) { MessageBox.Show("부서를 선택하세요."); return; }
                var name = _txtTeamName.Text?.Trim();
                if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("팀명을 입력하세요."); return; }
                DBManager.Instance.ExecuteNonQuery(
                    "INSERT INTO teams (department_id, name) VALUES (@did, @name)",
                    new MySqlParameter("@did", did), new MySqlParameter("@name", name));
                LoadTeamsGridForSelectedDept();
                LoadUsersGrid(_txtUserSearch.Text?.Trim());
                InitializePermissionTree();
            }
            catch (Exception ex)
            {
                MessageBox.Show("팀 추가 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TeamUpdate_Click(object sender, EventArgs e)
        {
            if (_gridTeam.CurrentRow == null) { MessageBox.Show("수정할 팀을 선택하세요."); return; }
            var drv = _gridTeam.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) { MessageBox.Show("선택 행 데이터를 읽을 수 없습니다."); return; }
            var id = Convert.ToInt32(drv["id"]);
            var name = _txtTeamName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("팀명을 입력하세요."); return; }
            DBManager.Instance.ExecuteNonQuery(
                "UPDATE teams SET name=@name WHERE id=@id",
                new MySqlParameter("@name", name), new MySqlParameter("@id", id));
            LoadTeamsGridForSelectedDept(_txtTeamSearch.Text?.Trim());
            LoadUsersGrid(_txtUserSearch.Text?.Trim());
            InitializePermissionTree();
        }

        private void TeamDelete_Click(object sender, EventArgs e)
        {
            if (_gridTeam.SelectedRows.Count == 0) { MessageBox.Show("삭제할 팀을 선택하세요."); return; }
            if (MessageBox.Show("선택된 팀을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (DataGridViewRow row in _gridTeam.SelectedRows)
            {
                var drv = row.DataBoundItem as DataRowView;
                if (drv == null) continue;
                int tid = Convert.ToInt32(drv["id"]);
                DBManager.Instance.ExecuteNonQuery("DELETE FROM teams WHERE id=@id", new MySqlParameter("@id", tid));
            }
            LoadTeamsGridForSelectedDept(_txtTeamSearch.Text?.Trim());
            LoadUsersGrid(_txtUserSearch.Text?.Trim());
            InitializePermissionTree();
        }

        private void TeamSearch_Click(object sender, EventArgs e)
        {
            LoadTeamsGridForSelectedDept(_txtTeamSearch.Text?.Trim());
        }

        private void LoadDeptComboForTeamTab()
        {
            var dt = DBManager.Instance.ExecuteDataTable(
                "SELECT id, name FROM departments WHERE company_id=@cid ORDER BY name",
                new MySqlParameter("@cid", _companyId));
            _cboDeptForTeam.DataSource = dt;
            _cboDeptForTeam.DisplayMember = "name";
            _cboDeptForTeam.ValueMember = "id";
            _cboDeptForTeam.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
        }

        private void LoadTeamsGridForSelectedDept(string keyword = null)
        {
            if (_cboDeptForTeam.SelectedValue == null) { _gridTeam.DataSource = null; return; }
            int deptId;
            if (!int.TryParse(Convert.ToString(_cboDeptForTeam.SelectedValue), out deptId)) { _gridTeam.DataSource = null; return; }
            var sql = "SELECT t.id, t.name, t.department_id, d.name AS department FROM teams t JOIN departments d ON d.id=t.department_id WHERE t.department_id=@did" +
                      (string.IsNullOrWhiteSpace(keyword) ? "" : " AND t.name LIKE @kw") +
                      " ORDER BY t.name";
            var pars = new List<MySqlParameter> { new MySqlParameter("@did", deptId) };
            if (!string.IsNullOrWhiteSpace(keyword)) pars.Add(new MySqlParameter("@kw", "%" + keyword + "%"));
            var dt = DBManager.Instance.ExecuteDataTable(sql, pars.ToArray());
            _gridTeam.DataSource = dt;
            if (_gridTeam.Columns.Contains("id")) _gridTeam.Columns["id"].Visible = false;
            if (_gridTeam.Columns.Contains("department_id")) _gridTeam.Columns["department_id"].Visible = false;
            if (_gridTeam.Columns.Contains("name")) _gridTeam.Columns["name"].HeaderText = "팀명";
            if (_gridTeam.Columns.Contains("department")) _gridTeam.Columns["department"].HeaderText = "부서";
        }
    }
}
