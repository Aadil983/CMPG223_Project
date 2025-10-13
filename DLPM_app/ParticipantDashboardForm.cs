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
                MessageBox.Show($"Initialization Error: {ex.Message}", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _dbManager = null;
                return;
            }

            lblWelcome.Text = $"Welcome, {_participant.FirstName} {_participant.LastName}!";
            SetupDataGridView();
            LoadAssessmentStatus(); // Renamed to reflect current data structure

            // Ensure the event handler is attached if not done in the designer
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);

            // Adding a visual cue for status
            dgvAssessments.CellFormatting += DgvAssessments_CellFormatting;
        }

        private void SetupDataGridView()
        {
            dgvAssessments.AutoGenerateColumns = false;
            dgvAssessments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssessments.ReadOnly = true;
            dgvAssessments.AllowUserToAddRows = false;
            dgvAssessments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Define columns using the properties from ParticipantAssessmentStatusModel
            dgvAssessments.Columns.Clear();
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentName", HeaderText = "Assessment Title" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassPercentage", HeaderText = "Pass (%)" });
            // NEW: Displays if the participant is enrolled in the module
            dgvAssessments.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "IsEnrolled", HeaderText = "Enrolled?" });
            // Updated column to use the new model property names
            dgvAssessments.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "HasTakenAssessment", HeaderText = "Taken?" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastScore", HeaderText = "Last Score" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastAttemptDate", HeaderText = "Last Date", DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm" } });
        }

        private void LoadAssessmentStatus()
        {
            if (_dbManager == null) return;

            // Fetches all assessment statuses using the correct method and model
            List<ParticipantAssessmentStatusModel> assessments = _dbManager.GetParticipantAssessmentStatus(_participant.StakeholderID);
            dgvAssessments.DataSource = assessments;

            if (assessments.Count > 0)
            {
                // This line might throw an error if AutoGenerateColumns=false is set before columns are added.
                // It's safer to use ClearSelection() and let the user select a row.
                dgvAssessments.Rows[0].Selected = true; 
            }
        }

        /// <summary>
        /// Adds color formatting to rows based on assessment status.
        /// </summary>
        private void DgvAssessments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvAssessments.Rows[e.RowIndex];
                var status = row.DataBoundItem as ParticipantAssessmentStatusModel;

                if (status != null)
                {
                    if (!status.IsEnrolled)
                    {
                        // Not Enrolled: Light gray background
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                    }
                    else if (status.HasTakenAssessment)
                    {
                        // Check if the score is less than the required pass percentage
                        if (status.LastScore.HasValue && status.LastScore.Value < status.PassPercentage)
                        {
                            // Failed Attempt: Light coral background
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            // Passed Attempt: Light green background
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }
                    else
                    {
                        // Enrolled but Not Taken: Default white
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an assessment from the list to view its history.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected assessment object from the DataGridView using the corrected model
            ParticipantAssessmentStatusModel selectedAssessment = dgvAssessments.SelectedRows[0].DataBoundItem as ParticipantAssessmentStatusModel;

            if (selectedAssessment != null)
            {
                // Launch the new Results History Form
                ResultsHistoryForm historyForm = new ResultsHistoryForm(_participant, selectedAssessment);
                historyForm.ShowDialog();

                LoadAssessmentStatus(); // Reload the list to ensure the latest "Last Score" is shown
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Close the dashboard and show the login form again
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.Show();
            this.Close();
        }
    }
}