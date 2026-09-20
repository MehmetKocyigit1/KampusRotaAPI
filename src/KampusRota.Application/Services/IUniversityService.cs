using System.Collections.Generic;
using System.Threading.Tasks;
using KampusRota.Application.DTOs;

namespace KampusRota.Application.Services
{
    public interface IUniversityService
    {
        Task<List<UniversityDto>> GetAllUniversitiesAsync();
        Task<UniversityDto?> GetUniversityByIdAsync(int id);
        Task<List<CampusLocationDto>> GetCampusLocationsAsync(int universityId);
        Task<UniversityDto?> GetUniversityByEmailAsync(string email);
    }
}
