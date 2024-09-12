using MyInvestAPI.Domain;

namespace MyInvestAPI.ViewModels
{
    public class UpdateActiveViewModel
    { 
        public float DyDesiredPercentage { get; set; }

        public Active UpdateActive(Active active)
        {
            active.LastUpdatedAt = DateTime.UtcNow;
            active.DYDesiredPercentage = DyDesiredPercentage;
            return active;
        }
    }

}
