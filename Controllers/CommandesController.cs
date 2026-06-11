using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CommandesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Commandes.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var commande = _db.Commandes.FirstOrDefault(c => c.Id == id);
        return commande == null ? NotFound() : Ok(commande);
    }

    [HttpGet("byClient/{clientId}")]
    public IActionResult GetByClientId(int clientId)
    {
        var commandes = _db.Commandes.Where(c => c.ClientId == clientId).ToList();
        return Ok(commandes);
    }

    [HttpPost]
    public IActionResult Create(Commande commande)
    {
        commande.Date = DateTime.Now;
        commande.Statut = "en_attente";
        _db.Commandes.Add(commande);
        _db.SaveChanges();
        return Ok(commande);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Commande commande)
    {
        var existing = _db.Commandes.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        existing.Produit = commande.Produit;
        existing.Quantite = commande.Quantite;
        existing.PrixTotal = commande.PrixTotal;
        existing.Statut = commande.Statut;
        existing.ClientNom = commande.ClientNom;
        existing.ClientEmail = commande.ClientEmail;
        existing.ClientTelephone = commande.ClientTelephone;
        _db.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var commande = _db.Commandes.FirstOrDefault(c => c.Id == id);
        if (commande == null) return NotFound();
        _db.Commandes.Remove(commande);
        _db.SaveChanges();
        return NoContent();
    }
}