:var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://mon-projet-angular-examen.web.app",
                "https://projet-angular-final.web.app"    // ← AJOUTE CETTE LIGNE
              )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();

