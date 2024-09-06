using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyInvestAPI.ViewModels
{
    public class CreatePurseViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int User_Id { get; set; }

        public Purse CreatePurse()
        {
            return new Purse(name: Name, description: Description, user_id: User_Id);
        }
    }
}
