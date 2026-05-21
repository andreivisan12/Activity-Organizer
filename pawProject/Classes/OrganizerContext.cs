using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pawProject.Classes
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext() : base("name=OrganizerDB")
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
