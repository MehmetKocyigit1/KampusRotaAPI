using System.Collections.Generic;
using KampusRota.Domain.Common;

namespace KampusRota.Domain.Entities
{
    public class University : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int DefaultZoom { get; set; } = 14;
        public string EmailDomain { get; set; } = string.Empty;
        public string? IletisimEmail { get; set; }

        public virtual ICollection<CampusLocation> Locations { get; set; } = new List<CampusLocation>();
        public virtual ICollection<Yolculuk> Yolculuklar { get; set; } = new List<Yolculuk>();
        public virtual ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
    }
}
