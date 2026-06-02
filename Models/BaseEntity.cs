namespace KampusRota.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime OlusturulmaTarihi { get; set; }
        public int OlusturanKullaniciId { get; set; }

        public DateTime? GuncellenmeTarihi { get; set; }
        public int? GuncelleyenKullaniciId { get; set; }

        public bool AktifMi { get; set; } = true;

        public bool SilindiMi { get; set; } = false;
        public DateTime? SilinmeTarihi { get; set; }
        public int? SilenKullaniciId { get; set; }
    }
}
