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
    public partial class ParticipantDashboardForm : Form
    {
        private readonly DBManager _dbManager;
        private readonly Stakeholder _participant;

        public ParticipantDashboardForm(Stakeholder participant)
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            _participant = participant;
            lblWelcome.Text = $"Welcome, {_participant.FirstName} {_participant.LastName}!";
            SetupDataGridView();
            LoadAssessments();

            btnViewHistory.Text = "View History";

            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
        }

        private void SetupDataGridView()
        {
            dgvAssessments.AutoGenerateColumns = false;
            dgvAssessments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssessments.ReadOnly = true;
            dgvAssessments.AllowUserToAddRows = false;
            dgvAssessments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Define columns
            dgvAssessments.Columns.Clear();
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentName", HeaderText = "Assessment Title", Width = 200 });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartnerName", HeaderText = "Partner" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassPercentage", HeaderText = "Pass (%)" });
            dgvAssessments.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "HasTaken", HeaderText = "Taken?" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastScore", HeaderText = "Last Score" });
            dgvAssessments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LastAttemptDate", HeaderText = "Last Date" });
        }

        private void LoadAssessments()
        {
            // Fetches all available assessments along with the participant's last score
            List<AvailableAssessment> assessments = _dbManager.GetAvailableAssessmentsForParticipant(_participant.StakeholderID);
            dgvAssessments.DataSource = assessments;

            if (assessments.Count > 0)
            {
                dgvAssessments.Rows[0].Selected = true;
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            if (dgvAssessments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an assessment from the list to view its history.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected assessment object from the DataGridView
            AvailableAssessment selectedAssessment = dgvAssessments.SelectedRows[0].DataBoundItem as AvailableAssessment;

            if (selectedAssessment != null)
            {
                // Launch the new Results History Form
                ResultsHistoryForm historyForm = new ResultsHistoryForm(_participant, selectedAssessment);
                historyForm.ShowDialog();

                LoadAssessments(); // Reload the list to ensure the latest "Last Score" is shown
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
