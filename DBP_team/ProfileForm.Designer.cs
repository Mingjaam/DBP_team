namespace DBP_team
{
    partial class ProfileForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureProfile = new System.Windows.Forms.PictureBox();
            this.labelFullName = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.labelTeam = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChangeImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureProfile
            // 
            this.pictureProfile.Location = new System.Drawing.Point(12, 12);
            this.pictureProfile.Name = "pictureProfile";
            this.pictureProfile.Size = new System.Drawing.Size(120, 120);
            this.pictureProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureProfile.TabIndex = 0;
            this.pictureProfile.TabStop = false;
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(150, 20);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(57, 13);
            this.labelFullName.TabIndex = 1;
            this.labelFullName.Text = "이름: (없음)";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(150, 45);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(65, 13);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "이메일: (없음)";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(150, 70);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(59, 13);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "회사: (없음)";
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(150, 95);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(67, 13);
            this.labelDepartment.TabIndex = 4;
            this.labelDepartment.Text = "부서: (없음)";
            // 
            // labelTeam
            // 
            this.labelTeam.AutoSize = true;
            this.labelTeam.Location = new System.Drawing.Point(150, 120);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(49, 13);
            this.labelTeam.TabIndex = 5;
            this.labelTeam.Text = "팀: (없음)";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(250, 180);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChangeImage
            // 
            this.btnChangeImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeImage.Location = new System.Drawing.Point(150, 180);
            this.btnChangeImage.Name = "btnChangeImage";
            this.btnChangeImage.Size = new System.Drawing.Size(94, 23);
            this.btnChangeImage.TabIndex = 6;
            this.btnChangeImage.Text = "이미지 변경";
            this.btnChangeImage.UseVisualStyleBackColor = true;
            this.btnChangeImage.Click += new System.EventHandler(this.btnChangeImage_Click);
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 215);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnChangeImage);
            this.Controls.Add(this.labelTeam);
            this.Controls.Add(this.labelDepartment);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelFullName);
            this.Controls.Add(this.pictureProfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "프로필";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureProfile;
        private System.Windows.Forms.Label labelFullName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.Label labelTeam;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChangeImage;
    }
}