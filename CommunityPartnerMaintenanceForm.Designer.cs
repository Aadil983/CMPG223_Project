namespace WindowsFormsApp1
{
    // START OF CommunityPartnerMaintenanceForm.Designer.cs

    partial class CommunityPartnerMaintenanceForm
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
            this.dgvPartners = new System.Windows.Forms.DataGridView();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtPartnerID = new System.Windows.Forms.TextBox();
            this.lblPartnerID = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtContactPhone = new System.Windows.Forms.TextBox();
            this.lblContactPhone = new System.Windows.Forms.Label();
            this.txtContactEmail = new System.Windows.Forms.TextBox();
            this.lblContactEmail = new System.Windows.Forms.Label();
            this.txtContactPerson = new System.Windows.Forms.TextBox();
            this.lblContactPerson = new System.Windows.Forms.Label();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.lblPartnerName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartners)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(357, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Community Partner Maintenance";
            // 
            // dgvPartners
            // 
            this.dgvPartners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartners.Location = new System.Drawing.Point(15, 46);
            this.dgvPartners.Margin = new System.Windows.Forms.Padding(2);
            this.dgvPartners.Name = "dgvPartners";
            this.dgvPartners.RowHeadersWidth = 51;
            this.dgvPartners.RowTemplate.Height = 29;
            this.dgvPartners.Size = new System.Drawing.Size(562, 162);
            this.dgvPartners.TabIndex = 1;
            this.dgvPartners.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPartners_CellContentClick);
            // 
            // pnlInput
            // 
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInput.Controls.Add(this.btnClose);
            this.pnlInput.Controls.Add(this.txtPartnerID);
            this.pnlInput.Controls.Add(this.lblPartnerID);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.txtContactPhone);
            this.pnlInput.Controls.Add(this.lblContactPhone);
            this.pnlInput.Controls.Add(this.txtContactEmail);
            this.pnlInput.Controls.Add(this.lblContactEmail);
            this.pnlInput.Controls.Add(this.txtContactPerson);
            this.pnlInput.Controls.Add(this.lblContactPerson);
            this.pnlInput.Controls.Add(this.txtPartnerName);
            this.pnlInput.Controls.Add(this.lblPartnerName);
            this.pnlInput.Location = new System.Drawing.Point(15, 221);
            this.pnlInput.Margin = new System.Windows.Forms.Padding(2);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(563, 168);
            this.pnlInput.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(465, 120);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 30);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // txtPartnerID
            // 
            this.txtPartnerID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtPartnerID.Location = new System.Drawing.Point(354, 13);
            this.txtPartnerID.Margin = new System.Windows.Forms.Padding(2);
            this.txtPartnerID.Name = "txtPartnerID";
            this.txtPartnerID.ReadOnly = true;
            this.txtPartnerID.Size = new System.Drawing.Size(194, 20);
            this.txtPartnerID.TabIndex = 12;
            // 
            // lblPartnerID
            // 
            this.lblPartnerID.AutoSize = true;
            this.lblPartnerID.Location = new System.Drawing.Point(292, 16);
            this.lblPartnerID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPartnerID.Name = "lblPartnerID";
            this.lblPartnerID.Size = new System.Drawing.Size(58, 13);
            this.lblPartnerID.TabIndex = 11;
            this.lblPartnerID.Text = "Partner ID:";
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
            this.btnDelete.Size = new System.Drawing.Size(82, 30);
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
            this.btnUpdate.Location = new System.Drawing.Point(158, 120);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(82, 30);
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
            this.btnAdd.Location = new System.Drawing.Point(15, 120);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(128, 30);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add Partner";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtContactPhone
            // 
            this.txtContactPhone.Location = new System.Drawing.Point(15, 97);
            this.txtContactPhone.Margin = new System.Windows.Forms.Padding(2);
            this.txtContactPhone.Name = "txtContactPhone";
            this.txtContactPhone.Size = new System.Drawing.Size(264, 20);
            this.txtContactPhone.TabIndex = 7;
            // 
            // lblContactPhone
            // 
            this.lblContactPhone.AutoSize = true;
            this.lblContactPhone.Location = new System.Drawing.Point(15, 82);
            this.lblContactPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContactPhone.Name = "lblContactPhone";
            this.lblContactPhone.Size = new System.Drawing.Size(81, 13);
            this.lblContactPhone.TabIndex = 6;
            this.lblContactPhone.Text = "Contact Phone:";
            // 
            // txtContactEmail
            // 
            this.txtContactEmail.Location = new System.Drawing.Point(292, 60);
            this.txtContactEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtContactEmail.Name = "txtContactEmail";
            this.txtContactEmail.Size = new System.Drawing.Size(256, 20);
            this.txtContactEmail.TabIndex = 5;
            // 
            // lblContactEmail
            // 
            this.lblContactEmail.AutoSize = true;
            this.lblContactEmail.Location = new System.Drawing.Point(292, 46);
            this.lblContactEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContactEmail.Name = "lblContactEmail";
            this.lblContactEmail.Size = new System.Drawing.Size(75, 13);
            this.lblContactEmail.TabIndex = 4;
            this.lblContactEmail.Text = "Contact Email:";
            // 
            // txtContactPerson
            // 
            this.txtContactPerson.Location = new System.Drawing.Point(15, 60);
            this.txtContactPerson.Margin = new System.Windows.Forms.Padding(2);
            this.txtContactPerson.Name = "txtContactPerson";
            this.txtContactPerson.Size = new System.Drawing.Size(264, 20);
            this.txtContactPerson.TabIndex = 3;
            // 
            // lblContactPerson
            // 
            this.lblContactPerson.AutoSize = true;
            this.lblContactPerson.Location = new System.Drawing.Point(15, 46);
            this.lblContactPerson.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContactPerson.Name = "lblContactPerson";
            this.lblContactPerson.Size = new System.Drawing.Size(83, 13);
            this.lblContactPerson.TabIndex = 2;
            this.lblContactPerson.Text = "Contact Person:";
            // 
            // txtPartnerName
            // 
            this.txtPartnerName.Location = new System.Drawing.Point(15, 26);
            this.txtPartnerName.Margin = new System.Windows.Forms.Padding(2);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.Size = new System.Drawing.Size(264, 20);
            this.txtPartnerName.TabIndex = 1;
            // 
            // lblPartnerName
            // 
            this.lblPartnerName.AutoSize = true;
            this.lblPartnerName.Location = new System.Drawing.Point(15, 11);
            this.lblPartnerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(75, 13);
            this.lblPartnerName.TabIndex = 0;
            this.lblPartnerName.Text = "Partner Name:";
            // 
            // CommunityPartnerMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 400);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.dgvPartners);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "CommunityPartnerMaintenanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Community Partner Maintenance";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartners)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvPartners;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.TextBox txtContactPerson;
        private System.Windows.Forms.Label lblContactPerson;
        private System.Windows.Forms.TextBox txtContactEmail;
        private System.Windows.Forms.Label lblContactEmail;
        private System.Windows.Forms.TextBox txtContactPhone;
        private System.Windows.Forms.Label lblContactPhone;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtPartnerID;
        private System.Windows.Forms.Label lblPartnerID;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
    }
    // END OF CommunityPartnerMaintenanceForm.Designer.cs
}