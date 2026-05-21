using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pawProject
{
    public partial class AddEditActivityForm : Form
    {
        public AddEditActivityForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid Category and Project from the dropdown lists.",
                    "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the code right here, don't try to save!
            }

            using (var context = new pawProject.Classes.OrganizerContext())
            {
                var newActivity = new pawProject.Classes.Activity
                {
                    Title = textBox1.Text,
                    DatePlanned = dateTimePicker1.Value,
                    DurationMinutes = (int)numericUpDown1.Value,

                    // Because of our safety check above, we guarantee these won't be null
                    CategoryId = (int)comboBox1.SelectedValue,
                    ProjectId = (int)comboBox2.SelectedValue
                };

                context.Activities.Add(newActivity);
                context.SaveChanges();

                MessageBox.Show("Activity Saved Successfully!");
                this.Close();
            }
        }
        private void AddEditActivityForm_Load(object sender, EventArgs e)
        {
            using (var context = new pawProject.Classes.OrganizerContext())
            {
                // If there are no categories in the DB, create some dummy ones!
                if (!context.Categories.Any())
                {
                    context.Categories.Add(new pawProject.Classes.Category { Name = "Uni", Description = "University tasks" });
                    context.Categories.Add(new pawProject.Classes.Category { Name = "Personal", Description = "Life stuff" });

                    context.Projects.Add(new pawProject.Classes.Project { Title = "Act Manager", Deadline = DateTime.Now.AddDays(30) });

                    context.SaveChanges(); // Save the dummy data
                }

                // Now bind the real database data to your ComboBoxes
                comboBox1.DataSource = context.Categories.ToList();
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "CategoryId";

                comboBox2.DataSource = context.Projects.ToList();
                comboBox2.DisplayMember = "Title";
                comboBox2.ValueMember = "ProjectId";
            }
        }
    }
}
