using System;
using System.Collections.Generic;
using KampusRota.Domain.Common;

namespace KampusRota.Domain.Entities
{
    public class Yolculuk : BaseEntity
    {
        public int SurucuId { get; set; }
        public virtual Kullanici? Surucu { get; set; }

        public int? UniversityId { get; set; }
        public virtual University? University { get; set; }

        public string KalkisNoktasi { get; set; } = string.Empty;
        public double? KalkisLatitude { get; set; }
        public double? KalkisLongitude { get; set; }

        public string VarisNoktasi { get; set; } = string.Empty;
        public double? VarisLatitude { get; set; }
        public double? VarisLongitude { get; set; }

        public DateTime KalkisZamani { get; set; }
        public int BosKoltukSayisi { get; set; }
        public decimal KisiBasiUcret { get; set; } = 0;
        public string Aciklama { get; set; } = string.Empty;
        public string IletisimTelefonu { get; set; } = string.Empty;
        public bool SadeceKadinlarMi { get; set; } = false;

        public virtual ICollection<YolculukTalebi> Talepler { get; set; } = new List<YolculukTalebi>();
        public virtual ICollection<YolculukYorumu> Yorumlar { get; set; } = new List<YolculukYorumu>();
    }
}
