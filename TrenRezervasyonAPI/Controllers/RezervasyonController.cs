using Microsoft.AspNetCore.Mvc;
using TrenRezervasyonAPI.Models;
using TrenRezervasyonAPI.Services;


namespace TrenRezervasyonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RezervasyonController : ControllerBase
    {
        private readonly RezervasyonService _rezervasyonService;
        
        public RezervasyonController(RezervasyonService rezervasyonService)
        {
            _rezervasyonService = rezervasyonService;
        }
        
        [HttpPost]
        public ActionResult<RezervasyonResponse> RezervasyonYap([FromBody] RezervasyonRequest request)
        {
            try
            {
                // Temel validasyonlar
                if (request == null)
                {
                    return BadRequest("Geçersiz istek");
                }
                
                if (request.RezervasyonYapilacakKisiSayisi <= 0)
                {
                    return BadRequest("Rezervasyon yapılacak kişi sayısı 0'dan büyük olmalıdır");
                }
                
                if (request.Tren?.Vagonlar == null || !request.Tren.Vagonlar.Any())
                {
                    return BadRequest("Tren bilgileri eksik veya vagon bulunamadı");
                }
                
                var response = _rezervasyonService.RezervasyonYap(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}