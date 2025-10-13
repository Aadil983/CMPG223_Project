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
    public partial class AssessmentMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public AssessmentMaintenanceForm()
        {
            InitializeComponent();

            // Initialize DBManager instance, assuming it takes ConnectionString
            _dbManager = new DBManager(Constants.ConnectionString);

            LoadComboBoxes();
            SetupDataGridView();
            LoadAssessments();
        }

        /// <summary>
        /// Loads data into the Module and Partner ComboBoxes.
        /// </summary>
        private void LoadComboBoxes()
        {
            if (_dbManager == null) return;
            try
            {
                // NOTE: Requires DBManager.GetModules() and DBManager.GetPartners() to exist.

                // 1. Load Modules
                // Assuming GetModules returns List<ModuleModel> with {ModuleID, ModuleName}
                var modules = _dbManager.Modules;
                cmbModule.DataSource = modules;
                cmbModule.DisplayMember = "ModuleName";
                cmbModule.ValueMember = "ModuleID";
                cmbModule.SelectedIndex = -1;

                // 2. Load Partners
                // Assuming GetPartners returns List<PartnerModel> with {PartnerID, PartnerName}
                var partners = _dbManager.GetPartners();
                cmbPartner.DataSource = partners;
                cmbPartner.DisplayMember = "PartnerName";
                cmbPartner.ValueMember = "PartnerID";
                cmbPartner.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load supporting data (Modules/Partners): {ex.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configures the DataGridView columns to match the AssessmentMaintenanceModel.
        /// Uses ModuleName as the primary descriptor.
        /// </summary>
        private void SetupDataGridView()
        {
            dgvAssessments.AutoGenerateColumns = false;
            dgvAssessments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssessments.ReadOnly = true;
            dgvAssessments.AllowUserToAddRows = false;
            dgvAssessments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvAssessments.Columns.Clear();

            // Display Columns
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentID", HeaderText = "ID", Width = 50 });
            // ModuleName is used as the descriptive identifier for the assessment
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module/Assessment", Width = 200 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner", Width = 150 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PassPercentage",
                HeaderText = "Pass %",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "P0" } // Format as percentage
            });

            // Hidden Foreign Key Columns (Needed for edit/update operations)
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleID", Visible = false });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerID", Visible = false });
        }

        /// <summary>
        /// Calls the DBManager to fetch all assessment data and binds it to the grid.
        /// </summary>
        private void LoadAssessments()
        {
            try
            {
                // The GetAllAssessmentsForAdmin method is located in the DBManager class in Program.cs
                List<AssessmentMaintenanceModel> assessments = _dbManager.GetAllAssessmentsForAdmin();
                dgvAssessments.DataSource = assessments;

                if (assessments.Count > 0)
                {
                    dgvAssessments.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load assessments: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAssessments();
        }

        private void ClearInputFields()
        {
            txtAssessmentID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPassPercentage.Text = string.Empty;
            cmbModule.SelectedIndex = -1;
            cmbPartner.SelectedIndex = -1;
            btnAdd.Text = "Add Assessment";
            txtName.Focus();
        }

        private void dgvAssessments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count > 0)
            {
                Assessment selectedAssessment = dgvAssessments.SelectedRows[0].DataBoundItem as Assessment;
                if (selectedAssessment != null)
                {
                    txtAssessmentID.Text = selectedAssessment.AssessmentID.ToString();
                    txtName.Text = selectedAssessment.AssessmentName;
                    txtPassPercentage.Text = selectedAssessment.PassPercentage.ToString();

                    // Select correct Module in ComboBox
                    cmbModule.SelectedValue = selectedAssessment.ModuleID;

                    // Select correct Partner in ComboBox
                    cmbPartner.SelectedValue = selectedAssessment.PartnerID;

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
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPassPercentage.Text) ||
                cmbModule.SelectedIndex == -1 ||
                cmbPartner.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in the assessment name, select a Module, select a Partner, and enter the Pass Percentage.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPassPercentage.Text, out decimal percentage) || percentage < 0 || percentage > 100)
            {
                MessageBox.Show("Pass Percentage must be a number between 0 and 100.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassPercentage.Focus();
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

            string name = txtName.Text;
            int moduleId = (int)cmbModule.SelectedValue;
            int partnerId = (int)cmbPartner.SelectedValue;
            decimal passPercentage = decimal.Parse(txtPassPercentage.Text);

            int newId = _dbManager.AddAssessment(name, moduleId, partnerId, passPercentage);

            if (newId > 0)
            {
                MessageBox.Show($"Assessment '{name}' added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAssessments();
            }
            else
            {
                MessageBox.Show("Failed to add assessment. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAssessmentID.Text))
            {
                MessageBox.Show("Please select an assessment to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int assessmentId = int.Parse(txtAssessmentID.Text);
            string name = txtName.Text;
            int moduleId = (int)cmbModule.SelectedValue;
            int partnerId = (int)cmbPartner.SelectedValue;
            decimal passPercentage = decimal.Parse(txtPassPercentage.Text);

            bool success = _dbManager.UpdateAssessment(assessmentId, name, moduleId, partnerId, passPercentage);

            if (success)
            {
                MessageBox.Show($"Assessment ID {assessmentId} updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAssessments();
            }
            else
            {
                MessageBox.Show("Failed to update assessment. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtAssessmentID.Text))
            {
                MessageBox.Show("Please select an assessment to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int assessmentId = int.Parse(txtAssessmentID.Text);
            string name = txtName.Text;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Assessment ID {assessmentId}: {name}? This will affect all associated questions and results.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeleteAssessment(assessmentId);

                if (success)
                {
                    MessageBox.Show("Assessment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAssessments();
                }
                else
                {
                    MessageBox.Show("Failed to delete assessment. It may be referenced by other records (e.g., questions or results).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvAssessments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Prevents trying to access SelectedRows while the DataGridView is updating
            if (dgvAssessments.CurrentRow == null || dgvAssessments.CurrentRow.DataBoundItem == null)
            {
                ClearInputFields();
                return;
            }

            // FIX 1: Cast the DataBoundItem to the correct model type: AssessmentMaintenanceModel
            AssessmentMaintenanceModel selectedAssessment = dgvAssessments.CurrentRow.DataBoundItem as AssessmentMaintenanceModel;

            if (selectedAssessment != null)
            {
                txtAssessmentID.Text = selectedAssessment.AssessmentID.ToString();

                // FIX 2: Use ModuleName since AssessmentName doesn't exist in the model/DB
                txtName.Text = selectedAssessment.ModuleName;

                // The PassPercentage in your model is decimal, format it if necessary, or just display
                txtPassPercentage.Text = (selectedAssessment.PassPercentage * 100).ToString("F2");

                // FIX 3: Selecting values in ComboBoxes relies on ValueMember being set (done in LoadComboBoxes)
                cmbModule.SelectedValue = selectedAssessment.ModuleID;
                cmbPartner.SelectedValue = selectedAssessment.PartnerID;

                btnAdd.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                ClearInputFields();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
    }
}