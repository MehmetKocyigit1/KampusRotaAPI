using System.Collections.Generic;

namespace KampusRota.Application.DTOs
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int DefaultZoom { get; set; } = 14;
        public string EmailDomain { get; set; } = string.Empty;
        public string? IletisimEmail { get; set; }
        public int LocationCount { get; set; }
        public List<CampusLocationDto> Locations { get; set; } = new List<CampusLocationDto>();
    }

    public class CampusLocationDto
    {
        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string LocationKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; } = "Kampus";
        public string? Description { get; set; }
    }
}
