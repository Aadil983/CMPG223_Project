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
    public partial class AdminDashboardForm : Form
    {
        // Store the logged-in user's details
        private readonly Stakeholder _currentUser;

        public AdminDashboardForm(Stakeholder user)
        {
            InitializeComponent();
            _currentUser = user;

            // Customize the window title and welcome message based on the logged-in user
            this.Text = $"{user.RoleType} Dashboard - DLPM";
            lblWelcome.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName} ({_currentUser.RoleType})";
        }

        // --- Navigation Handlers ---
        private void AdminDashboardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // When the dashboard is closed, the main login form will reappear, as handled in LoginForm.cs
        }

        private void btnModules_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Opening Module Maintenance...", "Navigation");
            ModuleMaintenanceForm moduleForm = new ModuleMaintenanceForm();
            moduleForm.ShowDialog();
        }

        private void btnPartners_Click_1(object sender, EventArgs e)
        {
            CommunityPartnerMaintenanceForm partnerForm = new CommunityPartnerMaintenanceForm();
            partnerForm.ShowDialog();
        }

        private void btnParticipants_Click_1(object sender, EventArgs e)
        {
            StakeholderMaintenanceForm partnerForm = new StakeholderMaintenanceForm();
            partnerForm.ShowDialog();
        }

        private void btnAssessments_Click_1(object sender, EventArgs e)
        {
            AssessmentMaintenanceForm assessmentForm = new AssessmentMaintenanceForm();
            assessmentForm.ShowDialog();
        }

        private void btnReports_Click_1(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void btnViewEnrollmentRequests_Click(object sender, EventArgs e)
        {
            EnrollmentForm enrollmentForm = new EnrollmentForm(_currentUser);
            enrollmentForm.ShowDialog();
        }
    }
}