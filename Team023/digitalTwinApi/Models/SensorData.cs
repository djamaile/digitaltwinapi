using System;
using System.ComponentModel.DataAnnotations;

namespace digitalTwinApi.Models
{
    public class SensorData
    {

        [Key]
        public DateTime TimeStamp { get; set; }

        [Key]
        public Sensor Sensor { get; set; }
        
        public Simulation Simulation { get; set; }

        public string Value { get; set; }

        
    }
}