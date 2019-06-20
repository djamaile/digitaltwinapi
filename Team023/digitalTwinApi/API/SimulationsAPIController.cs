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
    [Route("api/[controller]")]
    public class SimulationsAPIController : Controller
    {

        private readonly DigitalTwinContext _context;

        public SimulationsAPIController(DigitalTwinContext context)
        {
            _context = context;

            if (_context.Simulations.Count() == 0)
            {
                _context.Simulations.Add(new Simulation { Key = "1", ProductieStraat = "1", KlantId = "1", Bovenwaarde = 1, Onderwaarde = 1 });
                _context.SaveChanges();
            }
        }

        // GET: api/SimulationsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Simulation>>> GetSimulationsItems()
        {
            return await _context.Simulations.ToListAsync();
        }

        // GET: api/SimulationsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Simulation>>> GetSimulationItemByID(string id)
        {
            var simulation = await _context.Simulations.Where(e => e.KlantId.Equals(id)).ToListAsync();
            
            if (simulation == null)
            {
                return NotFound();
            }

            return simulation;
        }





    }
}
