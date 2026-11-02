
using KonyvtariKolcsonzes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KonyvtariKolcsonzes.Controllers
{
    [ApiController]
    [Route("api/konyvek")]
    public class KonyvekController : ControllerBase
    {
        private readonly KonyvtarContext _context;

        public KonyvekController(KonyvtarContext context)
        {
            _context = context;
        }

        public class KonyvAdat
        {
            [Required]
            [MaxLength(150)]
            public string Cim { get; set; } = "";

            [Required]
            [MaxLength(100)]
            public string Szerzo { get; set; } = "";

            [Required]
            [MaxLength(20)]
            public string ISBN { get; set; } = "";

            public int KiadasEve { get; set; }

            public int? OlvasoId { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var konyvek = await _context.Konyvek
                .AsNoTracking()
                .Select(k => new
                {
                    k.Id,
                    k.Cim,
                    k.Szerzo,
                    k.ISBN,
                    k.KiadasEve,
                    k.OlvasoId
                })
                .ToListAsync();

            return Ok(konyvek);
        }

        [HttpGet("szabad")]
        public async Task<IActionResult> GetSzabad()
        {
            var konyvek = await _context.Konyvek
                .AsNoTracking()
                .Where(k => k.OlvasoId == null)
                .ToListAsync();

            return Ok(konyvek);
        }

        [HttpGet("kereses")]
        public async Task<IActionResult> Kereses(
            [FromQuery] string? szoveg)
        {
            if (string.IsNullOrWhiteSpace(szoveg))
                return BadRequest(
                    "A keresési szöveg kötelező.");

            var keresett = szoveg.Trim();

            var konyvek = await _context.Konyvek
                .AsNoTracking()
                .Where(k =>
                    k.Cim.Contains(keresett) ||
                    k.Szerzo.Contains(keresett))
                .ToListAsync();

            return Ok(konyvek);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var konyv = await _context.Konyvek
                .AsNoTracking()
                .Where(k => k.Id == id)
                .Select(k => new
                {
                    k.Id,
                    k.Cim,
                    k.Szerzo,
                    k.ISBN,
                    k.KiadasEve,
                    k.OlvasoId
                })
                .FirstOrDefaultAsync();

            if (konyv == null)
                return NotFound();

            return Ok(konyv);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] KonyvAdat adat)
        {
            if (!Ervenyes(adat))
                return BadRequest(
                    "A cím, szerző és ISBN kötelező.");

            adat.ISBN = adat.ISBN.Trim();

            if (await _context.Konyvek
                .AnyAsync(k => k.ISBN == adat.ISBN))
                return BadRequest(
                    "Ez az ISBN már szerepel.");

            if (adat.OlvasoId != null &&
                !await _context.Olvasok
                    .AnyAsync(o => o.Id == adat.OlvasoId))
                return BadRequest(
                    "A megadott olvasó nem létezik.");

            var konyv = new Konyv
            {
                Cim = adat.Cim.Trim(),
                Szerzo = adat.Szerzo.Trim(),
                ISBN = adat.ISBN,
                KiadasEve = adat.KiadasEve,
                OlvasoId = adat.OlvasoId
            };

            _context.Konyvek.Add(konyv);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "A könyv nem menthető. Ellenőrizd az ISBN-t és az olvasót.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = konyv.Id },
                new
                {
                    konyv.Id,
                    konyv.Cim,
                    konyv.Szerzo,
                    konyv.ISBN,
                    konyv.KiadasEve,
                    konyv.OlvasoId
                });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] KonyvAdat adat)
        {
            var konyv = await _context.Konyvek
                .FindAsync(id);

            if (konyv == null)
                return NotFound();

            if (!Ervenyes(adat))
                return BadRequest(
                    "A cím, szerző és ISBN kötelező.");

            adat.ISBN = adat.ISBN.Trim();

            if (await _context.Konyvek.AnyAsync(k =>
                k.ISBN == adat.ISBN && k.Id != id))
                return BadRequest(
                    "Ez az ISBN már szerepel.");

            if (adat.OlvasoId != null &&
                !await _context.Olvasok
                    .AnyAsync(o => o.Id == adat.OlvasoId))
                return BadRequest(
                    "A megadott olvasó nem létezik.");

            if (konyv.OlvasoId != null &&
                adat.OlvasoId != null &&
                konyv.OlvasoId != adat.OlvasoId)
            {
                return BadRequest(
                    "A könyv már ki van kölcsönözve. Előbb vissza kell hozni.");
            }

            konyv.Cim = adat.Cim.Trim();
            konyv.Szerzo = adat.Szerzo.Trim();
            konyv.ISBN = adat.ISBN;
            konyv.KiadasEve = adat.KiadasEve;
            konyv.OlvasoId = adat.OlvasoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "A módosítás nem menthető. Ellenőrizd az adatokat.");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var konyv = await _context.Konyvek
                .FindAsync(id);

            if (konyv == null)
                return NotFound();

            _context.Konyvek.Remove(konyv);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static bool Ervenyes(KonyvAdat adat)
        {
            return
                !string.IsNullOrWhiteSpace(adat.Cim) &&
                !string.IsNullOrWhiteSpace(adat.Szerzo) &&
                !string.IsNullOrWhiteSpace(adat.ISBN);
        }
    }
}
