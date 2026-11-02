
using KonyvtariKolcsonzes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<KonyvtarContext>(
    options => options.UseSqlite(
        "Data Source=konyvtar.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<KonyvtarContext>();

    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
