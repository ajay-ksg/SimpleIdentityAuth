using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using SimpleAuthenticationAuthorization.Database.Repository;
using SimpleAuthenticationAuthorization.Enums;
using SimpleAuthenticationAuthorization.Models.Entities;
using static SimpleAuthenticationAuthorization.Enums.Roles;

namespace SimpleAuthenticationAuthorization.Controllers;

[Route("api/[controller]")]
public class ProductController(ProductRepository productRepository) : ControllerBase
{
    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return  Ok(await productRepository.GetAll());
    }
    
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        return Ok(await productRepository.Create(product));
    }
}