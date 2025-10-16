using System;

namespace WindowsFormsApp1
{
    partial class ParticipantDashboardForm
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
            this.dgvAssessments = new System.Windows.Forms.DataGridView();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRequestEnrollment = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssessments)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 13);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(270, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Participant Dashboard";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblWelcome.Location = new System.Drawing.Point(19, 49);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(105, 21);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome, [P]!";
            // 
            // dgvAssessments
            // 
            this.dgvAssessments.AllowUserToAddRows = false;
            this.dgvAssessments.AllowUserToDeleteRows = false;
            this.dgvAssessments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAssessments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssessments.Location = new System.Drawing.Point(19, 94);
            this.dgvAssessments.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAssessments.Name = "dgvAssessments";
            this.dgvAssessments.ReadOnly = true;
            this.dgvAssessments.RowHeadersWidth = 51;
            this.dgvAssessments.RowTemplate.Height = 29;
            this.dgvAssessments.Size = new System.Drawing.Size(638, 195);
            this.dgvAssessments.TabIndex = 2;
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewHistory.Location = new System.Drawing.Point(529, 302);
            this.btnViewHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(128, 29);
            this.btnViewHistory.TabIndex = 3;
            this.btnViewHistory.Text = "View History";
            this.btnViewHistory.UseVisualStyleBackColor = true;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogout.Location = new System.Drawing.Point(19, 302);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 29);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(19, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Available Assessments:";
            // 
            // btnRequestEnrollment
            // 
            this.btnRequestEnrollment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRequestEnrollment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRequestEnrollment.Location = new System.Drawing.Point(359, 302);
            this.btnRequestEnrollment.Margin = new System.Windows.Forms.Padding(2);
            this.btnRequestEnrollment.Name = "btnRequestEnrollment";
            this.btnRequestEnrollment.Size = new System.Drawing.Size(149, 29);
            this.btnRequestEnrollment.TabIndex = 6;
            this.btnRequestEnrollment.Text = "Request Enrollment";
            this.btnRequestEnrollment.UseVisualStyleBackColor = true;
            // 
            // ParticipantDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 344);
            this.Controls.Add(this.btnRequestEnrollment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnViewHistory);
            this.Controls.Add(this.dgvAssessments);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ParticipantDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assessment Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssessments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.DataGridView dgvAssessments;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRequestEnrollment;
    }

}