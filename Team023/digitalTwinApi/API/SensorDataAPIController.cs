using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digitalTwinApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace digitalTwinApi.API
{

    [Route("api/SensorDataAPI")]
    [ApiController]
    public class SensorDataAPIController : Controller
    {
        private readonly DigitalTwinContext _context;

        public SensorDataAPIController(DigitalTwinContext context)
        {
            _context = context;

            if (_context.SensorDatas.Count() == 0)
            {
                _context.SensorDatas.Add(new SensorData { TimeStamp = DateTime.Now, Sensor = _context.Sensors.First(), Simulation = _context.Simulations.First(), Value = "Opvuller" });
                _context.SaveChanges();
            }
        }

        // GET: api/SensoDatarAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorData>>> GetSensorDataItems()
        {
            return await _context.SensorDatas.ToListAsync();
        }

        // GET: api/SensorDataAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorData>> GetSensorDataItemByID(string id)
        {
            var sensor = await _context.SensorDatas
                .Include(e => e.Sensor.SensorDatas)
                .Include(e => e.Simulation)
                .Where(e => e.Simulation.Key.Equals(id))
                .FirstOrDefaultAsync(e => e.Simulation != null);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }


    }

}
