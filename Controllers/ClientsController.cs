using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ClientsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Clients.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var client = _db.Clients.FirstOrDefault(c => c.Id == id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var client = _db.Clients.FirstOrDefault(c => c.Email == request.Email && c.Password == request.Password);
        if (client == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });
        return Ok(new { client.Id, client.Nom, client.Prenom, client.Email, client.Telephone, client.IsAdmin });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] Client client)
    {
        var existing = _db.Clients.FirstOrDefault(c => c.Email == client.Email);
        if (existing != null)
            return BadRequest(new { message = "Cet email est déjà utilisé" });
        client.IsAdmin = false;
        client.CreatedDate = DateTime.Now;
        _db.Clients.Add(client);
        _db.SaveChanges();
        return Ok(new { client.Id, client.Nom, client.Prenom, client.Email, client.Telephone, client.IsAdmin });
    }

    [HttpPost]
    public IActionResult Create(Client client)
    {
        client.CreatedDate = DateTime.Now;
        _db.Clients.Add(client);
        _db.SaveChanges();
        return Ok(client);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Client client)
    {
        var existing = _db.Clients.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        existing.Nom = client.Nom;
        existing.Prenom = client.Prenom;
        existing.Email = client.Email;
        existing.Password = client.Password;
        existing.Telephone = client.Telephone;
        existing.IsAdmin = client.IsAdmin;
        _db.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var client = _db.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null) return NotFound();
        _db.Clients.Remove(client);
        _db.SaveChanges();
        return NoContent();
    }
}