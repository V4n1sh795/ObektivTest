using Microsoft.EntityFrameworkCore;
using DBContext;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);
// Configs



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=/data/app.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));


// MainApp
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
// Endpoints
app.MapGet("/", async (AppDbContext db) => Results.Ok(await db.Flats.ToListAsync()));

app.MapGet("/api/prices", Service.Api.Prices);

app.MapPost("/api/flat", Service.Api.Flat);

app.MapPatch("/api/patch_price", Service.Api.PatchPrice);

app.Run();
