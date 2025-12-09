namespace DBP_team
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listChat = new System.Windows.Forms.ListView();
            this.labelChat = new System.Windows.Forms.Label();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSearchPrev = new System.Windows.Forms.Button();
            this.btnSearchNext = new System.Windows.Forms.Button();
            this.lblSearchCount = new System.Windows.Forms.Label();
            this.btnEmoji = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.btnSearchTime = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // listChat
            // 
            this.listChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listChat.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.listChat.HideSelection = false;
            this.listChat.Location = new System.Drawing.Point(0, 105);
            this.listChat.Margin = new System.Windows.Forms.Padding(0);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(484, 432);
            this.listChat.TabIndex = 0;
            this.listChat.UseCompatibleStateImageBehavior = false;
            this.listChat.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // labelChat
            // 
            this.labelChat.AutoSize = true;
            this.labelChat.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.labelChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.labelChat.Location = new System.Drawing.Point(15, 10);
            this.labelChat.Name = "labelChat";
            this.labelChat.Size = new System.Drawing.Size(74, 21);
            this.labelChat.TabIndex = 1;
            this.labelChat.Text = "채팅방명";
            // 
            // txtChat
            // 
            this.txtChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChat.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtChat.Location = new System.Drawing.Point(10, 10);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(270, 25);
            this.txtChat.TabIndex = 2;
            this.txtChat.TextChanged += new System.EventHandler(this.txtChat_TextChanged);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(144)))), ((int)(((byte)(226)))));
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(396, 10);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(66, 22);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "전송";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.txtSearch.Location = new System.Drawing.Point(15, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 23);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.Text = "메시지 검색...";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnSearch.Location = new System.Drawing.Point(275, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSearchPrev
            // 
            this.btnSearchPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnSearchPrev.FlatAppearance.BorderSize = 0;
            this.btnSearchPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchPrev.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSearchPrev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnSearchPrev.Location = new System.Drawing.Point(343, 40);
            this.btnSearchPrev.Name = "btnSearchPrev";
            this.btnSearchPrev.Size = new System.Drawing.Size(28, 23);
            this.btnSearchPrev.TabIndex = 8;
            this.btnSearchPrev.Text = "▲";
            this.btnSearchPrev.UseVisualStyleBackColor = false;
            this.btnSearchPrev.Click += new System.EventHandler(this.btnSearchPrev_Click);
            // 
            // btnSearchNext
            // 
            this.btnSearchNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnSearchNext.FlatAppearance.BorderSize = 0;
            this.btnSearchNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchNext.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnSearchNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnSearchNext.Location = new System.Drawing.Point(377, 40);
            this.btnSearchNext.Name = "btnSearchNext";
            this.btnSearchNext.Size = new System.Drawing.Size(28, 23);
            this.btnSearchNext.TabIndex = 9;
            this.btnSearchNext.Text = "▼";
            this.btnSearchNext.UseVisualStyleBackColor = false;
            this.btnSearchNext.Click += new System.EventHandler(this.btnSearchNext_Click);
            // 
            // lblSearchCount
            // 
            this.lblSearchCount.AutoSize = true;
            this.lblSearchCount.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblSearchCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblSearchCount.Location = new System.Drawing.Point(413, 44);
            this.lblSearchCount.Name = "lblSearchCount";
            this.lblSearchCount.Size = new System.Drawing.Size(26, 15);
            this.lblSearchCount.TabIndex = 10;
            this.lblSearchCount.Text = "0/0";
            // 
            // btnEmoji
            // 
            this.btnEmoji.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmoji.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnEmoji.FlatAppearance.BorderSize = 0;
            this.btnEmoji.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmoji.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnEmoji.Location = new System.Drawing.Point(292, 10);
            this.btnEmoji.Name = "btnEmoji";
            this.btnEmoji.Size = new System.Drawing.Size(45, 28);
            this.btnEmoji.TabIndex = 6;
            this.btnEmoji.Text = "😊";
            this.btnEmoji.UseVisualStyleBackColor = false;
            this.btnEmoji.Click += new System.EventHandler(this.btnEmoji_Click);
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnFile.FlatAppearance.BorderSize = 0;
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFile.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnFile.Location = new System.Drawing.Point(343, 10);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(45, 28);
            this.btnFile.TabIndex = 7;
            this.btnFile.Text = "📎";
            this.btnFile.UseVisualStyleBackColor = false;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.labelChat);
            this.pnlTop.Controls.Add(this.lblSearchCount);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnSearchNext);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.btnSearchPrev);
            this.pnlTop.Controls.Add(this.dtpStartTime);
            this.pnlTop.Controls.Add(this.dtpEndTime);
            this.pnlTop.Controls.Add(this.btnSearchTime);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(484, 105);
            this.pnlTop.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "~";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpStartTime.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(15, 65);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(140, 22);
            this.dtpStartTime.TabIndex = 11;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEndTime.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(185, 65);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(140, 22);
            this.dtpEndTime.TabIndex = 12;
            // 
            // btnSearchTime
            // 
            this.btnSearchTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnSearchTime.FlatAppearance.BorderSize = 0;
            this.btnSearchTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchTime.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSearchTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnSearchTime.Location = new System.Drawing.Point(335, 65);
            this.btnSearchTime.Name = "btnSearchTime";
            this.btnSearchTime.Size = new System.Drawing.Size(70, 30);
            this.btnSearchTime.TabIndex = 13;
            this.btnSearchTime.Text = "시간검색";
            this.btnSearchTime.UseVisualStyleBackColor = false;
            this.btnSearchTime.Click += new System.EventHandler(this.btnSearchTime_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlBottom.Controls.Add(this.txtChat);
            this.pnlBottom.Controls.Add(this.btnEmoji);
            this.pnlBottom.Controls.Add(this.btnFile);
            this.pnlBottom.Controls.Add(this.btnSend);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 540);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(484, 50);
            this.pnlBottom.TabIndex = 12;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 590);
            this.Controls.Add(this.listChat);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(400, 498);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "채팅";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listChat;
        private System.Windows.Forms.Label labelChat;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnEmoji;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnSearchPrev;
        private System.Windows.Forms.Button btnSearchNext;
        private System.Windows.Forms.Label lblSearchCount;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Button btnSearchTime;
        private System.Windows.Forms.Label label1;
    }
}