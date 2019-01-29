using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scheduler.Models;
using scheduler.Services;

namespace scheduler.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private ISchdulesRepository _scheduleRepository;
        private ILogger<ScheduleController> _logger;

        public ScheduleController(ISchdulesRepository schdulesRepository, ILogger<ScheduleController> logger)
        {
            _scheduleRepository = schdulesRepository;
            _logger = logger;
        }

        // GET api/schedule
        [HttpGet]
        public IActionResult Get([FromQuery]string googleId)
        {
            try
            {
                var schedules = _scheduleRepository.GetSchedules(googleId);

                if (schedules == null)
                {
                    _logger.LogInformation("Schedule list was not fount at: api/schedule");
                    return NotFound();
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Exception at: api/schedule", ex);
                return StatusCode(500);
            }
        }

        // GET api/schedules/5
        [HttpGet("{id}", Name = "schedules")]
        public IActionResult Get(int id)
        {
            var schedule = _scheduleRepository.GetSchedule(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // POST api/schedule
        [HttpPost]
        public IActionResult Post([FromBody]ScheduleViewModel scheduleVM)
        {
            if (ModelState.IsValid)
            {
                var saveSchedule = Mapper.Map<Schedule>(scheduleVM);
                _scheduleRepository.AddSchedule(saveSchedule);
                if (!_scheduleRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling yout request!");
                }

                return CreatedAtRoute("schedules", new { saveSchedule.Id }, saveSchedule);
            }

            return BadRequest(ModelState);
        }


        // PUT api/schedule/5
        [HttpPut]
        public IActionResult Put([FromBody]Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                var scheduleToUpdate = _scheduleRepository.GetSchedule(schedule.Id);

                if (scheduleToUpdate == null)
                {
                    return NotFound();
                }

                Mapper.Map(schedule, scheduleToUpdate);

                if (!_scheduleRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling yout request!");
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/schedule/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] GoogleIdString Token)
        {
            var scheduleToDelete = _scheduleRepository.GetSchedule(id);

            if (scheduleToDelete == null || Token.GoogleId != scheduleToDelete.GoogleId)
            {
                return NotFound();
            }

            _scheduleRepository.DeleteSchedule(scheduleToDelete);

            if (!_scheduleRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling yout request!");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}