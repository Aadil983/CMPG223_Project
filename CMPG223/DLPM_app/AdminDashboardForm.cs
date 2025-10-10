using System.Windows.Forms;
using DLPM;

namespace WindowsFormsApp1
{
    // Minimal placeholder to satisfy references from LoginForm.
    public class AdminDashboardForm : Form
    {
        private readonly Stakeholder _user;

        public AdminDashboardForm(Stakeholder user)
        {
            _user = user;
            this.Text = "Admin Dashboard";
            this.Width = 800;
            this.Height = 600;
        }
    }
}