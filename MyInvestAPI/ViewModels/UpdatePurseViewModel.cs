using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyInvestAPI.ViewModels
{
    public class UpdatePurseViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; } 

        public Purse UpdatePurse(Purse purse)
        {
            purse.Name = Name;
            purse.Description = Description;
            return purse;
        }
    }
}
