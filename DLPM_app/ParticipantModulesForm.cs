using System.Windows.Forms;
using DLPM;

namespace WindowsFormsApp1
{
    // Minimal placeholder to satisfy references from LoginForm.
    public class ParticipantModulesForm : Form
    {
        private readonly Stakeholder _user;

        public ParticipantModulesForm(Stakeholder user)
        {
            _user = user;
            this.Text = "Participant Modules";
            this.Width = 800;
            this.Height = 600;
        }
    }
}