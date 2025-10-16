namespace WindowsFormsApp1
{
    partial class AdminDashboardForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnModules = new System.Windows.Forms.Button();
            this.btnPartners = new System.Windows.Forms.Button();
            this.btnParticipants = new System.Windows.Forms.Button();
            this.btnAssessments = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.btnViewEnrollmentRequests = new System.Windows.Forms.Button();
            this.pnlNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(11, 20);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(344, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Digital Literacy Management";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblWelcome.Location = new System.Drawing.Point(26, 52);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(224, 21);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome, [User Name] ([Role])";
            // 
            // btnModules
            // 
            this.btnModules.BackColor = System.Drawing.Color.LightBlue;
            this.btnModules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModules.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnModules.Location = new System.Drawing.Point(9, 8);
            this.btnModules.Margin = new System.Windows.Forms.Padding(2);
            this.btnModules.Name = "btnModules";
            this.btnModules.Size = new System.Drawing.Size(150, 50);
            this.btnModules.TabIndex = 2;
            this.btnModules.Text = "Maintain Training Modules";
            this.btnModules.UseVisualStyleBackColor = false;
            this.btnModules.Click += new System.EventHandler(this.btnModules_Click_1);
            // 
            // btnPartners
            // 
            this.btnPartners.BackColor = System.Drawing.Color.LightBlue;
            this.btnPartners.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPartners.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnPartners.Location = new System.Drawing.Point(164, 8);
            this.btnPartners.Margin = new System.Windows.Forms.Padding(2);
            this.btnPartners.Name = "btnPartners";
            this.btnPartners.Size = new System.Drawing.Size(150, 50);
            this.btnPartners.TabIndex = 3;
            this.btnPartners.Text = "Maintain Community Partners";
            this.btnPartners.UseVisualStyleBackColor = false;
            this.btnPartners.Click += new System.EventHandler(this.btnPartners_Click_1);
            // 
            // btnParticipants
            // 
            this.btnParticipants.BackColor = System.Drawing.Color.LightBlue;
            this.btnParticipants.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParticipants.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnParticipants.Location = new System.Drawing.Point(9, 62);
            this.btnParticipants.Margin = new System.Windows.Forms.Padding(2);
            this.btnParticipants.Name = "btnParticipants";
            this.btnParticipants.Size = new System.Drawing.Size(150, 50);
            this.btnParticipants.TabIndex = 4;
            this.btnParticipants.Text = "Maintain Participants";
            this.btnParticipants.UseVisualStyleBackColor = false;
            this.btnParticipants.Click += new System.EventHandler(this.btnParticipants_Click_1);
            // 
            // btnAssessments
            // 
            this.btnAssessments.BackColor = System.Drawing.Color.LightBlue;
            this.btnAssessments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssessments.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnAssessments.Location = new System.Drawing.Point(164, 62);
            this.btnAssessments.Margin = new System.Windows.Forms.Padding(2);
            this.btnAssessments.Name = "btnAssessments";
            this.btnAssessments.Size = new System.Drawing.Size(150, 50);
            this.btnAssessments.TabIndex = 5;
            this.btnAssessments.Text = "Maintain Assessments";
            this.btnAssessments.UseVisualStyleBackColor = false;
            this.btnAssessments.Click += new System.EventHandler(this.btnAssessments_Click_1);
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.Color.LightCoral;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnReports.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnReports.Location = new System.Drawing.Point(164, 116);
            this.btnReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(150, 50);
            this.btnReports.TabIndex = 6;
            this.btnReports.Text = "Generate Reports";
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click_1);
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.Controls.Add(this.btnViewEnrollmentRequests);
            this.pnlNavigation.Controls.Add(this.btnModules);
            this.pnlNavigation.Controls.Add(this.btnReports);
            this.pnlNavigation.Controls.Add(this.btnPartners);
            this.pnlNavigation.Controls.Add(this.btnAssessments);
            this.pnlNavigation.Controls.Add(this.btnParticipants);
            this.pnlNavigation.Location = new System.Drawing.Point(19, 84);
            this.pnlNavigation.Margin = new System.Windows.Forms.Padding(2);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(322, 178);
            this.pnlNavigation.TabIndex = 7;
            // 
            // btnViewEnrollmentRequests
            // 
            this.btnViewEnrollmentRequests.BackColor = System.Drawing.Color.LightCoral;
            this.btnViewEnrollmentRequests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewEnrollmentRequests.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnViewEnrollmentRequests.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnViewEnrollmentRequests.Location = new System.Drawing.Point(11, 116);
            this.btnViewEnrollmentRequests.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewEnrollmentRequests.Name = "btnViewEnrollmentRequests";
            this.btnViewEnrollmentRequests.Size = new System.Drawing.Size(150, 50);
            this.btnViewEnrollmentRequests.TabIndex = 7;
            this.btnViewEnrollmentRequests.Text = "View Enrollment Requests";
            this.btnViewEnrollmentRequests.UseVisualStyleBackColor = false;
            this.btnViewEnrollmentRequests.Click += new System.EventHandler(this.btnViewEnrollmentRequests_Click);
            // 
            // AdminDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 273);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "AdminDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.pnlNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnModules;
        private System.Windows.Forms.Button btnPartners;
        private System.Windows.Forms.Button btnParticipants;
        private System.Windows.Forms.Button btnAssessments;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Button btnViewEnrollmentRequests;
    }

}