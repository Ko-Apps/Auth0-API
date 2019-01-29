using scheduler.Models;
using System.Collections.Generic;
using System.Linq;

namespace scheduler.DataStore
{
    public static class ScheduleContextExtensions
    {
        public static void EnsureSeedDataForContext(this ScheduleContext context)
        {
            if (context.Schedules.Any())
            {
                return;
            }

            // init seed data 
            var Schedules = new List<Schedule>()
            {
                new Schedule()
                {
                    GoogleId = "ajbfeualfekan2394",
                    Date = "12/12/12",
                    Hours = 6
                },
                new Schedule()
                {
                    GoogleId = "a3242dualfekan2394",
                    Date = "12/11/12",
                    Hours = 5
                },
                new Schedule()
                {
                    GoogleId = "ajbf3242eualfekan2394",
                    Date = "01/01/12",
                    Hours = 9
                },
                new Schedule()
                {
                    GoogleId = "adeadeaalfekan2394",
                    Date = "12/12/12",
                    Hours = 8
                }
            };


            context.AddRange(Schedules);
            context.SaveChanges();
        }
    }
}
