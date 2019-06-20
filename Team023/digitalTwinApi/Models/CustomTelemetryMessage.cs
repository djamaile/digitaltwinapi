using System;
using System.Runtime.Serialization;

namespace digitalTwinApi.Models
{
    [DataContract(Name="CustomTelemetryMessage")]
    public class CustomTelemetryMessage
    {
        [DataMember(Name="SensorValue")]
        public string SensorValue { get; set; }
    }
}