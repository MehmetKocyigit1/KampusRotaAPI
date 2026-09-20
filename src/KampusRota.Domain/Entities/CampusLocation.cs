using KampusRota.Domain.Common;
using KampusRota.Domain.Enums;

namespace KampusRota.Domain.Entities
{
    public class CampusLocation : BaseEntity
    {
        public int UniversityId { get; set; }
        public virtual University? University { get; set; }

        public string LocationKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public LocationCategory Category { get; set; } = LocationCategory.Kampus;
        public string? Description { get; set; }
    }
}
