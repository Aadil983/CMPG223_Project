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
    public partial class CommunityPartnerMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public CommunityPartnerMaintenanceForm()
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            this.Text = "Community Partner Maintenance";
            SetupDataGridView();
            LoadPartners();
        }

        private void SetupDataGridView()
        {
            dgvPartners.AutoGenerateColumns = false;
            dgvPartners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartners.ReadOnly = true;
            dgvPartners.AllowUserToAddRows = false;

            dgvPartners.Columns.Clear();
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerID", HeaderText = "ID", Name = "PartnerID", Width = 50 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner Name", Name = "PartnerName", Width = 150 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContactPerson", HeaderText = "Contact Person", Name = "ContactPerson", Width = 150 });
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContactEmail", HeaderText = "Email", Name = "ContactEmail", Width = 180 });
            var phoneColumn = new DataGridViewTextBoxColumn { DataPropertyName = "ContactPhone", HeaderText = "Phone", Name = "ContactPhone" };
            phoneColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPartners.Columns.Add(phoneColumn);
            dgvPartners.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DateJoined", HeaderText = "Date Joined", Name = "DateJoined", Width = 100, Visible = false });

            dgvPartners.SelectionChanged += dgvPartners_SelectionChanged;
        }

        private void LoadPartners()
        {
            List<CommunityPartner> partners = _dbManager.GetAllPartners();
            dgvPartners.DataSource = partners;

            ClearInputFields();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ClearInputFields()
        {
            txtPartnerID.Text = string.Empty;
            txtPartnerName.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            txtContactEmail.Text = string.Empty;
            txtContactPhone.Text = string.Empty;
            btnAdd.Text = "Add Partner";
            txtPartnerName.Focus();
        }

        private void dgvPartners_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPartners.SelectedRows.Count > 0)
            {
                CommunityPartner selectedPartner = dgvPartners.SelectedRows[0].DataBoundItem as CommunityPartner;
                if (selectedPartner != null)
                {
                    txtPartnerID.Text = selectedPartner.PartnerID.ToString();
                    txtPartnerName.Text = selectedPartner.PartnerName;
                    txtContactPerson.Text = selectedPartner.ContactPerson;
                    txtContactEmail.Text = selectedPartner.ContactEmail;
                    txtContactPhone.Text = selectedPartner.ContactPhone;

                    btnAdd.Text = "Clear";
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

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtPartnerName.Text) ||
                string.IsNullOrWhiteSpace(txtContactPerson.Text) ||
                string.IsNullOrWhiteSpace(txtContactEmail.Text) ||
                string.IsNullOrWhiteSpace(txtContactPhone.Text))
            {
                MessageBox.Show("Please fill in all partner details.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Partner";
                return;
            }

            if (!ValidateInput()) return;

            string name = txtPartnerName.Text;
            string person = txtContactPerson.Text;
            string email = txtContactEmail.Text;
            string phone = txtContactPhone.Text;

            int newId = _dbManager.AddPartner(name, person, email, phone);

            if (newId > 0)
            {
                MessageBox.Show($"Partner '{name}' added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPartners();
            }
            else
            {
                MessageBox.Show("Failed to add partner. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (!int.TryParse(txtPartnerID.Text, out int partnerId))
            {
                MessageBox.Show("Partner ID is invalid.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtPartnerName.Text;
            string person = txtContactPerson.Text;
            string email = txtContactEmail.Text;
            string phone = txtContactPhone.Text;

            bool success = _dbManager.UpdatePartner(partnerId, name, person, email, phone);

            if (success)
            {
                MessageBox.Show($"Partner ID {partnerId} updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPartners();
            }
            else
            {
                MessageBox.Show("Failed to update partner. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvPartners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a partner to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CommunityPartner selectedPartner = dgvPartners.SelectedRows[0].DataBoundItem as CommunityPartner;
            if (selectedPartner == null) return;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Partner ID {selectedPartner.PartnerID}: {selectedPartner.PartnerName}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeletePartner(selectedPartner.PartnerID);

                if (success)
                {
                    MessageBox.Show("Partner deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPartners();
                }
                else
                {
                    MessageBox.Show("Failed to delete partner. It may be referenced by other records (Foreign Key Constraint).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
