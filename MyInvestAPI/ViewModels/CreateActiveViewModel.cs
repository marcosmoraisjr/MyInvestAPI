using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class CreateActiveViewModel
    {
        public string? Type { get; set; }
        public string? Code { get; set; }
        public int Purse_Id { get; set; }

        public Active CreateActive(Purse purse)
        {
            Active active = new();
            active.Code = Code;
            active.type = this.Type;
            active.CreatedAt = DateTime.UtcNow;
            active.LastUpdatedAt = DateTime.UtcNow;
            active.Purses = new List<Purse>() { purse };
            return active;
        }
    }
    
}
