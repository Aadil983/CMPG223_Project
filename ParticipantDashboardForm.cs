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
    public partial class ParticipantDashboardForm : Form
    {
        private readonly DBManager _dbManager;
        private readonly Stakeholder _participant;

        public ParticipantDashboardForm(Stakeholder participant)
        {
            InitializeComponent();
            _participant = participant;

            try
            {
                _dbManager = new DBManager(Constants.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization Error: {ex.Message}", "Fatal Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _dbManager = null;
                return;
            }

            lblWelcome.Text = $"Welcome, {_participant.FirstName} {_participant.LastName}!";
            SetupDataGridView();
            LoadAssessments();

            btnViewHistory.Click += btnViewHistory_Click;
            btnRequestEnrollment.Click += btnRequestEnrollment_Click;
            dgvAssessments.SelectionChanged += DgvAssessments_SelectionChanged;
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

        /// <summary>
        /// Set up DataGridView columns and appearance
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
        /// When user selects a row, enable/disable the enrollment button
        /// </summary>
        private void DgvAssessments_SelectionChanged(object sender, EventArgs e)
        {
            btnRequestEnrollment.Enabled = dgvAssessments.SelectedRows.Count > 0;
        }

        /// <summary>
        /// Open the participant's results history form
        /// </summary>
        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            var historyForm = new ResultsHistoryForm(_participant);
            historyForm.ShowDialog();
        }

        /// <summary>
        /// Allow participant to request enrollment for a selected module
        /// </summary>
        private void btnRequestEnrollment_Click(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an assessment to enroll in.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvAssessments.SelectedRows[0];
            int moduleID = Convert.ToInt32(selectedRow.Cells["ModuleID"].Value);
            string moduleName = selectedRow.Cells["ModuleName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Do you want to enroll in module '{moduleName}'?",
                "Confirm Enrollment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string insertQuery = @"
                        INSERT INTO Enrollment (ParticipantStakeholderID, ModuleID, EnrollmentDate)
                        VALUES (@ParticipantID, @ModuleID, GETDATE());";

                    var parameters = new Dictionary<string, object>
                    {
                        { "@ParticipantID", _participant.StakeholderID },
                        { "@ModuleID", moduleID }
                    };

                    _dbManager.ExecuteNonQuery(insertQuery, parameters);

                    MessageBox.Show("Enrollment request successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error enrolling in module: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}