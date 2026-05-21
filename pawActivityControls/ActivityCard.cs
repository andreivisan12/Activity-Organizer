using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pawActivityControls
{
    public partial class ActivityCard : UserControl
    {
        public ActivityCard()
        {
            InitializeComponent();
        }

        private void ActivityCard_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        public void SetData(string title, string date, string category)
        {
            lblTitle.Text = title;
            lblDate.Text = date;
            lblCategory.Text = category;
        }
    }
}
