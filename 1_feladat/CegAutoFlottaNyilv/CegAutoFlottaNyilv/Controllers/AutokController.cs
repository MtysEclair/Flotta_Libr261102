using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CegAutoFlottaNyilv.Models;

[Route("api/[controller]")]
[ApiController]
public class AutokController : ControllerBase
{
    private readonly CegAutoFlottaNyilvContext _context;
    public AutokController(CegAutoFlottaNyilvContext context)
    {
        _context = context;
    }

    // GET: api/Auto
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auto>>> GetAuto()
    {
        return await _context.Auto.ToListAsync();
    }

    // GET: api/Auto/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Auto>> GetAuto(int id)
    {
        var auto = await _context.Auto.FindAsync(id);

        if (auto == null)
        {
            return NotFound();
        }

        return auto;
    }

    // PUT: api/Auto/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuto(int? id, Auto auto)
    {
        if (id != auto.Id)
        {
            return BadRequest();
        }

        _context.Entry(auto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AutoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Auto
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Auto>> PostAuto(Auto auto)
    {
        _context.Auto.Add(auto);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuto", new { id = auto.Id }, auto);
    }

    // DELETE: api/Auto/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuto(int? id)
    {
        var auto = await _context.Auto.FindAsync(id);
        if (auto == null)
        {
            return NotFound();
        }

        _context.Auto.Remove(auto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AutoExists(int? id)
    {
        return _context.Auto.Any(e => e.Id == id);
    }
}
