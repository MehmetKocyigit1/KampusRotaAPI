using System;
using KampusRota.Domain.Common;

namespace KampusRota.Domain.Entities
{
    public class YolculukYorumu : BaseEntity
    {
        public int YolculukId { get; set; }
        public virtual Yolculuk? Yolculuk { get; set; }

        public int YorumYapanKullaniciId { get; set; }
        public virtual Kullanici? YorumYapanKullanici { get; set; }

        public int PuanlananKullaniciId { get; set; }
        public virtual Kullanici? PuanlananKullanici { get; set; }

        public int Puan { get; set; }
        public string Yorum { get; set; } = string.Empty;
        public DateTime YorumTarihi { get; set; } = DateTime.UtcNow;
    }
}
