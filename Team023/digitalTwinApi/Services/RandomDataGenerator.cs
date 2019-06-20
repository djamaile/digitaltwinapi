using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using digitalTwinApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;

namespace digitalTwinApi.Services
{
    public class RandomDataGenerator
    {
        private Random rnd = new Random();
        
        private DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CustomTelemetryMessage));
        private IConfigurationSection settings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()
            .GetSection("Settings");

        private readonly DeviceClient deviceClient;

        private IClientProxy _client;
        private DigitalTwinContext _context;
        private Simulation _simulation;
            
        public RandomDataGenerator()
        {
            deviceClient = DeviceClient.CreateFromConnectionString(settings["DeviceConnectionString"]);
        }
        
        public RandomDataGenerator(IClientProxy client, DigitalTwinContext context, Simulation simulation)
        {
            _context = context;
            _client = client;
            
            _context.Add(simulation);
            _context.SaveChanges();
            
            _simulation = simulation;
            
            deviceClient = DeviceClient.CreateFromConnectionString(settings["DeviceConnectionString"]);
        }

        
        private async Task<Func<string>> CreateGetRandomSensorReading(string sensorDataType, int iteration, int onderwaarde, int bovenwaarde, string datum)
        {
            switch (sensorDataType)
            {
                default:
                    throw new Exception($"Unsupported configuration: SensorDataType, '{sensorDataType}'. Please check your appsettings.json.");
                case "Motion":
                    if (iteration % 6 < 3)
                        return () => "false";
                    else
                        return () => "true";

                case "Temperature":
                    return () => rnd.Next(onderwaarde, bovenwaarde).ToString(CultureInfo.InvariantCulture);
                case "CarbonDioxide":
                    if (iteration % 6 < 3)
                        return () => rnd.Next(800, 1000).ToString(CultureInfo.InvariantCulture);
                    else
                        return () => rnd.Next(1000, 1100).ToString(CultureInfo.InvariantCulture);
            }
        }

        public async Task GenerateSensorsData(int interval, List<Sensor> sensoren)
        {            
            var delayPerMessageSend = interval;
            var countOfSendsPerIteration = sensoren.Count;
            var maxSecondsToRun = 15 * 60;
            var maxIterations = maxSecondsToRun / countOfSendsPerIteration / delayPerMessageSend;
            var curIteration = 0;
            
            do {
                foreach (var sensor in sensoren)
                {

                    var dbSensor = _context.Sensors.FirstOrDefault(s => s.HardwareId == sensor.HardwareId);

                    if (dbSensor == null)
                    {
                        _context.Sensors.Add(sensor);
                    }                   
                    
                    await GenerateSensorData(sensor, curIteration);
                    
                    await _context.SaveChangesAsync();
                }
            } while (++curIteration < maxIterations);
        }

        private async Task GenerateSensorData(Sensor sensor, int curIteration)
        {
            var getRandomSensorReading = await CreateGetRandomSensorReading(sensor.DataType, curIteration, _simulation.Onderwaarde, _simulation.Bovenwaarde, _simulation.Datum);
 
            var sensorData = new SensorData()
            {
                Sensor = sensor,
                Value = getRandomSensorReading(),
                TimeStamp = DateTime.Now,
                Simulation = _simulation
            };

            _context.SensorDatas.Add(sensorData);
            
            await _client.SendAsync("SensorData", new { Id = _simulation.ProductieStraat + sensor.MachineNaam ,  sensorData.Value, Q = true, sensorData.TimeStamp });
        }
    }
}