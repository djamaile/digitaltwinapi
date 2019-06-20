using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using digitalTwinApi.Models;
using digitalTwinApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Newtonsoft.Json;

namespace digitalTwinApi
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private static IConfigurationSection settings;
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Sensor> sensoren = new List<Sensor>();
            
            Sensor sensor1 = new Sensor();
            sensor1.DataType =  "Temperature";
            sensor1.HardwareId = "DJAMAILE_SENSOR_7";
            
            Sensor sensor2 = new Sensor();
            sensor2.DataType =  "Temperature";
            sensor2.HardwareId = "DJAMAILE_SENSOR_6";
            
            sensoren.Add(sensor1);
            sensoren.Add(sensor2);
            
            //RandomGenerator.SendEvent(deviceClient, 2, sensoren, 70, 100).Wait();
                
            RandomDataGenerator rdg = new RandomDataGenerator();
            rdg.GenerateSensorsData(2, sensoren);

            return Json(new { Success = true });
        }

        public async Task<IActionResult> Check()
        {
            return Json(new { Success = true });
        }
    }
}