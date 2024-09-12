using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class Active
    {
        [Key]
        public int Active_Id { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public float DYDesiredPercentage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Purse> Purses { get; set; } = new List<Purse>();
    } 
}
