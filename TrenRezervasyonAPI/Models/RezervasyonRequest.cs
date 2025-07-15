using TrenRezervasyonAPI.Models;

namespace TrenRezervasyonAPI
{
    public class RezervasyonRequest
    {
        public TrenModel Tren { get; set; } = new TrenModel();
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlesitirilebilir { get; set; }
    }
}