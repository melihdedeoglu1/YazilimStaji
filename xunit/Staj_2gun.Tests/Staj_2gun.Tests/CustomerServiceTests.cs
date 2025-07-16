using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Staj_2gun.Data;
using Staj_2gun.Models;
using Staj_2gun.Services;
using System.Linq;

public class CustomerServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Her test için ayrı DB
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        using var context = GetInMemoryDbContext();
        var service = new CustomerService(context);

        var customer = new Customer { Name = "Test Müşteri" };
        var result = await service.CreateAsync(customer);

        Assert.NotNull(result);
        Assert.Equal("Test Müşteri", result.Name);
        Assert.Single(context.Customers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        using var context = GetInMemoryDbContext();
        context.Customers.Add(new Customer { Name = "C1" });
        context.Customers.Add(new Customer { Name = "C2" });
        await context.SaveChangesAsync();

        var service = new CustomerService(context);
        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
    {
        using var context = GetInMemoryDbContext();
        var customer = new Customer { Name = "Aranan Müşteri" };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var service = new CustomerService(context);
        var result = await service.GetByIdAsync(customer.Id);

        Assert.NotNull(result);
        Assert.Equal("Aranan Müşteri", result!.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomerName()
    {
        using var context = GetInMemoryDbContext();
        var customer = new Customer { Name = "Eski İsim" };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var service = new CustomerService(context);
        var updated = new Customer { Name = "Yeni İsim" };

        var result = await service.UpdateAsync(customer.Id, updated);

        Assert.NotNull(result);
        Assert.Equal("Yeni İsim", result!.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomer()
    {
        using var context = GetInMemoryDbContext();
        var customer = new Customer { Name = "Silinecek" };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var service = new CustomerService(context);
        var result = await service.DeleteAsync(customer.Id);

        Assert.True(result);
        Assert.Empty(context.Customers);
    }
}
