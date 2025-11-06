using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DBP_team.Controls;

namespace DBP_team
{
    public partial class ChatForm : Form
    {
        private readonly int _myUserId;
        private readonly int _otherUserId;
        private readonly string _otherName;
        private FlowLayoutPanel _flow;

        public ChatForm(int myUserId, int otherUserId, string otherName)
        {
            InitializeComponent();

            _myUserId = myUserId;
            _otherUserId = otherUserId;
            _otherName = string.IsNullOrWhiteSpace(otherName) ? "상대" : otherName;

            labelChat.Text = _otherName;

            // 기존 listChat(디자이너)에 의존하므로 숨기고 FlowLayoutPanel을 추가
            if (this.Controls.Contains(listChat)) listChat.Visible = false;

            _flow = new FlowLayoutPanel
            {
                Location = listChat.Location,
                Size = listChat.Size,
                Anchor = listChat.Anchor,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = listChat.BackColor
            };

            this.Controls.Add(_flow);
            _flow.BringToFront();

            _flow.Resize += (s, e) =>
            {
                // 폭 변경 시 각 bubble 재계산
                foreach (Control c in _flow.Controls)
                {
                    if (c is ChatBubbleControl bubble)
                    {
                        // Tag에 저장된 데이터로 재설정
                        var t = bubble.Tag as Tuple<string, DateTime, bool, int>;
                        if (t != null)
                        {
                            bubble.Width = _flow.ClientSize.Width;
                            bubble.SetData(t.Item1, t.Item2, t.Item3, _flow.ClientSize.Width);
                        }
                    }
                }
            };

            btnSend.Text = "전송";

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

                _flow.Controls.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    var lbl = new Label { Text = "대화가 없습니다.", AutoSize = true, ForeColor = Color.Gray, Margin = new Padding(10) };
                    _flow.Controls.Add(lbl);
                    return;
                }

                foreach (DataRow r in dt.Rows)
                {
                    var id = Convert.ToInt32(r["id"]);
                    var time = r["created_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["created_at"]);
                    var senderId = Convert.ToInt32(r["sender_id"]);
                    var whoMine = senderId == _myUserId;
                    var message = r["message"]?.ToString() ?? "";

                    var bubble = CreateBubble(message, time, whoMine, id);
                    _flow.Controls.Add(bubble);
                }

                if (_flow.Controls.Count > 0)
                    _flow.ScrollControlIntoView(_flow.Controls[_flow.Controls.Count - 1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("채팅 불러오는 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ChatBubbleControl CreateBubble(string message, DateTime time, bool isMine, int id)
        {
            var bubble = new ChatBubbleControl();
            bubble.Width = Math.Max(200, _flow.ClientSize.Width);
            // tag에 데이터를 저장해 resize 시 재설정에 사용
            bubble.Tag = Tuple.Create(message, time, isMine, id);
            bubble.SetData(message, time, isMine, _flow.ClientSize.Width);
            bubble.Margin = new Padding(0, 6, 0, 6);
            return bubble;
        }

        // 즉시 하단에 새 말풍선 추가 (전송 후 호출)
        private void AddBubbleImmediate(string message, DateTime time, bool isMine, int id)
        {
            var bubble = CreateBubble(message, time, isMine, id);
            _flow.Controls.Add(bubble);
            _flow.ScrollControlIntoView(bubble);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 필요 시 구현
        }

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

                var sentTime = DateTime.Now;
                txtChat.Clear();
                txtChat.Focus();

                // 즉시 UI에 추가
                AddBubbleImmediate(text, sentTime, true, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("메시지 전송 중 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void ChatForm_Load(object sender, EventArgs e)
        {

        }
    }
}
