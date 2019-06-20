using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace digitalTwinApi.Models
{
    public class Sensor
    {
        [Key]
        public string HardwareId { get; set; }
        
        public string MachineNaam { get; set; }
        
        public string DataType { get; set; }
        
        public IEnumerable<SensorData> SensorDatas { get; set; }
    }
}