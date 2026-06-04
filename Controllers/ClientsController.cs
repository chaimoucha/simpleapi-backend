using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private static List<Client> _clients = new()
    {
        new Client { Id = 1, Nom = "Admin", Prenom = "Super", Email = "admin@test.com", Password = "admin123", IsAdmin = true, CreatedDate = DateTime.Now },
        new Client { Id = 2, Nom = "Dupont", Prenom = "Jean", Email = "jean@email.com", Password = "user123", IsAdmin = false, CreatedDate = DateTime.Now },
        new Client { Id = 3, Nom = "Martin", Prenom = "Marie", Email = "marie@email.com", Password = "user123", IsAdmin = false, CreatedDate = DateTime.Now }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(_clients);

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var client = _clients.FirstOrDefault(c => c.Id == id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var client = _clients.FirstOrDefault(c => c.Email == request.Email && c.Password == request.Password);
        if (client == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect" });
        
        return Ok(new { 
            client.Id, 
            client.Nom, 
            client.Prenom, 
            client.Email, 
            client.IsAdmin 
        });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] Client client)
    {
        // Vérifier si l'email existe déjà
        var existing = _clients.FirstOrDefault(c => c.Email == client.Email);
        if (existing != null)
        {
            return BadRequest(new { message = "Cet email est déjà utilisé" });
        }
        
        // Créer un nouvel ID
        client.Id = _clients.Max(c => c.Id) + 1;
        
        // Forcer IsAdmin à false pour les nouveaux comptes
        client.IsAdmin = false;
        
        // Ajouter la date de création automatique
        client.CreatedDate = DateTime.Now;
        
        _clients.Add(client);
        
        return Ok(new { 
            client.Id, 
            client.Nom, 
            client.Prenom, 
            client.Email, 
            client.IsAdmin 
        });
    }

    [HttpPost]
    public IActionResult Create(Client client)
    {
        client.Id = _clients.Max(c => c.Id) + 1;
        client.CreatedDate = DateTime.Now;
        _clients.Add(client);
        return Ok(client);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Client client)
    {
        var existing = _clients.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        existing.Nom = client.Nom;
        existing.Prenom = client.Prenom;
        existing.Email = client.Email;
        existing.Password = client.Password;
        existing.IsAdmin = client.IsAdmin;
        // Ne pas modifier CreatedDate
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var client = _clients.FirstOrDefault(c => c.Id == id);
        if (client == null) return NotFound();
        _clients.Remove(client);
        return NoContent();
    }
}

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = "";
    public string Prenom { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsAdmin { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;  // ← AJOUTÉ
}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}