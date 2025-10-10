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
    public partial class LoginForm : Form
    {

        private readonly DBManager _dbManager;

        public LoginForm()
        {
            InitializeComponent();

            // Ensure you initialize the DBManager with the connection string
            // The Constants.ConnectionString is defined in your Security.cs file.
            _dbManager = new DBManager(Constants.ConnectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Attempt to log in the user
            Stakeholder loggedInUser = _dbManager.LoginUser(email, password);

            if (loggedInUser != null)
            {
                // 2. Determine the user's role and redirect
                MessageBox.Show($"Login Successful! Welcome {loggedInUser.FirstName}. You are a {loggedInUser.RoleType}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hide the login form
                this.Hide();

                // *** ROLE-BASED REDIRECTION LOGIC ***
                Form nextForm;

                if (loggedInUser.RoleType == "Admin" || loggedInUser.RoleType == "Instructor")
                {
                    // Navigate to the main administrative dashboard
                    nextForm = new AdminDashboardForm(loggedInUser);
                }
                else if (loggedInUser.RoleType == "Participant")
                {
                    // Navigate to the participant view/module list
                    nextForm = new ParticipantModulesForm(loggedInUser);
                }
                else
                {
                    // For 'Partner' or any unhandled role
                    MessageBox.Show("Your role is not configured for a main application dashboard.", "Role Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Show(); // Show login again or close
                    return;
                }

                // Show the next form and handle application exit when it closes
                nextForm.FormClosed += (s, args) => this.Close();
                nextForm.Show();
            }
            else
            {
                // The DBManager already prints the error to the console, but for GUI we need a message box
                MessageBox.Show("Login failed. Check your email and password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegisterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Opening Registration Form.", "Navigate", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var reg = new RegistrationForm();
            this.Hide();               // hide the login form instance
            reg.FormClosed += (s, args) => this.Show(); // show login when registration closes
            reg.Show();                // show registration form modelessly
            reg.Focus();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
