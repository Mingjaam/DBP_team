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
            this.SuspendLayout();
            // 
            // listChat
            // 
            this.listChat.HideSelection = false;
            this.listChat.Location = new System.Drawing.Point(39, 79);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(304, 434);
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
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 582);
            this.Controls.Add(this.labelChat);
            this.Controls.Add(this.listChat);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listChat;
        private System.Windows.Forms.Label labelChat;
    }
}