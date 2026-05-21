using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pawProject.Classes;

namespace pawProject
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void LoadActivities()
        {
            flowLayoutPanel1.Controls.Clear();

            using (var context = new pawProject.Classes.OrganizerContext())
            {
                var activityList = context.Activities
                    .Select(a => new
                    {
                        ID = a.ActivityId,
                        Title = a.Title,
                        Date = a.DatePlanned,
                        Duration = a.DurationMinutes + " min",
                        CategoryName = a.Category.Name,
                        ProjectTitle = a.Project.Title
                    }).ToList();

                dgvActivities.DataSource = activityList;

                if (dgvActivities.Columns["ID"] != null)
                {
                    dgvActivities.Columns["ID"].Visible = false;
                }

                foreach (var act in activityList)
                {
                    pawActivityControls.ActivityCard newCard = new pawActivityControls.ActivityCard();

                    newCard.SetData(act.Title, act.Date.ToShortDateString(), act.CategoryName);

                    flowLayoutPanel1.Controls.Add(newCard);
                }
                panelChart.Invalidate();
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadActivities();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddEditActivityForm addForm = new AddEditActivityForm();

            addForm.ShowDialog();

            LoadActivities();
        }

        private void exportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var context = new pawProject.Classes.OrganizerContext())
            {
                var exportList = context.Activities.Select(a => new ActivityExport
                {
                    Title = a.Title,
                    Date = a.DatePlanned.ToString(),
                    Duration = a.DurationMinutes,
                    Category = a.Category.Name,
                    Project = a.Project.Title
                }).ToList();

                if (!exportList.Any())
                {
                    MessageBox.Show("There are no activities to export!", "Empty Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog.Title = "Export Activities as XML";
                saveFileDialog.FileName = "MyActivities.xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ActivityExport>));

                        using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            serializer.Serialize(stream, exportList);
                        }

                        MessageBox.Show("Activities successfully exported to XML!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (var context = new pawProject.Classes.OrganizerContext())
            {
                var chartData = context.Activities
                    .GroupBy(a => a.Category.Name)
                    .Select(group => new { CategoryName = group.Key, Count = group.Count() })
                    .ToList();

                if (!chartData.Any()) return;

                int padding = 40;
                int barWidth = 60;
                int spacing = 40;
                int maxBarHeight = panelChart.Height - (padding * 2);

                int maxCount = chartData.Max(d => d.Count);
                if (maxCount == 0) maxCount = 1; 

                Pen axisPen = new Pen(Color.Black, 2);
                g.DrawLine(axisPen, padding, panelChart.Height - padding, panelChart.Width - padding, panelChart.Height - padding); // X Axis
                g.DrawLine(axisPen, padding, padding, padding, panelChart.Height - padding); // Y Axis

                int currentX = padding + spacing;
                Font textFont = new Font("Segoe UI", 10, FontStyle.Bold);

                foreach (var item in chartData)
                {
                    int barHeight = (int)((double)item.Count / maxCount * maxBarHeight);
                    int yPos = panelChart.Height - padding - barHeight;

                    g.FillRectangle(Brushes.CornflowerBlue, currentX, yPos, barWidth, barHeight);

                    g.DrawRectangle(Pens.DarkBlue, currentX, yPos, barWidth, barHeight);

                    g.DrawString(item.CategoryName, textFont, Brushes.Black, currentX, panelChart.Height - padding + 5);

                    g.DrawString(item.Count.ToString(), textFont, Brushes.Black, currentX + (barWidth / 3), yPos - 20);


                    currentX += barWidth + spacing;
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var context = new pawProject.Classes.OrganizerContext())
            {

                var reportData = context.Activities
                    .Select(a => new
                    {
                        a.Title,
                        a.DatePlanned,
                        a.DurationMinutes,
                        CategoryName = a.Category.Name,
                        ProjectName = a.Project.Title
                    }).ToList();

                if (!reportData.Any())
                {
                    MessageBox.Show("There are no activities to include in the report.", "Empty Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save Activity Report";
                saveFileDialog.FileName = "ActivityReport.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            writer.WriteLine("                 ACTIVITIES MASTER REPORT                ");
                            writer.WriteLine("=========================================================");
                            writer.WriteLine($"Generated on: {DateTime.Now.ToString("f")}");
                            writer.WriteLine("---------------------------------------------------------");
                            writer.WriteLine();

                            foreach (var act in reportData)
                            {
                                writer.WriteLine($"ACTIVITY: {act.Title.ToUpper()}");
                                writer.WriteLine($"  Date:      {act.DatePlanned.ToShortDateString()}");
                                writer.WriteLine($"  Duration:  {act.DurationMinutes} minutes");
                                writer.WriteLine($"  Category:  {act.CategoryName}");
                                writer.WriteLine($"  Project:   {act.ProjectName}");
                                writer.WriteLine("---------------------------------------------------------");
                            }

                            writer.WriteLine();
                            writer.WriteLine("                    END OF REPORT                        ");
                            writer.WriteLine("=========================================================");
                        }

                        MessageBox.Show("Report successfully generated and saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error writing to file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void dgvActivities_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvActivities.ClearSelection();
                dgvActivities.Rows[e.RowIndex].Selected = true;

                string dragText = dgvActivities.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                dgvActivities.DoDragDrop(dragText, DragDropEffects.Copy);
            }
        }

        private void txtNotes_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy; // Show the [+] cursor!
            }
            else
            {
                e.Effect = DragDropEffects.None; // Show the red circle
            }
        }

        private void txtNotes_DragDrop(object sender, DragEventArgs e)
        {
            // Grab the text and append it to the box
            string droppedText = (string)e.Data.GetData(DataFormats.StringFormat);
            txtNotes.AppendText("• Dragged Activity: " + droppedText + Environment.NewLine);
        }

        private void deleteActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvActivities.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to permanently delete this activity?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int selectedId = (int)dgvActivities.SelectedRows[0].Cells["ID"].Value;

                    using (var context = new pawProject.Classes.OrganizerContext())
                    {
                        var activityToDelete = context.Activities.Find(selectedId);

                        if (activityToDelete != null)
                        {
                            context.Activities.Remove(activityToDelete);
                            context.SaveChanges();

                            LoadActivities();

                            MessageBox.Show("Activity deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please left-click a row to select it before deleting.", "Select an Activity", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            LoadActivities();

            panelChart.Invalidate();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
    [Serializable]
    public class ActivityExport
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public int Duration { get; set; }
        public string Category { get; set; }
        public string Project { get; set; }
    }
}
