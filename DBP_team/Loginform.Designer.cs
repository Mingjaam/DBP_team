namespace DBP_team
{
    partial class Loginform
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.chkAutoLogin = new System.Windows.Forms.CheckBox();
            this.btnTogglePwd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.White;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtId.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtId.Location = new System.Drawing.Point(60, 240);
            this.txtId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(320, 29);
            this.txtId.TabIndex = 0;
            this.txtId.Text = "아이디";
            this.txtId.TextChanged += new System.EventHandler(this.txtId_TextChanged);
            // 
            // txtPwd
            // 
            this.txtPwd.BackColor = System.Drawing.Color.White;
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwd.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtPwd.Location = new System.Drawing.Point(60, 285);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(260, 29);
            this.txtPwd.TabIndex = 1;
            this.txtPwd.Text = "비밀번호";
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(60, 375);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(320, 45);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.label1.Location = new System.Drawing.Point(120, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 51);
            this.label1.TabIndex = 3;
            this.label1.Text = "DBP Talk";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkRegister
            // 
            this.linkRegister.AutoSize = true;
            this.linkRegister.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.linkRegister.Location = new System.Drawing.Point(180, 435);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(67, 15);
            this.linkRegister.TabIndex = 4;
            this.linkRegister.TabStop = true;
            this.linkRegister.Text = "회원가입";
            this.linkRegister.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkRemember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.chkRemember.Location = new System.Drawing.Point(60, 330);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(107, 19);
            this.chkRemember.TabIndex = 5;
            this.chkRemember.Text = "ID/PW 저장";
            this.chkRemember.UseVisualStyleBackColor = true;
            this.chkRemember.CheckedChanged += new System.EventHandler(this.chkRemember_CheckedChanged);
            // 
            // chkAutoLogin
            // 
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkAutoLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.chkAutoLogin.Location = new System.Drawing.Point(200, 330);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(109, 19);
            this.chkAutoLogin.TabIndex = 6;
            this.chkAutoLogin.Text = "자동 로그인";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            this.chkAutoLogin.CheckedChanged += new System.EventHandler(this.chkAutoLogin_CheckedChanged);
            // 
            // btnTogglePwd
            // 
            this.btnTogglePwd.BackColor = System.Drawing.Color.White;
            this.btnTogglePwd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTogglePwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePwd.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.btnTogglePwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnTogglePwd.Location = new System.Drawing.Point(328, 285);
            this.btnTogglePwd.Name = "btnTogglePwd";
            this.btnTogglePwd.Size = new System.Drawing.Size(52, 29);
            this.btnTogglePwd.TabIndex = 7;
            this.btnTogglePwd.Text = "보기";
            this.btnTogglePwd.UseVisualStyleBackColor = false;
            this.btnTogglePwd.Click += new System.EventHandler(this.btnTogglePwd_Click);
            // 
            // Loginform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(440, 550);
            this.Controls.Add(this.btnTogglePwd);
            this.Controls.Add(this.chkAutoLogin);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.linkRegister);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Loginform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBP Talk - 로그인";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.CheckBox chkAutoLogin;
        private System.Windows.Forms.Button btnTogglePwd;
    }
}