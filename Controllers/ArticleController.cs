using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private static List<Article> _articles = new()
    {
        new Article { Id = 1, Nom = "Produit A", Prix = 100, Stock = 10 },
        new Article { Id = 2, Nom = "Produit B", Prix = 50, Stock = 20 }
    };
    private static int _nextId = 3;

    [HttpGet]
    public IActionResult GetAll() => Ok(_articles);

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        return article == null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public IActionResult Create(Article article)
    {
        article.Id = _nextId++;
        _articles.Add(article);
        return Ok(article);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Article article)
    {
        var existing = _articles.FirstOrDefault(a => a.Id == id);
        if (existing == null) return NotFound();
        existing.Nom = article.Nom;
        existing.Prix = article.Prix;
        existing.Stock = article.Stock;
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return NotFound();
        _articles.Remove(article);
        return NoContent();
    }
}

public class Article
{
    public int Id { get; set; }
    public string Nom { get; set; } = "";
    public decimal Prix { get; set; }
    public int Stock { get; set; }
}