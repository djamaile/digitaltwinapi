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
    [Route("api/SensorAPI")]
    [ApiController]
    public class SensorAPIController : Controller
    {
        private readonly DigitalTwinContext _context;

        public SensorAPIController(DigitalTwinContext context)
        {
            _context = context;

            if (_context.Sensors.Count() == 0)
            {
                _context.Sensors.Add(new Sensor { HardwareId = "1", MachineNaam = "Eerste", DataType = "1", SensorDatas = null });
                _context.SaveChanges();
            }
        }

        // GET: api/SensorAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorItems()
        {
            return await _context.Sensors.ToListAsync();
        }

        // GET: api/SensorAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensorItemByID(long id)
        {
            var sensor = await _context.Sensors.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }


    }
}
