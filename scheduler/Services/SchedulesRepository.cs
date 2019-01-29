using System.Collections.Generic;
using System.Linq;
using scheduler.DataStore;
using scheduler.Models;

namespace scheduler.Services
{
    public class SchedulesRepository : ISchdulesRepository
    {
        private ScheduleContext _context;

        public SchedulesRepository(ScheduleContext context)
        {
            _context = context;
        }

        public Schedule GetSchedule(int id)
        {
            return _context.Schedules.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Schedule> GetSchedules(string googleId)
        {
            return _context.Schedules
            .Where(s => s.GoogleId == googleId)
            .ToList();
        }

        public void AddSchedule(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
        }

        public void DeleteSchedule(Schedule schedule)
        {
            _context.Schedules.Remove(schedule);
        }

        public bool Save()
        {
           return (_context.SaveChanges() >= 0);
        }
    }
}
