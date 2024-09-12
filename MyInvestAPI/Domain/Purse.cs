using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class Purse
    {
        [Key]
        public int Purse_Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public ICollection<Active> Actives { get; set; } = new List<Active>();

        public Purse()
        { }

        public Purse(string name, string description, int user_id)
        {
            this.Name = name;
            this.Description = description;
            this.CreatedAt = DateTime.UtcNow;
            this.LastUpdatedAt = DateTime.UtcNow;
            this.User_Id = user_id;
            this.Actives = new List<Active>();
        }
    }
}
