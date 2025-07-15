namespace TrenRezervasyonAPI.Models
{
    public class VagonModel
    {
        public string Ad { get; set; } = string.Empty;
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }

        // Vagonun müsait koltuk sayısını hesaplayan property
        public int MusaitKoltukSayisi => Kapasite - DoluKoltukAdet;

        // %70 doluluk kontrolü için müsait kapasite
        public int MaksimumRezervasyonKapasitesi => (int)(Kapasite * 0.7) - DoluKoltukAdet;
    }
}