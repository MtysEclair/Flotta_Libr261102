
using KonyvtariKolcsonzes.Models;
using Microsoft.EntityFrameworkCore;

namespace KonyvtariKolcsonzes
{
    public class KonyvtarContext : DbContext
    {
        public KonyvtarContext(
            DbContextOptions<KonyvtarContext> options)
            : base(options)
        {
        }

        public DbSet<Olvaso> Olvasok { get; set; }
            = null!;

        public DbSet<Konyv> Konyvek { get; set; }
            = null!;

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Olvaso>()
                .HasMany(o => o.Konyvek)
                .WithOne(k => k.Olvaso)
                .HasForeignKey(k => k.OlvasoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Konyv>()
                .HasIndex(k => k.ISBN)
                .IsUnique();
        }
    }
}
