using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleAuthenticationAuthorization.Models.Entities;

[PrimaryKey(nameof(Id))]
public class Product
{
    int Id { get; set; }
    
    string Name { get; set; }
    
    decimal Price { get; set; }
    
    string Description { get; set; }
    
}