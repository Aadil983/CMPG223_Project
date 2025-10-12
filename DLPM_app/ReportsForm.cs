using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLPM;
using static DLPM.DBManager;

namespace WindowsFormsApp1
{
    public partial class ReportsForm : Form
    {
        private readonly DBManager _dbManager;
        private const string ParticipantReport = "Participant Performance Summary";
        private const string AssessmentReport = "Assessment Module Summary";

        public ReportsForm()
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            LoadLookups();
            cmbReportType.SelectedIndex = 0; // Default to first report
            SetupDataGridView(ParticipantReport);
        }

        private void LoadLookups()
        {
            // Add "All" option to modules
            var modules = new List<ModuleLookup> { new ModuleLookup { ModuleID = 0, ModuleName = "All Modules" } };
            modules.AddRange(_dbManager.GetAllModulesForLookup());
            cmbModule.DataSource = modules;
            cmbModule.DisplayMember = "ModuleName";
            cmbModule.ValueMember = "ModuleID";

            // Add "All" option to partners
            var partners = new List<PartnerLookup> { new PartnerLookup { PartnerID = 0, PartnerName = "All Partners" } };
            partners.AddRange(_dbManager.GetAllPartnersForLookup());
            cmbPartner.DataSource = partners;
            cmbPartner.DisplayMember = "PartnerName";
            cmbPartner.ValueMember = "PartnerID";
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedReport = cmbReportType.SelectedItem?.ToString();
            SetupDataGridView(selectedReport);
            ToggleFilters(selectedReport);
            dgvReports.DataSource = null; // Clear previous report data
        }

        private void ToggleFilters(string reportType)
        {
            // Reset all to hidden/disabled first
            lblModule.Visible = cmbModule.Visible = false;
            lblPartner.Visible = cmbPartner.Visible = false;
            lblStartDate.Visible = dtpStartDate.Visible = false;
            lblEndDate.Visible = dtpEndDate.Visible = false;

            if (reportType == ParticipantReport)
            {
                // Participant report uses Module and Partner filters
                lblModule.Visible = cmbModule.Visible = true;
                lblPartner.Visible = cmbPartner.Visible = true;
            }
            else if (reportType == AssessmentReport)
            {
                // Assessment report uses Date Range filters
                lblStartDate.Visible = dtpStartDate.Visible = true;
                lblEndDate.Visible = dtpEndDate.Visible = true;
            }
        }

        private void SetupDataGridView(string reportType)
        {
            dgvReports.Columns.Clear();
            dgvReports.AutoGenerateColumns = false;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.ReadOnly = true;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (reportType == ParticipantReport)
            {
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ParticipantName", HeaderText = "Participant Name" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IDNumber", HeaderText = "ID Number" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentsTaken", HeaderText = "Taken" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentsPassed", HeaderText = "Passed" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AverageScorePercentage", HeaderText = "Avg Score (%)" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OverallStatus", HeaderText = "Status" });
            }
            else if (reportType == AssessmentReport)
            {
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AssessmentName", HeaderText = "Assessment" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DateCreated", HeaderText = "Created Date" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalParticipants", HeaderText = "Total Attempts" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AverageScorePercentage", HeaderText = "Avg Score (%)" });
                dgvReports.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PassRatePercentage", HeaderText = "Pass Rate (%)" });
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string selectedReport = cmbReportType.SelectedItem?.ToString();

            if (selectedReport == ParticipantReport)
            {
                int? moduleId = (int)cmbModule.SelectedValue != 0 ? (int?)cmbModule.SelectedValue : null;
                int? partnerId = (int)cmbPartner.SelectedValue != 0 ? (int?)cmbPartner.SelectedValue : null;

                var data = _dbManager.GetParticipantPerformanceSummary(moduleId, partnerId);
                dgvReports.DataSource = data;

                if (data.Count == 0)
                {
                    MessageBox.Show("No participant performance data found for the selected filters.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (selectedReport == AssessmentReport)
            {
                DateTime? startDate = dtpStartDate.Visible ? (DateTime?)dtpStartDate.Value : null;
                DateTime? endDate = dtpEndDate.Visible ? (DateTime?)dtpEndDate.Value : null;

                if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
                {
                    MessageBox.Show("Start Date cannot be after End Date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var data = _dbManager.GetAssessmentSummaryByModule(startDate, endDate);
                dgvReports.DataSource = data;

                if (data.Count == 0)
                {
                    MessageBox.Show("No assessment summary data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
