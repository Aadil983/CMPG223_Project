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
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    // NOTE: This file assumes the controls are named:
    // dgvModules, txtModuleID, txtName, txtDescription, txtOutline, btnAdd, btnUpdate, btnDelete
    public partial class ModuleMaintenanceForm : Form
    {
        private readonly DBManager _dbManager;

        public ModuleMaintenanceForm()
        {
            InitializeComponent();

            try
            {
                _dbManager = new DBManager(Constants.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization Error: {ex.Message}", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _dbManager = null;
                return;
            }

            SetupDataGridView();
            LoadModules();
        }

        /// <summary>
        /// Configures the DataGridView columns to match the ModuleMaintenanceModel.
        /// </summary>
        private void SetupDataGridView()
        {
            dgvModules.AutoGenerateColumns = false;
            dgvModules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvModules.ReadOnly = true;
            dgvModules.AllowUserToAddRows = false;
            dgvModules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvModules.Columns.Clear();

            // Display Columns
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleID", HeaderText = "ID", Width = 50 });
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ModuleName", HeaderText = "Module Name", Width = 200 });
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Description", Width = 300 });

            // Hidden Column (ContentOutline is usually too long for a summary column)
            dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ContentOutline", Visible = false });
        }

        /// <summary>
        /// Calls the DBManager to fetch all module data and binds it to the grid.
        /// </summary>
        private void LoadModules()
        {
            if (_dbManager == null) return;
            try
            {
                List<ModuleMaintenanceModel> modules = _dbManager.GetAllModules();
                dgvModules.DataSource = modules;

                // Select the first row if data exists
                if (modules.Count > 0)
                {
                    dgvModules.Rows[0].Selected = true;
                }
                else
                {
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load modules: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadModules();
        }

        /// <summary>
        /// Clears all input fields and resets buttons.
        /// </summary>
        private void ClearInputFields()
        {
            txtModuleID.Text = string.Empty;
            txtModuleName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            btnAdd.Text = "Add Module";
            txtModuleName.Focus();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            dgvModules.ClearSelection();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtModuleName.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Module Name and Description fields are required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Clear")
            {
                ClearInputFields();
                btnAdd.Text = "Add Module";
                return;
            }

            if (!ValidateInput()) return;

            string name = txtModuleName.Text;
            string description = txtDescription.Text;
            string outline = txtContentLink.Text;

            int newId = _dbManager.AddModule(name, description, outline);

            if (newId > 0)
            {
                MessageBox.Show($"Module '{name}' added successfully! ID: {newId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModules();
            }
            else
            {
                MessageBox.Show("Failed to add module. Check console for database errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvModules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // IMPORTANT: Check if a single row is selected to prevent errors during binding updates
            if (dgvModules.CurrentRow == null || dgvModules.CurrentRow.DataBoundItem == null)
            {
                ClearInputFields();
                return;
            }

            ModuleMaintenanceModel selectedModule = dgvModules.CurrentRow.DataBoundItem as ModuleMaintenanceModel;

            if (selectedModule != null)
            {
                txtModuleID.Text = selectedModule.ModuleID.ToString();
                txtModuleName.Text = selectedModule.ModuleName;
                txtDescription.Text = selectedModule.Description;
                txtContentLink.Text = selectedModule.ContentOutline;

                btnAdd.Text = "Clear";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                ClearInputFields();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvModules.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtModuleID.Text))
            {
                MessageBox.Show("Please select a module to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int moduleId = int.Parse(txtModuleID.Text);
            string name = txtModuleName.Text;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete Module ID {moduleId}: {name}? This may affect associated enrollments, assessments, and partners.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _dbManager.DeleteModule(moduleId);

                if (success)
                {
                    MessageBox.Show("Module deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadModules();
                }
                else
                {
                    // This specific message is important if a foreign key constraint prevents deletion
                    MessageBox.Show("Failed to delete module. It may be referenced by other records (e.g., enrollments or assessments).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModuleID.Text))
            {
                MessageBox.Show("Please select a module to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int moduleId = int.Parse(txtModuleID.Text);
            string name = txtModuleName.Text;
            string description = txtDescription.Text;
            string outline = txtContentLink.Text;

            bool success = _dbManager.UpdateModule(moduleId, name, description, outline);

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
    }
}
