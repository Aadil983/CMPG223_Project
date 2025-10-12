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

    public partial class ModuleMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public ModuleMaintenanceForm()
        {
            InitializeComponent();
            _dbManager = new DBManager(Constants.ConnectionString);
            this.Text = "Module Maintenance (Add/Edit/Delete)";
            SetupDataGridView();
            LoadModules();
        }

        private void SetupDataGridView()
        {
            dgvModules.AutoGenerateColumns = false;
            dgvModules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvModules.ReadOnly = true;
            dgvModules.AllowUserToAddRows = false;

            dgvModules.Columns.Clear();
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleID", HeaderText = "ID", Name = "ModuleID", Width = 50 });
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module Name", Name = "ModuleName", Width = 150 });

            // FIX: Set AutoSizeMode instead of AutoSizeColumnMode
            var descColumn = new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Description", Name = "Description" };
            descColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvModules.Columns.Add(descColumn);

            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DurationHours", HeaderText = "Hours", Name = "DurationHours", Width = 60 });
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status", Name = "Status", Width = 80 });

            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContentLink", Name = "ContentLink", Visible = false });
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DateCreated", Name = "DateCreated", Visible = false });

            dgvModules.SelectionChanged += dgvModules_SelectionChanged;
        }

        private void LoadModules()
        {
            List<TrainingModule> modules = _dbManager.GetAllModules();
            dgvModules.DataSource = modules;

            ClearInputFields();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ClearInputFields()
        {
            txtModuleID.Text = string.Empty;
            txtModuleName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtContentLink.Text = string.Empty;
            txtDurationHours.Text = string.Empty;
            cmbStatus.SelectedIndex = -1; // Clear combo box selection
            btnAdd.Text = "Add Module";
            txtModuleName.Focus();
        }

        private void dgvModules_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvModules.SelectedRows.Count > 0)
            {
                TrainingModule selectedModule = dgvModules.SelectedRows[0].DataBoundItem as TrainingModule;
                if (selectedModule != null)
                {
                    txtModuleID.Text = selectedModule.ModuleID.ToString();
                    txtModuleName.Text = selectedModule.ModuleName;
                    txtDescription.Text = selectedModule.Description;
                    txtContentLink.Text = selectedModule.ContentLink;
                    txtDurationHours.Text = selectedModule.DurationHours.ToString();
                    cmbStatus.SelectedItem = selectedModule.Status;

                    btnAdd.Text = "Clear";
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            else
            {
                ClearInputFields();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Module";
                return;
            }

            // 1. Basic Validation
            if (string.IsNullOrWhiteSpace(txtModuleName.Text) || string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtDurationHours.Text) || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in the Module Name, Description, Duration, and Status.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDurationHours.Text, out int duration))
            {
                MessageBox.Show("Duration (Hours) must be a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Collect Data
            string name = txtModuleName.Text;
            string desc = txtDescription.Text;
            string link = txtContentLink.Text;
            string status = cmbStatus.SelectedItem.ToString();

            // 3. Call DBManager
            int newId = _dbManager.AddModule(name, desc, link, duration, status);

            if (newId > 0)
            {
                MessageBox.Show($"Module '{name}' added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModules();
            }
            else
            {
                MessageBox.Show("Failed to add module. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModuleID.Text))
            {
                MessageBox.Show("Please select a module to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Basic Validation
            if (string.IsNullOrWhiteSpace(txtModuleName.Text) || string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtDurationHours.Text) || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all required fields for the update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDurationHours.Text, out int duration) || !int.TryParse(txtModuleID.Text, out int moduleId))
            {
                MessageBox.Show("Duration or Module ID is invalid.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Collect Data
            string name = txtModuleName.Text;
            string desc = txtDescription.Text;
            string link = txtContentLink.Text;
            string status = cmbStatus.SelectedItem.ToString();

            // 3. Call DBManager
            bool success = _dbManager.UpdateModule(moduleId, name, desc, link, duration, status);

            if (success)
            {
                MessageBox.Show($"Module ID {moduleId} updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModules();
            }
            else
            {
                MessageBox.Show("Failed to update module. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvModules.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a module to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TrainingModule selectedModule = dgvModules.SelectedRows[0].DataBoundItem as TrainingModule;
            if (selectedModule == null) return;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Module ID {selectedModule.ModuleID}: {selectedModule.ModuleName}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeleteModule(selectedModule.ModuleID);

                if (success)
                {
                    MessageBox.Show("Module deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadModules();
                }
                else
                {
                    MessageBox.Show("Failed to delete module. It may be referenced by participant enrollments or assessments (Foreign Key Constraint).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}