using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Time_Table_Database.DataModels;

namespace Time_Table_Database.Core
{
    public class TimeTableContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Timing> Timings { get; set; }
        public DbSet<SelectedEntries> SelectedEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timing>().HasKey(t => new { t.Day, t.Hour });
            base.OnModelCreating(modelBuilder);
        }

    }
}