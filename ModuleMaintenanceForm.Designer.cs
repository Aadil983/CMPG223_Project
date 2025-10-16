namespace WindowsFormsApp1
{
    // START OF ModuleMaintenanceForm.Designer.cs

    partial class ModuleMaintenanceForm
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
            this.dgvModules = new System.Windows.Forms.DataGridView();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtModuleID = new System.Windows.Forms.TextBox();
            this.lblModuleID = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtDurationHours = new System.Windows.Forms.TextBox();
            this.lblDurationHours = new System.Windows.Forms.Label();
            this.txtContentLink = new System.Windows.Forms.TextBox();
            this.lblContentLink = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtModuleName = new System.Windows.Forms.TextBox();
            this.lblModuleName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(322, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Training Module Maintenance";
            // 
            // dgvModules
            // 
            this.dgvModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModules.Location = new System.Drawing.Point(15, 46);
            this.dgvModules.Margin = new System.Windows.Forms.Padding(2);
            this.dgvModules.Name = "dgvModules";
            this.dgvModules.RowHeadersWidth = 51;
            this.dgvModules.RowTemplate.Height = 29;
            this.dgvModules.Size = new System.Drawing.Size(562, 162);
            this.dgvModules.TabIndex = 1;
            this.dgvModules.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvModules_CellContentClick);
            // 
            // pnlInput
            // 
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInput.Controls.Add(this.btnClose);
            this.pnlInput.Controls.Add(this.cmbStatus);
            this.pnlInput.Controls.Add(this.lblStatus);
            this.pnlInput.Controls.Add(this.txtModuleID);
            this.pnlInput.Controls.Add(this.lblModuleID);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.txtDurationHours);
            this.pnlInput.Controls.Add(this.lblDurationHours);
            this.pnlInput.Controls.Add(this.txtContentLink);
            this.pnlInput.Controls.Add(this.lblContentLink);
            this.pnlInput.Controls.Add(this.txtDescription);
            this.pnlInput.Controls.Add(this.lblDescription);
            this.pnlInput.Controls.Add(this.txtModuleName);
            this.pnlInput.Controls.Add(this.lblModuleName);
            this.pnlInput.Location = new System.Drawing.Point(15, 221);
            this.pnlInput.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(563, 175);
            this.pnlInput.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(466, 138);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Active",
            "Draft",
            "Inactive"});
            this.cmbStatus.Location = new System.Drawing.Point(368, 97);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(181, 21);
            this.cmbStatus.TabIndex = 14;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(365, 82);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Status:";
            // 
            // txtModuleID
            // 
            this.txtModuleID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtModuleID.Location = new System.Drawing.Point(446, 13);
            this.txtModuleID.Margin = new System.Windows.Forms.Padding(2);
            this.txtModuleID.Name = "txtModuleID";
            this.txtModuleID.ReadOnly = true;
            this.txtModuleID.Size = new System.Drawing.Size(102, 20);
            this.txtModuleID.TabIndex = 12;
            // 
            // lblModuleID
            // 
            this.lblModuleID.AutoSize = true;
            this.lblModuleID.Location = new System.Drawing.Point(365, 16);
            this.lblModuleID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModuleID.Name = "lblModuleID";
            this.lblModuleID.Size = new System.Drawing.Size(59, 13);
            this.lblModuleID.TabIndex = 11;
            this.lblModuleID.Text = "Module ID:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(257, 138);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click_1);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightSalmon;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.Location = new System.Drawing.Point(161, 138);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(82, 23);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click_1);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(15, 138);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(128, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add Module";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtDurationHours
            // 
            this.txtDurationHours.Location = new System.Drawing.Point(466, 46);
            this.txtDurationHours.Margin = new System.Windows.Forms.Padding(2);
            this.txtDurationHours.Name = "txtDurationHours";
            this.txtDurationHours.Size = new System.Drawing.Size(54, 20);
            this.txtDurationHours.TabIndex = 7;
            // 
            // lblDurationHours
            // 
            this.lblDurationHours.AutoSize = true;
            this.lblDurationHours.Location = new System.Drawing.Point(364, 46);
            this.lblDurationHours.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDurationHours.Name = "lblDurationHours";
            this.lblDurationHours.Size = new System.Drawing.Size(87, 13);
            this.lblDurationHours.TabIndex = 6;
            this.lblDurationHours.Text = "Duration (Hours):";
            // 
            // txtContentLink
            // 
            this.txtContentLink.Location = new System.Drawing.Point(15, 97);
            this.txtContentLink.Margin = new System.Windows.Forms.Padding(2);
            this.txtContentLink.Name = "txtContentLink";
            this.txtContentLink.Size = new System.Drawing.Size(324, 20);
            this.txtContentLink.TabIndex = 5;
            // 
            // lblContentLink
            // 
            this.lblContentLink.AutoSize = true;
            this.lblContentLink.Location = new System.Drawing.Point(15, 82);
            this.lblContentLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContentLink.Name = "lblContentLink";
            this.lblContentLink.Size = new System.Drawing.Size(70, 13);
            this.lblContentLink.TabIndex = 4;
            this.lblContentLink.Text = "Content Link:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(15, 60);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(324, 20);
            this.txtDescription.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(15, 46);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description:";
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(15, 26);
            this.txtModuleName.Margin = new System.Windows.Forms.Padding(2);
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(324, 20);
            this.txtModuleName.TabIndex = 1;
            // 
            // lblModuleName
            // 
            this.lblModuleName.AutoSize = true;
            this.lblModuleName.Location = new System.Drawing.Point(15, 11);
            this.lblModuleName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModuleName.Name = "lblModuleName";
            this.lblModuleName.Size = new System.Drawing.Size(76, 13);
            this.lblModuleName.TabIndex = 0;
            this.lblModuleName.Text = "Module Name:";
            // 
            // ModuleMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 409);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.dgvModules);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ModuleMaintenanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Module Maintenance";
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvModules;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtModuleName;
        private System.Windows.Forms.Label lblModuleName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtContentLink;
        private System.Windows.Forms.Label lblContentLink;
        private System.Windows.Forms.TextBox txtDurationHours;
        private System.Windows.Forms.Label lblDurationHours;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtModuleID;
        private System.Windows.Forms.Label lblModuleID;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnClose;
    }
    // END OF ModuleMaintenanceForm.Designer.cs
}