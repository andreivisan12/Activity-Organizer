using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pawProject.Classes
{
    public class Activity
    {
        public int ActivityId { get; set; } // Primary Key
        public string Title { get; set; }
        public DateTime DatePlanned { get; set; }
        public int DurationMinutes { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }
        public int ProjectId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Project Project { get; set; }
    }
}
