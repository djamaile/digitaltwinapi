
using System.ComponentModel.DataAnnotations;

namespace digitalTwinApi.Models
{
    public class Simulation
    {
        [Key]
        public string Key { get; set; }
        
        public string ProductieStraat { get; set; }
                
        public string KlantId { get; set; }
        
        [Required]
        public int Bovenwaarde { get; set; }
        
        [Required]
        public int Onderwaarde { get; set; }
        
        [Required]
        public string Datum { get; set; }

    }
}