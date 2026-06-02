namespace KampusRota.Models
{
    public class Yolculuk : BaseEntity
    {
        public int SurucuId { get; set; }
        public string KalkisNoktasi { get; set; } = string.Empty;
        public string VarisNoktasi { get; set; } = string.Empty;
        public DateTime KalkisZamani { get; set; }
        public int BosKoltukSayisi { get; set; }

        public decimal KisiBasiUcret { get; set; } = 0; 
        public string Aciklama { get; set; } = string.Empty;
        public string IletisimTelefonu { get; set; } = string.Empty;
        public bool SadeceKadinlarMi { get; set; } = false; 

        public virtual Kullanici? Surucu { get; set; }
    }
}
