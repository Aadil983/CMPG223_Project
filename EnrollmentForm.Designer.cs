namespace WindowsFormsApp1
{
    partial class EnrollmentForm
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
            this.dgvModules = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpParticipant = new System.Windows.Forms.GroupBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.grpAdmin = new System.Windows.Forms.GroupBox();
            this.btnDeny = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).BeginInit();
            this.grpParticipant.SuspendLayout();
            this.grpAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(211, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Module Enrollment";
            // 
            // dgvModules
            // 
            this.dgvModules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModules.Location = new System.Drawing.Point(17, 65);
            this.dgvModules.Name = "dgvModules";
            this.dgvModules.Size = new System.Drawing.Size(645, 300);
            this.dgvModules.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(587, 452);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpParticipant
            // 
            this.grpParticipant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpParticipant.Controls.Add(this.btnRequest);
            this.grpParticipant.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpParticipant.Location = new System.Drawing.Point(17, 371);
            this.grpParticipant.Name = "grpParticipant";
            this.grpParticipant.Size = new System.Drawing.Size(645, 68);
            this.grpParticipant.TabIndex = 3;
            this.grpParticipant.TabStop = false;
            this.grpParticipant.Text = "Participant Actions";
            // 
            // btnRequest
            // 
            this.btnRequest.BackColor = System.Drawing.Color.MediumAquamarine;
            this.btnRequest.FlatAppearance.BorderSize = 0;
            this.btnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequest.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRequest.ForeColor = System.Drawing.Color.White;
            this.btnRequest.Location = new System.Drawing.Point(6, 25);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(180, 32);
            this.btnRequest.TabIndex = 0;
            this.btnRequest.Text = "Request Enrollment";
            this.btnRequest.UseVisualStyleBackColor = false;
            // 
            // grpAdmin
            // 
            this.grpAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAdmin.Controls.Add(this.btnDeny);
            this.grpAdmin.Controls.Add(this.btnApprove);
            this.grpAdmin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAdmin.Location = new System.Drawing.Point(17, 371);
            this.grpAdmin.Name = "grpAdmin";
            this.grpAdmin.Size = new System.Drawing.Size(645, 68);
            this.grpAdmin.TabIndex = 4;
            this.grpAdmin.TabStop = false;
            this.grpAdmin.Text = "Admin Approval Actions";
            this.grpAdmin.Visible = false;
            // 
            // btnDeny
            // 
            this.btnDeny.BackColor = System.Drawing.Color.IndianRed;
            this.btnDeny.FlatAppearance.BorderSize = 0;
            this.btnDeny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeny.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeny.ForeColor = System.Drawing.Color.White;
            this.btnDeny.Location = new System.Drawing.Point(192, 25);
            this.btnDeny.Name = "btnDeny";
            this.btnDeny.Size = new System.Drawing.Size(180, 32);
            this.btnDeny.TabIndex = 1;
            this.btnDeny.Text = "Deny Request";
            this.btnDeny.UseVisualStyleBackColor = false;
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.SeaGreen;
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(6, 25);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(180, 32);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve Request";
            this.btnApprove.UseVisualStyleBackColor = false;
            // 
            // EnrollmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 494);
            this.Controls.Add(this.grpAdmin);
            this.Controls.Add(this.grpParticipant);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvModules);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(400, 450);
            this.Name = "EnrollmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Enrollment";
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).EndInit();
            this.grpParticipant.ResumeLayout(false);
            this.grpAdmin.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.DataGridView dgvModules;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.GroupBox grpParticipant;
        public System.Windows.Forms.Button btnRequest;
        public System.Windows.Forms.GroupBox grpAdmin;
        public System.Windows.Forms.Button btnDeny;
        public System.Windows.Forms.Button btnApprove;
    }
}
