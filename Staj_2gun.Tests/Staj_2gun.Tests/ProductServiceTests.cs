using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Staj_2gun.Data;
using Staj_2gun.Models;
using Staj_2gun.Services;

public class ProductServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // her test için yeni db
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        using var context = GetInMemoryDbContext();
        var service = new ProductService(context);

        var product = new Product { Name = "Kalem", Stock = 10 };
        var result = await service.CreateAsync(product);

        Assert.NotNull(result);
        Assert.Equal("Kalem", result.Name);
        Assert.Single(context.Products);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        using var context = GetInMemoryDbContext();
        context.Products.AddRange(
            new Product { Name = "Defter", Stock = 5 },
            new Product { Name = "Silgi", Stock = 15 }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectProduct()
    {
        using var context = GetInMemoryDbContext();
        var product = new Product { Name = "Cetvel", Stock = 20 };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var result = await service.GetByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal("Cetvel", result!.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyProduct()
    {
        using var context = GetInMemoryDbContext();
        var product = new Product { Name = "Tükenmez Kalem", Stock = 30 };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var updated = new Product { Name = "Dolma Kalem", Stock = 50 };
        var result = await service.UpdateAsync(product.Id, updated);

        Assert.NotNull(result);
        Assert.Equal("Dolma Kalem", result!.Name);
        Assert.Equal(50, result.Stock);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {
        using var context = GetInMemoryDbContext();
        var product = new Product { Name = "Silgi", Stock = 7 };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var result = await service.DeleteAsync(product.Id);

        Assert.True(result);
        Assert.Empty(context.Products);
    }
}
