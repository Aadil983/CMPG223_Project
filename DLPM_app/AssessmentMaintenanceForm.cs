using DLPM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DLPM.DBManager;

namespace WindowsFormsApp1
{
    public partial class AssessmentMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public AssessmentMaintenanceForm()
        {
            InitializeComponent();

            _dbManager = new DBManager(Constants.ConnectionString);

            LoadComboBoxes();
            SetupDataGridView();
            LoadAssessments();
        }

        private void LoadComboBoxes()
        {
            if (_dbManager == null) return;
            try
            {
                var modules = _dbManager.Modules;
                cmbModule.DataSource = modules;
                cmbModule.DisplayMember = "ModuleName";
                cmbModule.ValueMember = "ModuleID";
                cmbModule.SelectedIndex = -1;

                var partners = _dbManager.GetPartners();
                cmbPartner.DataSource = partners;
                cmbPartner.DisplayMember = "PartnerName";
                cmbPartner.ValueMember = "PartnerID";
                cmbPartner.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load supporting data: {ex.Message}",
                    "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dgvAssessments.AutoGenerateColumns = false;
            dgvAssessments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssessments.ReadOnly = true;
            dgvAssessments.AllowUserToAddRows = false;
            dgvAssessments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAssessments.Columns.Clear();

            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AssessmentID",
                HeaderText = "ID",
                Width = 50
            });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AssessmentName",
                HeaderText = "Assessment Name",
                Width = 180
            });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ModuleName",
                HeaderText = "Module",
                Width = 180
            });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PartnerName",
                HeaderText = "Partner",
                Width = 150
            });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PassPercentage",
                HeaderText = "Pass %",
                Width = 80
            });
        }

        private void LoadAssessments()
        {
            try
            {
                var assessments = _dbManager.GetAllAssessmentsForAdmin();
                dgvAssessments.DataSource = assessments;

                if (assessments.Count > 0)
                    dgvAssessments.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load assessments: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            txtAssessmentID.Clear();
            txtName.Clear();
            txtPassPercentage.Clear();
            cmbModule.SelectedIndex = -1;
            cmbPartner.SelectedIndex = -1;
            btnAdd.Text = "Add Assessment";
            txtName.Focus();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter an assessment name.", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbModule.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a module.", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPassPercentage.Text, out decimal percentage) ||
                percentage < 0 || percentage > 100)
            {
                MessageBox.Show("Pass Percentage must be between 0 and 100.",
                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Assessment";
                return;
            }

            if (!ValidateInput()) return;

            string name = txtName.Text.Trim();
            int moduleId = (int)cmbModule.SelectedValue;
            int partnerId = (int)cmbPartner.SelectedValue;
            decimal passPercentage = decimal.Parse(txtPassPercentage.Text);

            try
            {
                int newId = _dbManager.AddStandaloneAssessment(name, moduleId, partnerId, passPercentage);

                if (newId > 0)
                {
                    MessageBox.Show($"Assessment '{name}' added successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAssessments();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Failed to add assessment.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding assessment: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an assessment to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(txtAssessmentID.Text);
            string name = txtName.Text;

            if (MessageBox.Show($"Delete assessment '{name}'?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool success = _dbManager.DeleteStandaloneAssessment(id);

                    if (success)
                    {
                        MessageBox.Show("Assessment deleted successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAssessments();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete assessment.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting assessment: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvAssessments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvAssessments.Rows.Count == 0) return;

            var selected = dgvAssessments.Rows[e.RowIndex].DataBoundItem as AssessmentMaintenanceModel;
            if (selected == null) return;

            txtAssessmentID.Text = selected.AssessmentID.ToString();
            txtName.Text = selected.AssessmentName;
            txtPassPercentage.Text = selected.PassPercentage.ToString("F2");

            cmbModule.SelectedValue = selected.ModuleID;
            cmbPartner.SelectedValue = selected.PartnerID;

            btnAdd.Text = "Clear";
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAssessmentID.Text))
            {
                MessageBox.Show("Select an assessment to update.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int id = int.Parse(txtAssessmentID.Text);
            string name = txtName.Text.Trim();
            int moduleId = (int)cmbModule.SelectedValue;
            int partnerId = (int)cmbPartner.SelectedValue;
            decimal passPercentage = decimal.Parse(txtPassPercentage.Text);

            try
            {
                bool success = _dbManager.UpdateStandaloneAssessment(id, name, moduleId, partnerId, passPercentage);

                if (success)
                {
                    MessageBox.Show("Assessment updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAssessments();
                }
                else
                {
                    MessageBox.Show("Failed to update assessment.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating assessment: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
