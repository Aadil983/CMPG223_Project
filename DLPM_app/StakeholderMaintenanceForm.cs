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

namespace WindowsFormsApp1
{
    public partial class StakeholderMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public StakeholderMaintenanceForm()
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            SetupDataGridView();
            LoadParticipants();
        }

        private void SetupDataGridView()
        {
            dgvParticipants.AutoGenerateColumns = false;
            dgvParticipants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvParticipants.ReadOnly = true;
            dgvParticipants.AllowUserToAddRows = false;
            dgvParticipants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Define columns
            dgvParticipants.Columns.Clear();
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StakeholderID", HeaderText = "ID", Name = "StakeholderID", Width = 50 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastName", HeaderText = "Last Name", Name = "LastName", Width = 120 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FirstName", HeaderText = "First Name", Name = "FirstName", Width = 120 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Name = "Email", Width = 200 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IDNumber", HeaderText = "ID Number", Name = "IDNumber", Width = 150 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContactNumber", HeaderText = "Phone", Name = "ContactNumber", Width = 120 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RoleType", HeaderText = "Role", Name = "RoleType", Width = 80, Visible = false });

            dgvParticipants.SelectionChanged += dgvParticipants_SelectionChanged;
        }

        private void LoadParticipants()
        {
            List<Stakeholder> participants = _dbManager.GetAllParticipants();
            dgvParticipants.DataSource = participants;

            ClearInputFields();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ClearInputFields()
        {
            txtStakeholderID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtIDNumber.Text = string.Empty;
            txtContactNumber.Text = string.Empty;
            txtFirstName.Focus();
        }

        private void dgvParticipants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvParticipants.SelectedRows.Count > 0)
            {
                Stakeholder selectedParticipant = dgvParticipants.SelectedRows[0].DataBoundItem as Stakeholder;
                if (selectedParticipant != null)
                {
                    txtStakeholderID.Text = selectedParticipant.StakeholderID.ToString();
                    txtFirstName.Text = selectedParticipant.FirstName;
                    txtLastName.Text = selectedParticipant.LastName;
                    txtEmail.Text = selectedParticipant.Email;
                    txtIDNumber.Text = selectedParticipant.IDNumber;
                    txtContactNumber.Text = selectedParticipant.ContactNumber;

                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            else
            {
                ClearInputFields();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private bool ValidateInput(bool isUpdate)
        {
            if (isUpdate && string.IsNullOrWhiteSpace(txtStakeholderID.Text))
            {
                MessageBox.Show("Please select a participant to update or delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtIDNumber.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("All fields (Name, Email, ID Number, Phone) must be filled in.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStakeholderID.Text, out _))
            {
                MessageBox.Show("Invalid Stakeholder ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(true)) return;

            int participantId = int.Parse(txtStakeholderID.Text);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string idNumber = txtIDNumber.Text;
            string contactNumber = txtContactNumber.Text;

            // Note: Password cannot be updated here for security reasons.

            bool success = _dbManager.UpdateStakeholderDetails(participantId, firstName, lastName, email, idNumber, contactNumber);

            if (success)
            {
                MessageBox.Show($"Participant ID {participantId} details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadParticipants(); // Refresh the grid
            }
            else
            {
                MessageBox.Show("Failed to update participant. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvParticipants.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtStakeholderID.Text))
            {
                MessageBox.Show("Please select a participant to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int participantId = int.Parse(txtStakeholderID.Text);
            string name = $"{txtFirstName.Text} {txtLastName.Text}";

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Participant {name} (ID: {participantId})? This action cannot be undone.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeleteStakeholder(participantId);

                if (success)
                {
                    MessageBox.Show("Participant deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadParticipants();
                }
                else
                {
                    MessageBox.Show("Failed to delete participant. They may be linked to other records (e.g., Assessments).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
