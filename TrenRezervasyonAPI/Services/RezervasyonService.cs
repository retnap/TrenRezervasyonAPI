using TrenRezervasyonAPI.Models;

namespace TrenRezervasyonAPI.Services
{
    public class RezervasyonService
    {
        public RezervasyonResponse RezervasyonYap(RezervasyonRequest request)
        {
            var response = new RezervasyonResponse();

            // Eğer farklı vagonlara yerleştirilebilirse
            if (request.KisilerFarkliVagonlaraYerlesitirilebilir)
            {
                response = FarkliVagonlaraYerlestir(request);
            }

            else
            {
                response = AyniVagonaYerlestir(request);
            }

            return response;
        }

        private RezervasyonResponse FarkliVagonlaraYerlestir(RezervasyonRequest request)
        {
            var response = new RezervasyonResponse();
            var kalanKisiSayisi = request.RezervasyonYapilacakKisiSayisi;

            // Vagonları müsait kapasiteye göre sırala (büyükten küçüğe)
            var musaitVagonlar = request.Tren.Vagonlar
                .Where(v => v.MaksimumRezervasyonKapasitesi > 0)
                .OrderByDescending(v => v.MaksimumRezervasyonKapasitesi)
                .ToList();

            // Her vagona sırayla yerlestir
            foreach (var vagon in musaitVagonlar)
            {
                if (kalanKisiSayisi <= 0) break;

                var yerlestirilecekKisiSayisi = Math.Min(kalanKisiSayisi, vagon.MaksimumRezervasyonKapasitesi);

                if (yerlestirilecekKisiSayisi > 0)
                {
                    response.YerlesimAyrinti.Add(new YerlesimAyrinti
                    {
                        VagonAdi = vagon.Ad,
                        KisiSayisi = yerlestirilecekKisiSayisi
                    });

                    kalanKisiSayisi -= yerlestirilecekKisiSayisi;
                }
            }

            // Tum kisiler yerlestirilebildi mi ?
            response.RezervasyonYapilabilir = kalanKisiSayisi == 0;

            // Eğer rezervasyon yapılamıyorsa yerleşim detayını temizle
            if (!response.RezervasyonYapilabilir)
            {
                response.YerlesimAyrinti.Clear();
            }

            return response;
        }

        private RezervasyonResponse AyniVagonaYerlestir(RezervasyonRequest request)
        {
            var response = new RezervasyonResponse();

            // Tek bir vagonda tüm kişileri yerleştirebilecek vagon ara
            var uygunVagon = request.Tren.Vagonlar
                .FirstOrDefault(v => v.MaksimumRezervasyonKapasitesi >= request.RezervasyonYapilacakKisiSayisi);

            if (uygunVagon != null)
            {
                response.RezervasyonYapilabilir = true;
                response.YerlesimAyrinti.Add(new YerlesimAyrinti
                {
                    VagonAdi = uygunVagon.Ad,
                    KisiSayisi = request.RezervasyonYapilacakKisiSayisi
                });
            }

            else
            {
                response.RezervasyonYapilabilir = false;
                response.YerlesimAyrinti.Clear();
            }

            return response;
        }
    }
}