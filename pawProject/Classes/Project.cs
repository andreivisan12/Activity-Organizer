using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pawProject.Classes
{
    public class Project
    {
        public int ProjectId { get; set; } // Primary Key
        public string Title { get; set; }
        public DateTime Deadline { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
