using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAuthenticationAuthorization.Models.Entities;

namespace SimpleAuthenticationAuthorization.Database;

public class Configuration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey();
    }
}