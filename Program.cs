using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://mon-projet-angular-examen.web.app",
                "https://projet-angular-final.web.app"
              )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Clients.Any())
    {
        db.Clients.AddRange(
            new Client { Nom = "Admin", Prenom = "Super", Email = "admin@test.com", Password = "admin123", IsAdmin = true, CreatedDate = DateTime.Now },
            new Client { Nom = "Dupont", Prenom = "Jean", Email = "jean@email.com", Password = "user123", Telephone = "+216 20 000 001", IsAdmin = false, CreatedDate = DateTime.Now },
            new Client { Nom = "Martin", Prenom = "Marie", Email = "marie@email.com", Password = "user123", Telephone = "+216 20 000 002", IsAdmin = false, CreatedDate = DateTime.Now }
        );
        db.SaveChanges();
    }
}

app.UseCors("AllowAll");
app.MapControllers();
app.Run();