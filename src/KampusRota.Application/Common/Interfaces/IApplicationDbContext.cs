using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KampusRota.Domain.Entities;

namespace KampusRota.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<University> Universities { get; set; }
        DbSet<CampusLocation> CampusLocations { get; set; }
        DbSet<Kullanici> Kullanicilar { get; set; }
        DbSet<Yolculuk> Yolculuklar { get; set; }
        DbSet<YolculukTalebi> YolculukTalepleri { get; set; }
        DbSet<YolculukYorumu> YolculukYorumlari { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
