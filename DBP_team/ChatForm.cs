using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public partial class ChatForm : Form
    {
        private readonly int _myUserId;
        private readonly int _otherUserId;
        private readonly string _otherName;

        public ChatForm(int myUserId, int otherUserId, string otherName)
        {
            InitializeComponent();

            _myUserId = myUserId;
            _otherUserId = otherUserId;
            _otherName = string.IsNullOrWhiteSpace(otherName) ? "상대" : otherName;

            // UI 초기화
            labelChat.Text = _otherName;

            // ListView 설정 (디자이너에 컬럼 없을 수 있어 코드에서 안전하게 설정)
            listChat.View = View.Details;
            listChat.FullRowSelect = true;
            listChat.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listChat.Columns.Clear();
            listChat.Columns.Add("시간", 140);
            listChat.Columns.Add("보낸사람", 80);
            listChat.Columns.Add("메시지", 400);
            listChat.MultiSelect = false;

            LoadMessages();
        }

        private void LoadMessages()
        {
            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, sender_id, receiver_id, message, created_at, is_read " +
                    "FROM chat " +
                    "WHERE (sender_id = @me AND receiver_id = @other) OR (sender_id = @other AND receiver_id = @me) " +
                    "ORDER BY created_at ASC",
                    new MySqlParameter("@me", _myUserId),
                    new MySqlParameter("@other", _otherUserId));

                listChat.Items.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    var li = new ListViewItem(new[] { "", "", "대화가 없습니다." });
                    listChat.Items.Add(li);
                    return;
                }

                foreach (DataRow r in dt.Rows)
                {
                    var time = r["created_at"] == DBNull.Value ? "" : Convert.ToDateTime(r["created_at"]).ToString("yyyy-MM-dd HH:mm");
                    var senderId = Convert.ToInt32(r["sender_id"]);
                    var who = senderId == _myUserId ? "나" : _otherName;
                    var msg = r["message"]?.ToString() ?? "";

                    var item = new ListViewItem(new[] { time, who, msg });
                    item.Tag = r["id"];
                    listChat.Items.Add(item);
                }

                // 최신 메시지가 아래에 오도록 마지막 항목이 보이게 함
                if (listChat.Items.Count > 0)
                    listChat.EnsureVisible(listChat.Items.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("채팅 불러오는 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 디자이너에서 연결된 이벤트 이름 유지: listView1_SelectedIndexChanged
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택 시 동작이 필요하면 구현
        }

        // 디자이너에서 btnSend 클릭에 연결된 메서드 이름: button1_Click
        private void button1_Click(object sender, EventArgs e)
        {
            var text = txtChat.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("메시지를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var sql = "INSERT INTO chat (sender_id, receiver_id, message) VALUES (@s, @r, @msg)";
                DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@s", _myUserId),
                    new MySqlParameter("@r", _otherUserId),
                    new MySqlParameter("@msg", text));

                // 전송 후 입력 초기화 및 최신 메시지 보이도록 다시 로드
                txtChat.Clear();
                txtChat.Focus();

                LoadMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show("메시지 전송 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
