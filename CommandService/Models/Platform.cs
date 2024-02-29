using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalId { get; set; } // reffers to the of the platform in the platService db

        [Required]
        public string Name { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}