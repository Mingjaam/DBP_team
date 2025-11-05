using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Models;


namespace DBP_team
{
    public partial class MainForm : Form
    {
        private int _companyId;
        private string _companyName;
        private string _userName;
        private bool _initializedFromCtor;
        private int _userId; // 로그인된 사용자 id

        public MainForm()
        {
            InitializeComponent();
            HookTreeEvents();
        }

        // User 객체로 초기화
        public MainForm(User user) : this()
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _userId = user.Id;
            _companyId = user.CompanyId ?? 0;
            _userName = string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName;
            _companyName = user.CompanyName; // 로그인 시 전달된 companyName 우선 사용

            // DB에서 회사명을 가져와야 하면 여기서 보완: 전달된 값이 비어있으면 DB에서 조회
            if (string.IsNullOrWhiteSpace(_companyName) && _companyId > 0)
            {
                try
                {
                    var obj = DBManager.Instance.ExecuteScalar(
                        "SELECT name FROM companies WHERE id = @id",
                        new MySqlParameter("@id", _companyId));
                    if (obj != null && obj != DBNull.Value)
                        _companyName = obj.ToString();
                }
                catch
                {
                    // 실패하면 _companyName은 null로 두고 아래에서 폴백 텍스트 사용
                }
            }

            _initializedFromCtor = true;

            labelCompany.Text = string.IsNullOrWhiteSpace(_companyName) ? $"회사 ID:{_companyId}" : _companyName;
            labelName.Text = string.IsNullOrWhiteSpace(_userName) ? "사용자: (알 수 없음)" : $"사용자: {_userName}";

            LoadCompanyTree();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_initializedFromCtor) return;

            if (_companyId > 0)
            {
                // 런타임 상황에 따라 회사명을 DB에서 보완 조회
                if (string.IsNullOrWhiteSpace(_companyName))
                {
                    try
                    {
                        var obj = DBManager.Instance.ExecuteScalar(
                            "SELECT name FROM companies WHERE id = @id",
                            new MySqlParameter("@id", _companyId));
                        if (obj != null && obj != DBNull.Value)
                            _companyName = obj.ToString();
                    }
                    catch
                    {
                        // 무시
                    }
                }

                labelCompany.Text = string.IsNullOrWhiteSpace(_companyName) ? $"회사 ID:{_companyId}" : _companyName;
                labelName.Text = string.IsNullOrWhiteSpace(_userName) ? "사용자: (알 수 없음)" : $"사용자: {_userName}";
                LoadCompanyTree();
            }
            else
            {
                labelCompany.Text = "회사 정보 없음";
                labelName.Text = "사용자 정보 없음";
            }
        }

        // 트리뷰 더블클릭 이벤트 훅
        private void HookTreeEvents()
        {
            treeViewUser.NodeMouseDoubleClick -= TreeViewUser_NodeMouseDoubleClick;
            treeViewUser.NodeMouseDoubleClick += TreeViewUser_NodeMouseDoubleClick;
        }

        // 트리 노드 더블클릭 핸들러: user:ID 태그를 파싱해 ChatForm 열기
        private void TreeViewUser_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;
            var tag = e.Node.Tag as string;
            if (string.IsNullOrWhiteSpace(tag)) return;

            // tag 형식: "user:123"
            if (tag.StartsWith("user:", StringComparison.OrdinalIgnoreCase))
            {
                var parts = tag.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1], out int otherId))
                {
                    var otherName = e.Node.Text;
                    // 로그인된 사용자 id(_userId) 가 있어야 함
                    if (_userId <= 0)
                    {
                        MessageBox.Show("로그인 사용자 정보가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var chat = new ChatForm(_userId, otherId, otherName);
                    chat.StartPosition = FormStartPosition.CenterParent;
                    chat.Show(this);
                }
            }
        }

        // 기존 LoadCompanyTree() 메서드 그대로 유지
        private void LoadCompanyTree()
        {
            treeViewUser.BeginUpdate();
            treeViewUser.Nodes.Clear();

            try
            {
                if (_companyId <= 0)
                {
                    treeViewUser.Nodes.Add(new TreeNode("회사 정보가 없습니다."));
                    return;
                }

                var dtDeps = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, name FROM departments WHERE company_id = @cid ORDER BY name",
                    new MySqlParameter("@cid", _companyId));

                if (dtDeps == null || dtDeps.Rows.Count == 0)
                {
                    treeViewUser.Nodes.Add(new TreeNode("등록된 부서가 없습니다."));
                    return;
                }

                foreach (DataRow dep in dtDeps.Rows)
                {
                    int depId = Convert.ToInt32(dep["id"]);
                    string depName = dep["name"]?.ToString() ?? $"부서 {depId}";
                    var depNode = new TreeNode(depName) { Tag = $"department:{depId}" };

                    var dtTeams = DBManager.Instance.ExecuteDataTable(
                        "SELECT id, name FROM teams WHERE department_id = @did ORDER BY name",
                        new MySqlParameter("@did", depId));

                    if (dtTeams != null && dtTeams.Rows.Count > 0)
                    {
                        foreach (DataRow team in dtTeams.Rows)
                        {
                            int teamId = Convert.ToInt32(team["id"]);
                            string teamName = team["name"]?.ToString() ?? $"팀 {teamId}";
                            var teamNode = new TreeNode(teamName) { Tag = $"team:{teamId}" };

                            var dtUsersInTeam = DBManager.Instance.ExecuteDataTable(
                                "SELECT id, full_name, email, role FROM users WHERE company_id = @cid AND department_id = @did AND team_id = @tid ORDER BY full_name",
                                new MySqlParameter("@cid", _companyId),
                                new MySqlParameter("@did", depId),
                                new MySqlParameter("@tid", teamId));

                            if (dtUsersInTeam != null && dtUsersInTeam.Rows.Count > 0)
                            {
                                foreach (DataRow u in dtUsersInTeam.Rows)
                                {
                                    var display = u["full_name"]?.ToString();
                                    if (string.IsNullOrWhiteSpace(display)) display = u["email"]?.ToString() ?? "이름 없음";
                                    var userNode = new TreeNode(display) { Tag = $"user:{u["id"]}" };
                                    teamNode.Nodes.Add(userNode);
                                }
                            }

                            depNode.Nodes.Add(teamNode);
                        }
                    }

                    var dtUsersNoTeam = DBManager.Instance.ExecuteDataTable(
                        "SELECT id, full_name, email, role FROM users WHERE company_id = @cid AND department_id = @did AND (team_id IS NULL OR team_id = 0) ORDER BY full_name",
                        new MySqlParameter("@cid", _companyId),
                        new MySqlParameter("@did", depId));

                    if (dtUsersNoTeam != null && dtUsersNoTeam.Rows.Count > 0)
                    {
                        foreach (DataRow u in dtUsersNoTeam.Rows)
                        {
                            var display = u["full_name"]?.ToString();
                            if (string.IsNullOrWhiteSpace(display)) display = u["email"]?.ToString() ?? "이름 없음";
                            var userNode = new TreeNode(display) { Tag = $"user:{u["id"]}" };
                            depNode.Nodes.Add(userNode);
                        }
                    }

                    treeViewUser.Nodes.Add(depNode);
                }

                treeViewUser.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("트리 로드 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                treeViewUser.EndUpdate();
            }
        }
    }
}
