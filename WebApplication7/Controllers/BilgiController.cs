using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.Model;
using NetTopologySuite.Geometries;
using WebApplication7.DtoS;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BilgiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BilgiController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("kaydet")]
        public async Task<IActionResult> Kaydet(BilgiEkleDto dto)
        {
            var veri = new bilgi
            {
                Sehir = dto.Sehir,
    
                Konum = new Point(dto.Boylam, dto.Enlem) { SRID = 4326 }
            };

            _db.bilgiler.Add(veri);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                mesaj = "Veri başarıyla kaydedildi!",
                kaydedilenVeri = new
                {
                    veri.Id,
                    veri.Sehir,
                    KonumMetni = veri.Konum.ToText()
                }
            });
        }

        [HttpGet("listele")]
        public async Task<IActionResult> Listele()
        {
            var liste = await _db.bilgiler
                .Select(b => new
                {
                    b.Id,
                    b.Sehir,
                    KonumMetni = b.Konum.ToText()
                })
                .ToListAsync();

            return Ok(liste);
        }
        [HttpPut("guncelle/{id}")]
        public async Task<IActionResult> Guncelle(int id, BilgiEkleDto dto) 
        {
            var bulunanVeri = await _db.bilgiler.FindAsync(id);
            if (bulunanVeri == null)
            {
                return NotFound(new { mesaj = $"{id} numaralı kayıt veritabanında bulunamadı." });
            }

            bulunanVeri.Sehir = dto.Sehir;
            bulunanVeri.Konum = new Point(dto.Boylam, dto.Enlem) { SRID = 4326 };

            _db.bilgiler.Update(bulunanVeri);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                mesaj = $"{id} numaralı kayıt başarıyla güncellendi!",
                guncellenenVeri = new
                {
                    bulunanVeri.Id,
                    bulunanVeri.Sehir,
                    KonumMetni = bulunanVeri.Konum.ToText()
                }
            });
        }


        [HttpDelete("sil/{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var bulunanVeri = await _db.bilgiler.FindAsync(id);
            if (bulunanVeri == null)
            {
                return NotFound(new { mesaj = $"{id} numaralı kayıt veritabanında bulunamadı." });
            }

            _db.bilgiler.Remove(bulunanVeri);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                mesaj = $"{id} numaralı kayıt başarıyla silindi!",
                silinenVeri = new
                {
                    bulunanVeri.Id,
                    bulunanVeri.Sehir,
                    KonumMetni = bulunanVeri.Konum.ToText()
                }
            });
        }
    }
}