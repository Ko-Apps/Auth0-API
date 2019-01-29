using Microsoft.EntityFrameworkCore;
using scheduler.Models;

namespace scheduler.DataStore
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> options)
         : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
