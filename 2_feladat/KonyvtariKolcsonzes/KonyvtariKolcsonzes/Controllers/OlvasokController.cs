
using KonyvtariKolcsonzes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonyvtariKolcsonzes.Controllers
{
    [ApiController]
    [Route("api/olvasok")]
    public class OlvasokController : ControllerBase
    {
        private readonly KonyvtarContext _context;

        public OlvasokController(KonyvtarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var olvasok = await _context.Olvasok
                .AsNoTracking()
                .Select(o => new
                {
                    o.Id,
                    o.Nev,
                    o.Email
                })
                .ToListAsync();

            return Ok(olvasok);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var olvaso = await _context.Olvasok
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.Nev,
                    o.Email
                })
                .FirstOrDefaultAsync();

            if (olvaso == null)
                return NotFound();

            return Ok(olvaso);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Olvaso olvaso)
        {
            if (string.IsNullOrWhiteSpace(olvaso.Nev) ||
                string.IsNullOrWhiteSpace(olvaso.Email))
                return BadRequest("A név és az e-mail kötelező.");

            olvaso.Id = 0;
            olvaso.Konyvek = new List<Konyv>();

            _context.Olvasok.Add(olvaso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = olvaso.Id },
                new { olvaso.Id, olvaso.Nev, olvaso.Email });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] Olvaso adat)
        {
            if (string.IsNullOrWhiteSpace(adat.Nev) ||
                string.IsNullOrWhiteSpace(adat.Email))
                return BadRequest("A név és az e-mail kötelező.");

            var olvaso = await _context.Olvasok.FindAsync(id);

            if (olvaso == null)
                return NotFound();

            olvaso.Nev = adat.Nev;
            olvaso.Email = adat.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var olvaso = await _context.Olvasok.FindAsync(id);

            if (olvaso == null)
                return NotFound();

            _context.Olvasok.Remove(olvaso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id:int}/konyvek")]
        public async Task<IActionResult> GetKonyvek(int id)
        {
            if (!await _context.Olvasok.AnyAsync(o => o.Id == id))
                return NotFound();

            var konyvek = await _context.Konyvek
                .AsNoTracking()
                .Where(k => k.OlvasoId == id)
                .ToListAsync();

            return Ok(konyvek);
        }

        [HttpGet("kolcsonzok")]
        public async Task<IActionResult> GetKolcsonzok()
        {
            var olvasok = await _context.Olvasok
                .AsNoTracking()
                .Where(o => o.Konyvek.Any())
                .Select(o => new
                {
                    o.Id,
                    o.Nev,
                    o.Email
                })
                .ToListAsync();

            return Ok(olvasok);
        }
    }
}
