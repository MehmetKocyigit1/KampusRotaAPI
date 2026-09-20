using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KampusRota.Application.DTOs;
using KampusRota.Application.Services;
using KampusRota.Infrastructure.Persistence;

namespace KampusRota.Infrastructure.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly AppDbContext _context;

        public UniversityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UniversityDto>> GetAllUniversitiesAsync()
        {
            return await _context.Universities
                .AsNoTracking()
                .Where(u => u.AktifMi && !u.SilindiMi)
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    City = u.City,
                    Latitude = u.Latitude,
                    Longitude = u.Longitude,
                    DefaultZoom = u.DefaultZoom,
                    EmailDomain = u.EmailDomain,
                    IletisimEmail = u.IletisimEmail,
                    LocationCount = u.Locations.Count(l => l.AktifMi && !l.SilindiMi)
                })
                .ToListAsync();
        }

        public async Task<UniversityDto?> GetUniversityByIdAsync(int id)
        {
            return await _context.Universities
                .AsNoTracking()
                .Where(u => u.Id == id && u.AktifMi && !u.SilindiMi)
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    City = u.City,
                    Latitude = u.Latitude,
                    Longitude = u.Longitude,
                    DefaultZoom = u.DefaultZoom,
                    EmailDomain = u.EmailDomain,
                    IletisimEmail = u.IletisimEmail,
                    LocationCount = u.Locations.Count(l => l.AktifMi && !l.SilindiMi),
                    Locations = u.Locations
                        .Where(l => l.AktifMi && !l.SilindiMi)
                        .Select(l => new CampusLocationDto
                        {
                            Id = l.Id,
                            UniversityId = l.UniversityId,
                            LocationKey = l.LocationKey,
                            Title = l.Title,
                            Latitude = l.Latitude,
                            Longitude = l.Longitude,
                            Category = l.Category.ToString(),
                            Description = l.Description
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CampusLocationDto>> GetCampusLocationsAsync(int universityId)
        {
            return await _context.CampusLocations
                .AsNoTracking()
                .Where(l => l.UniversityId == universityId && l.AktifMi && !l.SilindiMi)
                .Select(l => new CampusLocationDto
                {
                    Id = l.Id,
                    UniversityId = l.UniversityId,
                    LocationKey = l.LocationKey,
                    Title = l.Title,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Category = l.Category.ToString(),
                    Description = l.Description
                })
                .ToListAsync();
        }

        public async Task<UniversityDto?> GetUniversityByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return null;

            var domain = email.Split('@').Last().ToLowerInvariant();

            var university = await _context.Universities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.AktifMi && !u.SilindiMi && u.EmailDomain.ToLower() == domain);

            if (university == null)
                return null;

            return await GetUniversityByIdAsync(university.Id);
        }
    }
}
