using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Staj_2gun.Data;
using Staj_2gun.Models;
using Staj_2gun.Services;
using System;

public class OrderItemServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOrderItem()
    {
        using var context = GetInMemoryDbContext();
        var service = new OrderItemService(context);

        var item = new OrderItem { ProductId = 1, OrderId = 1, Quantity = 5 };
        var result = await service.CreateAsync(item);

        Assert.NotNull(result);
        Assert.Equal(5, result.Quantity);
        Assert.Single(context.OrderItems);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        using var context = GetInMemoryDbContext();
        context.OrderItems.AddRange(
            new OrderItem { ProductId = 1, OrderId = 1, Quantity = 2 },
            new OrderItem { ProductId = 2, OrderId = 1, Quantity = 4 }
        );
        await context.SaveChangesAsync();

        var service = new OrderItemService(context);
        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectItem()
    {
        using var context = GetInMemoryDbContext();
        var item = new OrderItem { ProductId = 1, OrderId = 1, Quantity = 3 };
        context.OrderItems.Add(item);
        await context.SaveChangesAsync();

        var service = new OrderItemService(context);
        var result = await service.GetByIdAsync(item.Id);

        Assert.NotNull(result);
        Assert.Equal(3, result!.Quantity);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateItem()
    {
        using var context = GetInMemoryDbContext();
        var item = new OrderItem { ProductId = 1, OrderId = 1, Quantity = 2 };
        context.OrderItems.Add(item);
        await context.SaveChangesAsync();

        var service = new OrderItemService(context);
        var updated = new OrderItem { ProductId = 1, OrderId = 1, Quantity = 10 };
        var result = await service.UpdateAsync(item.Id, updated);

        Assert.NotNull(result);
        Assert.Equal(10, result!.Quantity);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        using var context = GetInMemoryDbContext();
        var item = new OrderItem { ProductId = 1, OrderId = 1, Quantity = 2 };
        context.OrderItems.Add(item);
        await context.SaveChangesAsync();

        var service = new OrderItemService(context);
        var result = await service.DeleteAsync(item.Id);

        Assert.True(result);
        Assert.Empty(context.OrderItems);
    }
}
