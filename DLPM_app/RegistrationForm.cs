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

    public partial class RegistrationForm : Form
    {
        // The DBManager is now accessible because it is defined in Program.cs
        private readonly DBManager _dbManager;

        public RegistrationForm()
        {
            InitializeComponent();

            // Initialize DBManager using the constant connection string defined in Program.cs
            _dbManager = new DBManager(Constants.ConnectionString);
        }

        private void lblBackToLogin_Click(object sender, EventArgs e)
        {
            // Simple navigation to close the registration form
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Basic Input Validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtIDNumber.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, Password, ID Number, Contact Number).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Gather Data for Participant
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string idNumber = txtIDNumber.Text.Trim();
            string contactNumber = txtContactNumber.Text.Trim();

            // 3. Call the DBManager to register the participant
            int newId = _dbManager.RegisterParticipant(
                firstName,
                lastName,
                email,
                password,
                idNumber,
                contactNumber
            );

            // 4. Handle Result
            if (newId > 0)
            {
                MessageBox.Show($"Registration successful! Welcome {firstName}. You can now log in.",
                                "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the registration form and return to the login screen
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. A user with that email or ID number may already exist.",
                                "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}