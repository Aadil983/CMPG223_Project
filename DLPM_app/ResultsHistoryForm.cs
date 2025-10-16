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
    public partial class ResultsHistoryForm : Form
    {
        private readonly DBManager _dbManager;
        private readonly Stakeholder _participant;
        private readonly AvailableAssessment _selectedAssessment;
        private readonly ParticipantAssessmentStatusModel _selectedStatusModel;
        private bool _isSpecificAssessment;

        // ✅ Constructor 1 — specific assessment (AvailableAssessment)
        public ResultsHistoryForm(Stakeholder participant, AvailableAssessment selectedAssessment)
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            _participant = participant;
            _selectedAssessment = selectedAssessment;
            _isSpecificAssessment = true;

            InitializeFormContent();
            SetupDataGridView();
            LoadSpecificAssessmentHistory();
        }

        // ✅ Constructor 2 — specific assessment (ParticipantAssessmentStatusModel)
        public ResultsHistoryForm(Stakeholder participant, ParticipantAssessmentStatusModel selectedAssessment)
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            _participant = participant;
            _selectedStatusModel = selectedAssessment;
            _isSpecificAssessment = true;

            InitializeFormContent();
            SetupDataGridView();
            LoadSpecificAssessmentHistory();
        }

        // ✅ Constructor 3 — all assessments for a participant
        public ResultsHistoryForm(Stakeholder participant)
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            _participant = participant;
            _isSpecificAssessment = false;

            this.Text = $"Assessment History for {_participant.FirstName} {_participant.LastName}";
            lblParticipantName.Text = $"Participant: {_participant.FirstName} {_participant.LastName} (ID: {_participant.StakeholderID})";
            lblAssessmentName.Text = "All Assessments";

            SetupDataGridView();
            LoadAllResultsHistory();
        }

        private void InitializeFormContent()
        {
            if (_isSpecificAssessment)
            {
                string assessmentName = _selectedAssessment != null ? _selectedAssessment.AssessmentName : _selectedStatusModel?.AssessmentName;
                string moduleName = _selectedAssessment != null ? _selectedAssessment.ModuleName : _selectedStatusModel?.ModuleName;

                this.Text = $"{assessmentName} History";
                lblParticipantName.Text = $"Participant: {_participant.FirstName} {_participant.LastName} (ID: {_participant.StakeholderID})";
                lblAssessmentName.Text = $"Assessment: {assessmentName} ({moduleName})";
            }
        }

        private void SetupDataGridView()
        {
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.ReadOnly = true;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToResizeColumns = true;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentDate", HeaderText = "Date Taken", Width = 150 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Mark", HeaderText = "Score", DefaultCellStyle = new DataGridViewCellStyle { Format = "P0" }, Width = 80 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassStatus", HeaderText = "Status", Width = 80 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentName", HeaderText = "Assessment", Width = 150 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module", Width = 150 });
        }

        // ✅ Loads results for one assessment
        private void LoadSpecificAssessmentHistory()
        {
            int assessmentId = _selectedAssessment != null ? _selectedAssessment.AssessmentID : _selectedStatusModel.AssessmentID;
            List<ModuleAssessmentResult> history = _dbManager.GetAssessmentResultsHistory(
                _participant.StakeholderID, assessmentId);

            if (history.Count == 0)
            {
                MessageBox.Show("No previous results found for this assessment.", "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dgvHistory.DataSource = history;
        }

        // ✅ Loads results for all assessments
        private void LoadAllResultsHistory()
        {
            List<ModuleAssessmentResult> history = _dbManager.GetAllResultsForParticipant(_participant.StakeholderID);

            if (history == null || history.Count == 0)
            {
                MessageBox.Show("No assessment results found for this participant.", "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvHistory.DataSource = history;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}