using System.Net;
using CatalogAPI.Entities;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    // GET
    private IProductRepository _repository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = await _repository.GetProducts();
        
        return products;
    }
    
    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return NotFound();
        }
        return Ok(product);
    }
    
    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    public async Task<IActionResult> GetProductByCategory(string category)
    {
        var products = await _repository.GetProductByCategory(category);
        return Ok(products);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        await _repository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        return Ok(await _repository.UpdateProduct(product));
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        return Ok(await _repository.DeleteProduct(id));
    }
    
}