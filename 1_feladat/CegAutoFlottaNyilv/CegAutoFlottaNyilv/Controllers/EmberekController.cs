using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CegAutoFlottaNyilv.Models;

[Route("api/[controller]")]
[ApiController]
public class EmberekController : ControllerBase
{
    private readonly CegAutoFlottaNyilvContext _context;
    public EmberekController(CegAutoFlottaNyilvContext context)
    {
        _context = context;
    }

    // GET: api/Ember
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ember>>> GetEmber()
    {
        return await _context.Ember.ToListAsync();
    }

    // GET: api/Ember/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ember>> GetEmber(int id)
    {
        var ember = await _context.Ember.FindAsync(id);

        if (ember == null)
        {
            return NotFound();
        }

        return ember;
    }
    [HttpGet("{id}/autok")]
    public async Task<ActionResult<Ember>> GetWithCars(int id)
    {
        Ember? ember = await _context.Ember
            .Include(e => e.Autok)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ember == null)
            return NotFound();

        return ember;
    }

    // PUT: api/Ember/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmber(int? id, Ember ember)
    {
        if (id != ember.Id)
        {
            return BadRequest();
        }

        _context.Entry(ember).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmberExists(id))
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

    // POST: api/Ember
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Ember>> PostEmber(Ember ember)
    {
        _context.Ember.Add(ember);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmber", new { id = ember.Id }, ember);
    }

    // DELETE: api/Ember/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmber(int? id)
    {
        var ember = await _context.Ember.FindAsync(id);
        if (ember == null)
        {
            return NotFound();
        }

        _context.Ember.Remove(ember);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EmberExists(int? id)
    {
        return _context.Ember.Any(e => e.Id == id);
    }
}
