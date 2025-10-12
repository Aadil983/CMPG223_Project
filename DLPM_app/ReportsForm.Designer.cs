namespace WindowsFormsApp1
{
    partial class ReportsForm
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
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.lblReportType = new System.Windows.Forms.Label();
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.cmbPartner = new System.Windows.Forms.ComboBox();
            this.lblPartner = new System.Windows.Forms.Label();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.pnlFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 13);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(197, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "System Reporting";
            // 
            // cmbReportType
            // 
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.AddRange(new object[] {
            "Participant Performance Summary",
            "Assessment Module Summary"});
            this.cmbReportType.Location = new System.Drawing.Point(15, 29);
            this.cmbReportType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(264, 21);
            this.cmbReportType.TabIndex = 1;
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Location = new System.Drawing.Point(15, 14);
            this.lblReportType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(69, 13);
            this.lblReportType.TabIndex = 2;
            this.lblReportType.Text = "Report Type:";
            // 
            // dgvReports
            // 
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Location = new System.Drawing.Point(15, 117);
            this.dgvReports.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.RowHeadersWidth = 51;
            this.dgvReports.RowTemplate.Height = 29;
            this.dgvReports.Size = new System.Drawing.Size(750, 260);
            this.dgvReports.TabIndex = 3;
            // 
            // pnlFilters
            // 
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilters.Controls.Add(this.btnGenerate);
            this.pnlFilters.Controls.Add(this.lblEndDate);
            this.pnlFilters.Controls.Add(this.dtpEndDate);
            this.pnlFilters.Controls.Add(this.lblStartDate);
            this.pnlFilters.Controls.Add(this.dtpStartDate);
            this.pnlFilters.Controls.Add(this.cmbPartner);
            this.pnlFilters.Controls.Add(this.lblPartner);
            this.pnlFilters.Controls.Add(this.cmbModule);
            this.pnlFilters.Controls.Add(this.lblModule);
            this.pnlFilters.Controls.Add(this.cmbReportType);
            this.pnlFilters.Controls.Add(this.lblReportType);
            this.pnlFilters.Location = new System.Drawing.Point(15, 46);
            this.pnlFilters.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(750, 62);
            this.pnlFilters.TabIndex = 4;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(645, 27);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(90, 23);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(529, 14);
            this.lblEndDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(55, 13);
            this.lblEndDate.TabIndex = 10;
            this.lblEndDate.Text = "End Date:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(529, 29);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(98, 20);
            this.dtpEndDate.TabIndex = 9;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(428, 14);
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 13);
            this.lblStartDate.TabIndex = 8;
            this.lblStartDate.Text = "Start Date:";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(428, 29);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(98, 20);
            this.dtpStartDate.TabIndex = 7;
            // 
            // cmbPartner
            // 
            this.cmbPartner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartner.FormattingEnabled = true;
            this.cmbPartner.Location = new System.Drawing.Point(176, 29);
            this.cmbPartner.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbPartner.Name = "cmbPartner";
            this.cmbPartner.Size = new System.Drawing.Size(114, 21);
            this.cmbPartner.TabIndex = 6;
            // 
            // lblPartner
            // 
            this.lblPartner.AutoSize = true;
            this.lblPartner.Location = new System.Drawing.Point(176, 14);
            this.lblPartner.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPartner.Name = "lblPartner";
            this.lblPartner.Size = new System.Drawing.Size(44, 13);
            this.lblPartner.TabIndex = 5;
            this.lblPartner.Text = "Partner:";
            // 
            // cmbModule
            // 
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(300, 29);
            this.cmbModule.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(114, 21);
            this.cmbModule.TabIndex = 4;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(300, 14);
            this.lblModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(45, 13);
            this.lblModule.TabIndex = 3;
            this.lblModule.Text = "Module:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(690, 387);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 416);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.dgvReports);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Reports";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cmbPartner;
        private System.Windows.Forms.Label lblPartner;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClose;
    }

}