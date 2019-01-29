using scheduler.Models;
using System.Collections.Generic;

namespace scheduler.Services
{
    public interface ISchdulesRepository
    {
        IEnumerable<Schedule> GetSchedules(string googleId);
        Schedule GetSchedule(int id);
        void AddSchedule(Schedule schedule);
        void DeleteSchedule(Schedule schedule);
        bool Save();
    }
}
