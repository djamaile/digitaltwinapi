using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digitalTwinApi.Models;
using digitalTwinApi.Services;
using Microsoft.AspNetCore.SignalR;

namespace digitalTwinApi.Hubs
{
    public class SensorDataHub : Hub
    {
        private DigitalTwinContext _context;
        
        public SensorDataHub(DigitalTwinContext context)
        {
            _context = context;
        }
        
        public async Task CheckStatus()
        {
            await Clients.Caller.SendAsync("Status", new { Online = true });
        }
        
        public async Task SubscribeSensorData(List<Sensor> sensors, Simulation simulation)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            simulation.Datum = dateTime.ToString("dd/MM/yyyy");
            RandomDataGenerator rng = new RandomDataGenerator(Clients.Caller, _context, simulation);

            Console.WriteLine("HALLLO::::" + sensors.Count);
            
            await rng.GenerateSensorsData(10, sensors);
        }
    }
}