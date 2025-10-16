using DLPM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Removed: using Microsoft.VisualBasic; // No longer needed as password reset is removed

namespace WindowsFormsApp1
{
    // NOTE: This file assumes the controls are named:
    // dgvParticipants, txtStakeholderID, txtFirstName, txtLastName, txtEmail, 
    // cmbRoleType (ComboBox), btnAdd, btnUpdate, btnDelete,
    // txtPartnerName (Partner only field), txtContactNumber (Partner only field)
    public partial class StakeholderMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;
        private readonly List<string> roleTypes = new List<string> { "Admin", "Participant", "Partner" };

        public StakeholderMaintenanceForm()
        {
            InitializeComponent();

            try
            {
                _dbManager = new DBManager(Constants.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization Error: {ex.Message}", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _dbManager = null;
                return;
            }

            SetupControls();
            LoadStakeholders();
        }

        /// <summary>
        /// Sets up the RoleType ComboBox and DataGridView.
        /// </summary>
        private void SetupControls()
        {
            cmbRoleType.DataSource = roleTypes;
            cmbRoleType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoleType.SelectedIndexChanged += cmbRoleType_SelectedIndexChanged; // Attach handler

            SetupDataGridView();
        }

        /// <summary>
        /// Configures the DataGridView columns to match the Stakeholder model.
        /// </summary>
        private void SetupDataGridView()
        {
            // The DGV name is dgvParticipants
            dgvParticipants.AutoGenerateColumns = false;
            dgvParticipants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvParticipants.ReadOnly = true;
            dgvParticipants.AllowUserToAddRows = false;
            dgvParticipants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvParticipants.Columns.Clear();

            // Updated Columns to match the full Stakeholder model
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StakeholderID", HeaderText = "ID", Width = 50 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Last Name", Width = 100 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "First Name", Width = 100 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email Address", Width = 150 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RoleType", HeaderText = "Role", Width = 80 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner Name", Width = 150 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContactNumber", HeaderText = "Contact", Width = 100 });

            // FIX: Use the correct event handler for row selection
            dgvParticipants.SelectionChanged += dgvParticipants_SelectionChanged;
        }

        private void LoadStakeholders()
        {
            if (_dbManager == null) return;
            try
            {
                List<Stakeholder> users = _dbManager.GetAllStakeholders();
                dgvParticipants.DataSource = users;

                if (users.Count > 0)
                {
                    // Select the first row to automatically populate the fields
                    dgvParticipants.Rows[0].Selected = true;
                }
                else
                {
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load stakeholders: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Assuming a btnRefresh_Click method exists in the designer
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStakeholders();
        }

        private void ClearInputFields()
        {
            txtStakeholderID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContactNumber.Text = string.Empty;

            // New fields
            cmbRoleType.SelectedIndex = 0; // Default to Admin or first role
            txtPartnerName.Text = string.Empty;

            TogglePartnerFields(cmbRoleType.SelectedItem?.ToString());

            btnAdd.Text = "Add Stakeholder";
            txtFirstName.Focus();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            dgvParticipants.ClearSelection();
        }

        /// <summary>
        /// Enables/Disables Partner-specific fields based on the selected RoleType.
        /// </summary>
        private void TogglePartnerFields(string role)
        {
            bool isPartner = role == "Partner";
            txtPartnerName.Enabled = isPartner;
            txtContactNumber.Enabled = isPartner;

            if (!isPartner)
            {
                txtPartnerName.Text = string.Empty;
            }
        }

        private void cmbRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRoleType.SelectedItem is string selectedRole)
            {
                TogglePartnerFields(selectedRole);
            }
        }

        /// <summary>
        /// Handles populating the input fields when a new row is selected.
        /// </summary>
        private void dgvParticipants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvParticipants.CurrentRow == null || dgvParticipants.CurrentRow.DataBoundItem == null || dgvParticipants.SelectedRows.Count == 0)
            {
                ClearInputFields();
                return;
            }

            // Use SelectedRows[0] which is more reliable when SelectionMode is FullRowSelect
            Stakeholder selectedUser = dgvParticipants.SelectedRows[0].DataBoundItem as Stakeholder;

            if (selectedUser != null)
            {
                txtStakeholderID.Text = selectedUser.StakeholderID.ToString();
                txtFirstName.Text = selectedUser.FirstName;
                txtLastName.Text = selectedUser.LastName;
                txtEmail.Text = selectedUser.Email;

                // Set role and trigger partner fields toggle
                cmbRoleType.SelectedItem = selectedUser.RoleType;

                txtPartnerName.Text = selectedUser.PartnerName;
                txtContactNumber.Text = selectedUser.ContactNumber; // Retain existing contact number if used

                // Toggling based on the selected user's role
                TogglePartnerFields(selectedUser.RoleType);

                btnAdd.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                ClearInputFields();
            }
        }


#pragma warning disable S1172 // Unused method parameters should be removed
        private bool ValidateInput(bool isAdd = false)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || cmbRoleType.SelectedItem == null)
            {
                MessageBox.Show("First Name, Last Name, Email, and Role Type are required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Removed: Password check for Add is no longer needed

            // Partner field check
            if (cmbRoleType.SelectedItem.ToString() == "Partner" && string.IsNullOrWhiteSpace(txtPartnerName.Text))
            {
                MessageBox.Show("Partner Name is required when Role Type is 'Partner'.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Handle the Clear button toggle
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Stakeholder";
                return;
            }

            try
            {
                // Gather input
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string roleType = cmbRoleType.SelectedItem?.ToString();
                string partnerName = txtPartnerName.Text.Trim();
                string contactNumber = txtContactNumber.Text.Trim();
                string idNumber = txtIDNumber.Text.Trim();

                // Basic validation
                if (string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(roleType))
                {
                    MessageBox.Show("Please fill in all required fields (First Name, Last Name, Email, Role Type).",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Call database method
                int result = _dbManager.AddStakeholder(firstName, lastName, email, roleType, partnerName, contactNumber, idNumber);

                // Interpret the result
                if (result > 0)
                {
                    MessageBox.Show("Stakeholder added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStakeholders(); // refresh table if applicable
                    ClearInputFields();
                }
                else if (result == -2)
                {
                    MessageBox.Show("A stakeholder with this email already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Failed to add stakeholder. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while adding stakeholder:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // This handler handles the click event of the Add button if it was named btnAdd_Click_1 in the designer
        // Since both btnAdd_Click and btnAdd_Click_1 existed in the user's snippet, I'm cleaning up 
        // by making btnAdd_Click the main one and removing btnAdd_Click_1.
        // If the designer requires btnAdd_Click_1, please revert the method name.

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStakeholderID.Text))
            {
                MessageBox.Show("Please select a stakeholder to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int stakeholderId = int.Parse(txtStakeholderID.Text);
            string role = cmbRoleType.SelectedItem.ToString();
            string partnerName = (role == "Partner") ? txtPartnerName.Text.Trim() : null;
            string contact = (role == "Partner") ? txtContactNumber.Text.Trim() : null;

            // Calls the generalized update method
            bool success = _dbManager.UpdateStakeholder(
                stakeholderId,
                txtFirstName.Text.Trim(),
                txtLastName.Text.Trim(),
                txtEmail.Text.Trim(),
                role,
                partnerName,
                contact);

            if (success)
            {
                MessageBox.Show($"Stakeholder ID {stakeholderId} updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStakeholders();
            }
            else
            {
                MessageBox.Show("Failed to update stakeholder. This could be due to a duplicate email address or a database error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Removed: btnResetPassword_Click

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvParticipants.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtStakeholderID.Text))
            {
                MessageBox.Show("Please select a stakeholder to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int stakeholderId = int.Parse(txtStakeholderID.Text);
            string email = txtEmail.Text;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Stakeholder ID {stakeholderId} ({email})? This action is irreversible.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeleteStakeholder(stakeholderId);

                if (success)
                {
                    MessageBox.Show("Stakeholder deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStakeholders();
                    ClearInputFields();
                }
                else
                {
                    // Foreign key constraint (547) is caught in DBManager and returns false.
                    MessageBox.Show("Failed to delete stakeholder. This user may be referenced by other records (e.g., enrollments, assessments, or modules).", "Deletion Error: Foreign Key Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Removed: dgvParticipants_CellContentClick (was the incorrect event handler)
    }
}
