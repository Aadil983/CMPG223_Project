namespace WindowsFormsApp1
{
    partial class StakeholderMaintenanceForm
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
            this.dgvParticipants = new System.Windows.Forms.DataGridView();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.txtContactNumber = new System.Windows.Forms.TextBox();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.txtIDNumber = new System.Windows.Forms.TextBox();
            this.lblIDNumber = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtStakeholderID = new System.Windows.Forms.TextBox();
            this.lblStakeholderID = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.cmbRoleType = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(268, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Participant Maintenance";
            // 
            // dgvParticipants
            // 
            this.dgvParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticipants.Location = new System.Drawing.Point(15, 46);
            this.dgvParticipants.Margin = new System.Windows.Forms.Padding(2);
            this.dgvParticipants.Name = "dgvParticipants";
            this.dgvParticipants.RowHeadersWidth = 51;
            this.dgvParticipants.RowTemplate.Height = 29;
            this.dgvParticipants.Size = new System.Drawing.Size(675, 195);
            this.dgvParticipants.TabIndex = 1;

            // 
            // pnlInput
            // 
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.lblPartnerName);
            this.pnlInput.Controls.Add(this.txtPartnerName);
            this.pnlInput.Controls.Add(this.lblRole);
            this.pnlInput.Controls.Add(this.cmbRoleType);
            this.pnlInput.Controls.Add(this.txtContactNumber);
            this.pnlInput.Controls.Add(this.lblContactNumber);
            this.pnlInput.Controls.Add(this.txtIDNumber);
            this.pnlInput.Controls.Add(this.lblIDNumber);
            this.pnlInput.Controls.Add(this.txtEmail);
            this.pnlInput.Controls.Add(this.lblEmail);
            this.pnlInput.Controls.Add(this.btnClose);
            this.pnlInput.Controls.Add(this.txtStakeholderID);
            this.pnlInput.Controls.Add(this.lblStakeholderID);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.txtLastName);
            this.pnlInput.Controls.Add(this.lblLastName);
            this.pnlInput.Controls.Add(this.txtFirstName);
            this.pnlInput.Controls.Add(this.lblFirstName);
            this.pnlInput.Location = new System.Drawing.Point(15, 253);
            this.pnlInput.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(676, 170);
            this.pnlInput.TabIndex = 2;
            // 
            // txtContactNumber
            // 
            this.txtContactNumber.Location = new System.Drawing.Point(435, 65);
            this.txtContactNumber.Margin = new System.Windows.Forms.Padding(2);
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(226, 20);
            this.txtContactNumber.TabIndex = 14;
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.AutoSize = true;
            this.lblContactNumber.Location = new System.Drawing.Point(435, 50);
            this.lblContactNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(87, 13);
            this.lblContactNumber.TabIndex = 13;
            this.lblContactNumber.Text = "Contact Number:";
            // 
            // txtIDNumber
            // 
            this.txtIDNumber.Location = new System.Drawing.Point(435, 26);
            this.txtIDNumber.Margin = new System.Windows.Forms.Padding(2);
            this.txtIDNumber.Name = "txtIDNumber";
            this.txtIDNumber.Size = new System.Drawing.Size(226, 20);
            this.txtIDNumber.TabIndex = 12;
            // 
            // lblIDNumber
            // 
            this.lblIDNumber.AutoSize = true;
            this.lblIDNumber.Location = new System.Drawing.Point(435, 11);
            this.lblIDNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIDNumber.Name = "lblIDNumber";
            this.lblIDNumber.Size = new System.Drawing.Size(61, 13);
            this.lblIDNumber.TabIndex = 11;
            this.lblIDNumber.Text = "ID Number:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(15, 65);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(398, 20);
            this.txtEmail.TabIndex = 10;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(15, 50);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Email:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(579, 143);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtStakeholderID
            // 
            this.txtStakeholderID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtStakeholderID.Location = new System.Drawing.Point(311, 26);
            this.txtStakeholderID.Margin = new System.Windows.Forms.Padding(2);
            this.txtStakeholderID.Name = "txtStakeholderID";
            this.txtStakeholderID.ReadOnly = true;
            this.txtStakeholderID.Size = new System.Drawing.Size(102, 20);
            this.txtStakeholderID.TabIndex = 7;
            // 
            // lblStakeholderID
            // 
            this.lblStakeholderID.AutoSize = true;
            this.lblStakeholderID.Location = new System.Drawing.Point(311, 11);
            this.lblStakeholderID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStakeholderID.Name = "lblStakeholderID";
            this.lblStakeholderID.Size = new System.Drawing.Size(81, 13);
            this.lblStakeholderID.TabIndex = 6;
            this.lblStakeholderID.Text = "Stakeholder ID:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(351, 143);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightSalmon;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.Location = new System.Drawing.Point(181, 143);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(158, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update Participant";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(158, 26);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(2);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(136, 20);
            this.txtLastName.TabIndex = 3;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(158, 11);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(61, 13);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Last Name:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(15, 26);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(2);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(136, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(15, 11);
            this.lblFirstName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name:";
            // 
            // cmbRoleType
            // 
            this.cmbRoleType.FormattingEnabled = true;
            this.cmbRoleType.Location = new System.Drawing.Point(15, 109);
            this.cmbRoleType.Name = "cmbRoleType";
            this.cmbRoleType.Size = new System.Drawing.Size(158, 21);
            this.cmbRoleType.TabIndex = 3;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(15, 93);
            this.lblRole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(32, 13);
            this.lblRole.TabIndex = 15;
            this.lblRole.Text = "Role:";
            // 
            // txtPartnerName
            // 
            this.txtPartnerName.Location = new System.Drawing.Point(203, 109);
            this.txtPartnerName.Margin = new System.Windows.Forms.Padding(2);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.Size = new System.Drawing.Size(136, 20);
            this.txtPartnerName.TabIndex = 16;
            // 
            // lblPartnerName
            // 
            this.lblPartnerName.AutoSize = true;
            this.lblPartnerName.Location = new System.Drawing.Point(200, 93);
            this.lblPartnerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(75, 13);
            this.lblPartnerName.TabIndex = 17;
            this.lblPartnerName.Text = "Partner Name:";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(15, 143);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(158, 22);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "Add Participant";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // StakeholderMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 434);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.dgvParticipants);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "StakeholderMaintenanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Participant Maintenance";
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvParticipants;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtStakeholderID;
        private System.Windows.Forms.Label lblStakeholderID;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtContactNumber;
        private System.Windows.Forms.Label lblContactNumber;
        private System.Windows.Forms.TextBox txtIDNumber;
        private System.Windows.Forms.Label lblIDNumber;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRoleType;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.Button btnAdd;
    }

}