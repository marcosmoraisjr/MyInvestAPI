using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [Required]
        [StringLength(80)]
        [MinLength(8)]
        [JsonIgnore]
        public string? Password { get; set; }

        [Required]
        [StringLength(80)]
        public string? Email { get; set; }

        [Required]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public ICollection<Purse>? Purses { get; set; }

        public User()
        { }

        public User(string name, string password, string email, string phone)
        {
            this.Name = name;
            this.Password = password;
            this.Email = email;
            this.Phone = phone;
            this.CreatedAt = DateTime.UtcNow;
            this.LastUpdatedAt = DateTime.UtcNow;
            this.Purses = new List<Purse>();
        }
    }
}
