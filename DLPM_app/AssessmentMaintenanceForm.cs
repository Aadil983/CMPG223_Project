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
        private List<ModuleLookup> _modules;
        private List<PartnerLookup> _partners;

        public AssessmentMaintenanceForm()
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            SetupDataGridView();
            LoadLookups();
            LoadAssessments();
        }

        private void SetupDataGridView()
        {
            dgvAssessments.AutoGenerateColumns = false;
            dgvAssessments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssessments.ReadOnly = true;
            dgvAssessments.AllowUserToAddRows = false;
            dgvAssessments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvAssessments.Columns.Clear();
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentID", HeaderText = "ID", Name = "AssessmentID", Width = 50 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentName", HeaderText = "Assessment Name", Name = "AssessmentName", Width = 200 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module", Name = "ModuleName", Width = 150 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner", Name = "PartnerName", Width = 150 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassPercentage", HeaderText = "Pass %", Name = "PassPercentage", Width = 70 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DateCreated", HeaderText = "Date Created", Name = "DateCreated", Width = 100, Visible = false });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleID", HeaderText = "ModuleID", Name = "ModuleID", Visible = false });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerID", HeaderText = "PartnerID", Name = "PartnerID", Visible = false });

            dgvAssessments.SelectionChanged += dgvAssessments_SelectionChanged;
        }

        private void LoadLookups()
        {
            // Load Modules
            _modules = _dbManager.GetAllModulesForLookup();
            cmbModule.DataSource = _modules;
            cmbModule.DisplayMember = "ModuleName";
            cmbModule.ValueMember = "ModuleID";

            // Load Partners
            _partners = _dbManager.GetAllPartnersForLookup();
            cmbPartner.DataSource = _partners;
            cmbPartner.DisplayMember = "PartnerName";
            cmbPartner.ValueMember = "PartnerID";
        }

        private void LoadAssessments()
        {
            List<Assessment> assessments = _dbManager.GetAllAssessments();
            dgvAssessments.DataSource = assessments;

            ClearInputFields();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
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
    }
}