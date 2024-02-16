using System.ComponentModel.DataAnnotations;

namespace PlatService.DTOs
{
    public class PlatformCreateDto 
    {
        [Required] 
         public string name { get; set; }
         
        [Required]
        public string publisher { get; set; }
        
        [Required] 
        public string cost { get; set; }
    }
}