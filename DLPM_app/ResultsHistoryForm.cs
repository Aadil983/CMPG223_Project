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
        private ParticipantAssessmentStatusModel selectedAssessment;

        public ResultsHistoryForm(Stakeholder participant, AvailableAssessment selectedAssessment)
    {
        InitializeComponent();
        _dbManager = new DBManager(Constants.ConnectionString);
        _participant = participant;
        _selectedAssessment = selectedAssessment;

        InitializeFormContent();
        SetupDataGridView();
        LoadResultsHistory();
    }

        public ResultsHistoryForm(Stakeholder participant, ParticipantAssessmentStatusModel selectedAssessment)
        {
            _participant = participant;
            this.selectedAssessment = selectedAssessment;
        }

        private void InitializeFormContent()
    {
        this.Text = $"{_selectedAssessment.AssessmentName} History";
        lblParticipantName.Text = $"Participant: {_participant.FirstName} {_participant.LastName} (ID: {_participant.StakeholderID})";
        lblAssessmentName.Text = $"Assessment: {_selectedAssessment.AssessmentName} ({_selectedAssessment.ModuleName})";
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
        // Mark is displayed as a percentage. Using "P0" format code for no decimal places.
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentDate", HeaderText = "Date Taken", Width = 150 });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Mark", HeaderText = "Score", DefaultCellStyle = new DataGridViewCellStyle { Format = "P0" }, Width = 80 }); 
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassStatus", HeaderText = "Status", Width = 80 });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnrollmentID", HeaderText = "Enrollment ID", Width = 120 });
    }

    private void LoadResultsHistory()
    {
        // Fetch all attempts for this participant and assessment using the DBManager method
        List<ModuleAssessmentResult> history = _dbManager.GetAssessmentResultsHistory(
            _participant.StakeholderID,
            _selectedAssessment.AssessmentID);

        if (history.Count == 0)
        {
            MessageBox.Show($"No previous results found for {_selectedAssessment.AssessmentName}.", "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        dgvHistory.DataSource = history;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
}
