using System;
using KampusRota.Domain.Common;

namespace KampusRota.Domain.Entities
{
    public class YolculukTalebi : BaseEntity
    {
        public int YolculukId { get; set; }
        public virtual Yolculuk? Yolculuk { get; set; }

        public int YolcuId { get; set; }
        public virtual Kullanici? Yolcu { get; set; }

        public string Durum { get; set; } = "Bekliyor"; // Bekliyor, Onaylandı, Reddedildi, İptal Edildi
        public string TalepMesaji { get; set; } = string.Empty;
        public string SurucuNotu { get; set; } = string.Empty;
        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? OnayTarihi { get; set; }
    }
}
