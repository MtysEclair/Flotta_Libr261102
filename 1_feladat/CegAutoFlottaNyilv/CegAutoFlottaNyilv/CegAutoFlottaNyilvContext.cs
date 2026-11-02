using Microsoft.EntityFrameworkCore;

public class CegAutoFlottaNyilvContext(DbContextOptions<CegAutoFlottaNyilvContext> options) : DbContext(options)
{
    public DbSet<CegAutoFlottaNyilv.Models.Ember> Ember { get; set; } = default!;
    public DbSet<CegAutoFlottaNyilv.Models.Auto> Auto { get; set; } = default!;
}
