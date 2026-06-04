using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandesController : ControllerBase
{
    private static List<Commande> _commandes = new();
    private static int _nextId = 1;

    [HttpGet]
    public IActionResult GetAll() => Ok(_commandes);

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var commande = _commandes.FirstOrDefault(c => c.Id == id);
        return commande == null ? NotFound() : Ok(commande);
    }

    [HttpGet("byClient/{clientId}")]
    public IActionResult GetByClientId(int clientId)
    {
        var commandes = _commandes.Where(c => c.ClientId == clientId).ToList();
        return Ok(commandes);
    }

    [HttpPost]
    public IActionResult Create(Commande commande)
    {
        commande.Id = _nextId++;
        commande.Date = DateTime.Now;
        _commandes.Add(commande);
        return Ok(commande);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Commande commande)
    {
        var existing = _commandes.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        existing.Total = commande.Total;
        existing.ClientId = commande.ClientId;
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var commande = _commandes.FirstOrDefault(c => c.Id == id);
        if (commande == null) return NotFound();
        _commandes.Remove(commande);
        return NoContent();
    }
}

public class Commande
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public int ClientId { get; set; }
}