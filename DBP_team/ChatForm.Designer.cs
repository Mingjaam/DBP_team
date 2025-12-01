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
            this.SuspendLayout();
            // 
            // listChat
            // 
            this.listChat.HideSelection = false;
            this.listChat.Location = new System.Drawing.Point(12, 57);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(355, 513);
            this.listChat.TabIndex = 0;
            this.listChat.UseCompatibleStateImageBehavior = false;
            this.listChat.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // labelChat
            // 
            this.labelChat.AutoSize = true;
            this.labelChat.Location = new System.Drawing.Point(39, 28);
            this.labelChat.Name = "labelChat";
            this.labelChat.Size = new System.Drawing.Size(38, 12);
            this.labelChat.TabIndex = 1;
            this.labelChat.Text = "label1";
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(12, 593);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(200, 21);
            this.txtChat.TabIndex = 2;
            this.txtChat.TextChanged += new System.EventHandler(this.txtChat_TextChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(292, 591);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "전송";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 28);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 21);
            this.txtSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(240, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSearchPrev
            // 
            this.btnSearchPrev.Location = new System.Drawing.Point(305, 26);
            this.btnSearchPrev.Name = "btnSearchPrev";
            this.btnSearchPrev.Size = new System.Drawing.Size(24, 23);
            this.btnSearchPrev.TabIndex = 8;
            this.btnSearchPrev.Text = "▲";
            this.btnSearchPrev.UseVisualStyleBackColor = true;
            this.btnSearchPrev.Click += new System.EventHandler(this.btnSearchPrev_Click);
            // 
            // btnSearchNext
            // 
            this.btnSearchNext.Location = new System.Drawing.Point(335, 26);
            this.btnSearchNext.Name = "btnSearchNext";
            this.btnSearchNext.Size = new System.Drawing.Size(24, 23);
            this.btnSearchNext.TabIndex = 9;
            this.btnSearchNext.Text = "▼";
            this.btnSearchNext.UseVisualStyleBackColor = true;
            this.btnSearchNext.Click += new System.EventHandler(this.btnSearchNext_Click);
            // 
            // lblSearchCount
            // 
            this.lblSearchCount.AutoSize = true;
            this.lblSearchCount.Location = new System.Drawing.Point(365, 31);
            this.lblSearchCount.Name = "lblSearchCount";
            this.lblSearchCount.Size = new System.Drawing.Size(35, 12);
            this.lblSearchCount.TabIndex = 10;
            this.lblSearchCount.Text = "0/0";
            // 
            // btnEmoji
            // 
            this.btnEmoji.Location = new System.Drawing.Point(220, 591);
            this.btnEmoji.Name = "btnEmoji";
            this.btnEmoji.Size = new System.Drawing.Size(34, 23);
            this.btnEmoji.TabIndex = 6;
            this.btnEmoji.Text = "😊";
            this.btnEmoji.UseVisualStyleBackColor = true;
            this.btnEmoji.Click += new System.EventHandler(this.btnEmoji_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(260, 591);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(30, 23);
            this.btnFile.TabIndex = 7;
            this.btnFile.Text = "📎";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 626);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnEmoji);
            this.Controls.Add(this.btnSearchNext);
            this.Controls.Add(this.btnSearchPrev);
            this.Controls.Add(this.lblSearchCount);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.labelChat);
            this.Controls.Add(this.listChat);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}