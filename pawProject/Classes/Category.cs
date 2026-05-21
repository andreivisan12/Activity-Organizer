using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pawProject.Classes
{
    public class Category
    {
        public int CategoryId { get; set; } // Primary Key
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property: A category can have many activities
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
