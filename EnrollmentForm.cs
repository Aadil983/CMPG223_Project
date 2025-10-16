using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLPM;

namespace WindowsFormsApp1
{
    public partial class EnrollmentForm : Form
    {
        private readonly DBManager _dbManager;
        private readonly Stakeholder _currentUser;

        // Assumed controls:
        // GroupBox grpParticipant (Contains dgvModules and btnRequest)
        // DataGridView dgvModules (Used by both roles)
        // Button btnRequest (Participant only)
        // GroupBox grpAdmin (Contains btnApprove and btnDeny)
        // Button btnApprove (Admin only)
        // Button btnDeny (Admin only)
        // Label lblTitle (Used to set the context title)

        public EnrollmentForm(Stakeholder user)
        {
            InitializeComponent();
            _currentUser = user;
            _dbManager = new DBManager(Constants.ConnectionString);

            SetupFormByRole();
        }

        private void SetupFormByRole()
        {
            this.Text = "Module Enrollment Management";

            if (_currentUser.RoleType == "Admin")
            {
                SetupAdminView();
            }
            else // Assuming "Participant" or any non-Admin role uses the request flow
            {
                SetupParticipantView();
            }

            // Common DGV setup
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvModules.AutoGenerateColumns = false;
            dgvModules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvModules.ReadOnly = true;
            dgvModules.AllowUserToAddRows = false;
            dgvModules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvModules.Columns.Clear();

            if (_currentUser.RoleType == "Admin")
            {
                // Admin View Columns (Pending Requests)
                lblTitle.Text = "Pending Module Enrollment Requests";
                grpParticipant.Visible = false;
                grpAdmin.Visible = true;

                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnrollmentID", HeaderText = "Enrollment ID", Width = 80 });
                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ParticipantName", HeaderText = "Participant" });
                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module" });
                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RequestDate", HeaderText = "Requested On", DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } });
                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status", Width = 80 });

                btnApprove.Click += BtnApprove_Click;
                btnDeny.Click += BtnDeny_Click;
            }
            else // Participant View Columns (Available Modules)
            {
                lblTitle.Text = "Available Modules for Enrollment";
                grpParticipant.Visible = true;
                grpAdmin.Visible = false;

                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleID", HeaderText = "Module ID", Width = 80 });
                dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module Name" });

                btnRequest.Click += BtnRequest_Click;
            }
        }

        private void SetupAdminView()
        {
            // Assuming grpAdmin and its buttons (btnApprove, btnDeny) are configured in the designer
        }

        private void SetupParticipantView()
        {
            // Assuming grpParticipant and its button (btnRequest) are configured in the designer
        }

        private void LoadData()
        {
            try
            {
                if (_currentUser.RoleType == "Admin")
                {
                    List<EnrollmentModel> pendingRequests = _dbManager.GetPendingEnrollments();
                    dgvModules.DataSource = pendingRequests;
                    lblTitle.Text = $"Pending Module Enrollment Requests ({pendingRequests.Count} found)";
                }
                else
                {
                    List<EnrollmentModel> availableModules = _dbManager.GetAvailableModules(_currentUser.StakeholderID);
                    dgvModules.DataSource = availableModules;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollment data: {ex.Message}");
            }
        }

        // =================================================================
        // PARTICIPANT ACTIONS
        // =================================================================

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            if (dgvModules.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a module to enroll in.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnrollmentModel selectedModule = dgvModules.SelectedRows[0].DataBoundItem as EnrollmentModel;

            if (selectedModule != null)
            {
                if (MessageBox.Show($"Request enrollment for '{selectedModule.ModuleName}'?", "Confirm Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                if (_dbManager.RequestEnrollment(_currentUser.StakeholderID, selectedModule.ModuleID))
                {
                    MessageBox.Show("Enrollment request submitted successfully and is awaiting Admin approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Reload to remove the requested module from the list
                }
                else
                {
                    // DBManager.RequestEnrollment already shows a message if already pending/approved
                    // If it fails for other reasons:
                    MessageBox.Show("Failed to submit enrollment request. See console for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =================================================================
        // ADMIN ACTIONS
        // =================================================================

        private EnrollmentModel GetSelectedPendingRequest()
        {
            if (dgvModules.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an enrollment request from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            EnrollmentModel selectedRequest = dgvModules.SelectedRows[0].DataBoundItem as EnrollmentModel;

            if (selectedRequest == null || selectedRequest.Status != "Pending")
            {
                MessageBox.Show("The selected item is not a valid pending request.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return selectedRequest;
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            EnrollmentModel selectedRequest = GetSelectedPendingRequest();
            if (selectedRequest == null) return;

            if (MessageBox.Show($"Approve enrollment for {selectedRequest.ParticipantName} in '{selectedRequest.ModuleName}'?", "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_dbManager.UpdateEnrollmentStatus(selectedRequest.EnrollmentID, "Approved"))
                {
                    MessageBox.Show("Enrollment approved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Reload to remove the approved request
                }
                else
                {
                    MessageBox.Show("Failed to approve enrollment. See console for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeny_Click(object sender, EventArgs e)
        {
            EnrollmentModel selectedRequest = GetSelectedPendingRequest();
            if (selectedRequest == null) return;

            if (MessageBox.Show($"Deny enrollment for {selectedRequest.ParticipantName} in '{selectedRequest.ModuleName}'? This action cannot be undone.", "Confirm Denial", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_dbManager.UpdateEnrollmentStatus(selectedRequest.EnrollmentID, "Denied"))
                {
                    MessageBox.Show("Enrollment denied successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Reload to remove the denied request
                }
                else
                {
                    MessageBox.Show("Failed to deny enrollment. See console for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

