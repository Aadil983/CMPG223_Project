using System;

namespace WindowsFormsApp1
{
    partial class AssessmentMaintenanceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvAssessments = new System.Windows.Forms.DataGridView();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.txtPassPercentage = new System.Windows.Forms.TextBox();
            this.lblPassPercentage = new System.Windows.Forms.Label();
            this.cmbPartner = new System.Windows.Forms.ComboBox();
            this.lblPartner = new System.Windows.Forms.Label();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtAssessmentID = new System.Windows.Forms.TextBox();
            this.lblAssessmentID = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssessments)).BeginInit();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 13);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(274, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Assessment Maintenance";
            // 
            // dgvAssessments
            // 
            this.dgvAssessments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssessments.Location = new System.Drawing.Point(15, 46);
            this.dgvAssessments.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAssessments.Name = "dgvAssessments";
            this.dgvAssessments.RowHeadersWidth = 51;
            this.dgvAssessments.RowTemplate.Height = 29;
            this.dgvAssessments.Size = new System.Drawing.Size(638, 162);
            this.dgvAssessments.TabIndex = 1;
            this.dgvAssessments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssessments_CellContentClick);
            // 
            // pnlInput
            // 
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInput.Controls.Add(this.txtPassPercentage);
            this.pnlInput.Controls.Add(this.lblPassPercentage);
            this.pnlInput.Controls.Add(this.cmbPartner);
            this.pnlInput.Controls.Add(this.lblPartner);
            this.pnlInput.Controls.Add(this.cmbModule);
            this.pnlInput.Controls.Add(this.lblModule);
            this.pnlInput.Controls.Add(this.btnClose);
            this.pnlInput.Controls.Add(this.txtAssessmentID);
            this.pnlInput.Controls.Add(this.lblAssessmentID);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.txtName);
            this.pnlInput.Controls.Add(this.lblName);
            this.pnlInput.Location = new System.Drawing.Point(15, 221);
            this.pnlInput.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(638, 157);
            this.pnlInput.TabIndex = 2;
            // 
            // txtPassPercentage
            // 
            this.txtPassPercentage.Location = new System.Drawing.Point(390, 26);
            this.txtPassPercentage.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassPercentage.Name = "txtPassPercentage";
            this.txtPassPercentage.Size = new System.Drawing.Size(91, 20);
            this.txtPassPercentage.TabIndex = 18;
            // 
            // lblPassPercentage
            // 
            this.lblPassPercentage.AutoSize = true;
            this.lblPassPercentage.Location = new System.Drawing.Point(390, 11);
            this.lblPassPercentage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassPercentage.Name = "lblPassPercentage";
            this.lblPassPercentage.Size = new System.Drawing.Size(80, 13);
            this.lblPassPercentage.TabIndex = 17;
            this.lblPassPercentage.Text = "Pass % (0-100):";
            // 
            // cmbPartner
            // 
            this.cmbPartner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartner.FormattingEnabled = true;
            this.cmbPartner.Location = new System.Drawing.Point(292, 72);
            this.cmbPartner.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPartner.Name = "cmbPartner";
            this.cmbPartner.Size = new System.Drawing.Size(241, 21);
            this.cmbPartner.TabIndex = 16;
            // 
            // lblPartner
            // 
            this.lblPartner.AutoSize = true;
            this.lblPartner.Location = new System.Drawing.Point(292, 57);
            this.lblPartner.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPartner.Name = "lblPartner";
            this.lblPartner.Size = new System.Drawing.Size(98, 13);
            this.lblPartner.TabIndex = 15;
            this.lblPartner.Text = "Community Partner:";
            // 
            // cmbModule
            // 
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(15, 72);
            this.cmbModule.Margin = new System.Windows.Forms.Padding(2);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(264, 21);
            this.cmbModule.TabIndex = 14;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(15, 57);
            this.lblModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(45, 13);
            this.lblModule.TabIndex = 13;
            this.lblModule.Text = "Module:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(540, 120);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtAssessmentID
            // 
            this.txtAssessmentID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAssessmentID.Location = new System.Drawing.Point(532, 26);
            this.txtAssessmentID.Margin = new System.Windows.Forms.Padding(2);
            this.txtAssessmentID.Name = "txtAssessmentID";
            this.txtAssessmentID.ReadOnly = true;
            this.txtAssessmentID.Size = new System.Drawing.Size(91, 20);
            this.txtAssessmentID.TabIndex = 11;
            // 
            // lblAssessmentID
            // 
            this.lblAssessmentID.AutoSize = true;
            this.lblAssessmentID.Location = new System.Drawing.Point(532, 11);
            this.lblAssessmentID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAssessmentID.Name = "lblAssessmentID";
            this.lblAssessmentID.Size = new System.Drawing.Size(80, 13);
            this.lblAssessmentID.TabIndex = 10;
            this.lblAssessmentID.Text = "Assessment ID:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(255, 120);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightSalmon;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.Location = new System.Drawing.Point(158, 120);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(82, 23);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(15, 120);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(128, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Assessment";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(15, 26);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(361, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(15, 11);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(97, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Assessment Name:";
            // 
            // AssessmentMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 390);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.dgvAssessments);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "AssessmentMaintenanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assessment Maintenance";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssessments)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvAssessments;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cmbPartner;
        private System.Windows.Forms.Label lblPartner;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.TextBox txtPassPercentage;
        private System.Windows.Forms.Label lblPassPercentage;
        private System.Windows.Forms.TextBox txtAssessmentID;
        private System.Windows.Forms.Label lblAssessmentID;
    }

}