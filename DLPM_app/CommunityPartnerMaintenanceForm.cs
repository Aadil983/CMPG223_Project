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
using static DLPM.DBManager;

namespace WindowsFormsApp1
{
    // NOTE: This file assumes the controls are named:
    // dgvPartners, txtPartnerID, txtPartnerName, txtEmail, txtContactNumber, btnAdd, btnUpdate, btnDelete, btnClear
    public partial class CommunityPartnerMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public CommunityPartnerMaintenanceForm()
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

            SetupDataGridView();
            LoadPartners();
        }

        /// <summary>
        /// Configures the DataGridView columns to match the PartnerMaintenanceModel.
        /// </summary>
        private void SetupDataGridView()
        {
            dgvPartners.AutoGenerateColumns = false;
            dgvPartners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartners.ReadOnly = true;
            dgvPartners.AllowUserToAddRows = false;
            dgvPartners.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvPartners.Columns.Clear();

            // Display Columns
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerID", HeaderText = "ID", Width = 50 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner Name", Width = 200 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EmailAddress", HeaderText = "Email Address", Width = 150 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContactNumber", HeaderText = "Contact Number", Width = 100 });
        }

        /// <summary>
        /// Calls the DBManager to fetch all partner data and binds it to the grid.
        /// </summary>
        private void LoadPartners()
        {
            if (_dbManager == null) return;
            try
            {
                List<PartnerMaintenanceModel> partners = _dbManager.GetAllPartnersForMaintenance();
                dgvPartners.DataSource = partners;

                // Select the first row if data exists
                if (partners.Count > 0)
                {
                    dgvPartners.Rows[0].Selected = true;
                }
                else
                {
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load partners: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Assuming a btnRefresh_Click method exists in the designer
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPartners();
        }


        /// <summary>
        /// Clears all input fields and resets buttons for a new entry.
        /// </summary>
        private void ClearInputFields()
        {
            txtPartnerID.Text = string.Empty;
            txtPartnerName.Text = string.Empty;
            txtContactEmail.Text = string.Empty;
            txtContactPhone.Text = string.Empty;

            btnAdd.Text = "Add Partner";
            txtPartnerName.Focus();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            dgvPartners.ClearSelection();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtPartnerName.Text) || string.IsNullOrWhiteSpace(txtContactEmail.Text))
            {
                MessageBox.Show("Partner Name and Email Address are required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Basic email format check (optional but recommended)
            if (!txtContactEmail.Text.Contains("@") || !txtContactEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid Email Address.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Assuming a btnClear control exists in the designer
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Partner";
                return;
            }

            if (!ValidateInput()) return;

            string name = txtPartnerName.Text.Trim();
            string email = txtContactEmail.Text.Trim();
            string contact = txtContactPhone.Text.Trim();

            int newId = _dbManager.AddPartner(name, email, contact);

            if (newId > 0)
            {
                MessageBox.Show($"Partner '{name}' added successfully! ID: {newId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPartners();
            }
            else if (newId == -2)
            {
                MessageBox.Show("Failed to add partner: The email address is already in use by another Stakeholder.", "Error: Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Failed to add partner. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPartners_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Prevents errors during unbinding or clearing
            if (dgvPartners.CurrentRow == null || dgvPartners.CurrentRow.DataBoundItem == null)
            {
                ClearInputFields();
                return;
            }

            PartnerMaintenanceModel selectedPartner = dgvPartners.CurrentRow.DataBoundItem as PartnerMaintenanceModel;

            if (selectedPartner != null)
            {
                txtPartnerID.Text = selectedPartner.PartnerID.ToString();
                txtPartnerName.Text = selectedPartner.PartnerName;
                txtContactEmail.Text = selectedPartner.EmailAddress;
                txtContactPhone.Text = selectedPartner.ContactNumber;

                btnAdd.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                ClearInputFields();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvPartners.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtPartnerID.Text))
            {
                MessageBox.Show("Please select a partner to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int partnerId = int.Parse(txtPartnerID.Text);
            string name = txtPartnerName.Text;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Partner ID {partnerId}: {name}? This may prevent access to associated modules.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeletePartner(partnerId);

                if (success)
                {
                    MessageBox.Show("Partner deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPartners();
                }
                else
                {
                    // Foreign key constraint (547) is caught in DBManager and returns false.
                    MessageBox.Show("Failed to delete partner. This partner is still linked to one or more Modules via the ModulePartner table.", "Deletion Error: Foreign Key Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartnerID.Text))
            {
                MessageBox.Show("Please select a partner to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int partnerId = int.Parse(txtPartnerID.Text);
            string name = txtPartnerName.Text.Trim();
            string email = txtContactEmail.Text.Trim();
            string contact = txtContactPhone.Text.Trim();

            bool success = _dbManager.UpdatePartner(partnerId, name, email, contact);

            if (success)
            {
                MessageBox.Show($"Partner ID {partnerId} updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPartners();
            }
            else
            {
                // UpdatePartner returns false for duplicate email or generic failure
                MessageBox.Show("Failed to update partner. This could be due to a duplicate email address or a database error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}