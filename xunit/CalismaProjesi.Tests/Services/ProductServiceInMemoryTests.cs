using CalismaProjesi.Data;
using CalismaProjesi.Models;
using CalismaProjesi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalismaProjesi.Tests.Services
{
    public class ProductServiceInMemoryTests
    {
        private ProductContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ProductContext(options);
        }


        [Fact]
        public async Task CreateProductAsync_ShouldAddProductToDatabase() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product = new Product{ Id = 1, Name = "test", Price = 2.0 };

            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.CreateProductAsync(product);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test", result.Name);
            Assert.Equal(2.0, result.Price);
        }
        

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenIdExists() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);
            var product = new Product { Id = 1, Name = "test", Price = 2.0 };

            context.Products.Add(product);
            await context.SaveChangesAsync();


            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.GetProductByIdAsync(1);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test", result.Name);
            Assert.Equal(2.0, result.Price);

        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenIdNotExists()
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);
           
            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.GetProductByIdAsync(2);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.Null(result);

        }

        [Fact]
        public async Task CreateProductAsync_ShouldPersistProduct()
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product = new Product { Id = 1, Name = "test", Price = 2.0 };


            //act fonksiyonu çağırma işlemleri burada yapılır

            var created = await service.CreateProductAsync(product);
            var fromDb = await service.GetProductByIdAsync(created.Id);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(fromDb);
            Assert.Equal(1, fromDb.Id);
            Assert.Equal("test", fromDb.Name);
            Assert.Equal(2.0, fromDb.Price);

        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product1 = new Product { Id = 1, Name = "test1", Price = 2.0 };
            var product2 = new Product { Id = 2, Name = "test2", Price = 3.0 };

            // act fonksiyonu çağırma işlemleri burada yapılır

            var created = await service.CreateProductAsync(product1);
            var created2 = await service.CreateProductAsync(product2);

            var result = await service.GetAllProductsAsync();

            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.NotNull(result[0]);
            Assert.Equal(created.Id, result[0].Id);
            Assert.Equal(created.Name, result[0].Name);
            Assert.Equal(created.Price, result[0].Price);

            Assert.NotNull(result[1]);
            Assert.Equal(created2.Id, result[1].Id);
            Assert.Equal(created2.Name, result[1].Name);
            Assert.Equal(created2.Price, result[1].Price);

        }
        

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenIdExists() 
        {
            //arrange hazırlık işlemleri 

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product = new Product { Id = 1, Name = "test", Price = 2.0 };
            await service.CreateProductAsync(product);

            product.Name = "updated";
            product.Price = 3.0;


            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.UpdateProductAsync(1, product);
            var fromDB = await service.GetProductByIdAsync(1);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("updated", result.Name);
            Assert.Equal(3.0, result.Price);

            Assert.NotNull(fromDB);
            Assert.Equal(1, fromDB.Id);
            Assert.Equal("updated", fromDB.Name);
            Assert.Equal(3.0, fromDB.Price);

        }


        [Fact]
        public async Task DeleteProductAsync_ShouldRemoveProduct_WhenIdExists() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product = new Product { Id = 1, Name = "Test", Price = 3.0 };
            await service.CreateProductAsync(product);


            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.DeleteProductAsync(1);
            var fromDB = await service.GetProductByIdAsync(1);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.True(result);
            Assert.Null(fromDB); 
        }
        [Fact]
        public async Task DeleteProductAsync_ShouldReturnFalse_WhenIdDoesNotExist() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            //act fonksiyonu çağırma işlemleri burada yapılır
            var fromDB = await service.GetProductByIdAsync(1); 
            var result = await service.DeleteProductAsync(1);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.Null(fromDB);            
            Assert.False(result);
        }



        [Fact]
        public async Task CreateProductAsync_ShouldThrowException_WhenProductIsNull() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var invalidProduct = new Product { Id = 0, Name = "", Price = -1.0 };


            //act ve assert

            await Assert.ThrowsAsync<Exception>(async () => 
            {
                await service.CreateProductAsync(invalidProduct);
            });

        }

        [Fact]
        public void Product_ShouldFailValidation_WhenNameIsEmpty() 
        {
            //arrange hazırlık işlemleri

            var product = new Product { Id = 1, Name = "", Price = 10.0 };

            var validationContext = new ValidationContext(product, null, null);
            var validationResults = new List<ValidationResult>();

            // act fonksiyonu çağırma işlemleri burada yapılır

            bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("Ürün adı boş olamaz."));
        }

        [Fact]
        public void Product_ShouldFailValidation_WhenPriceIsOutOfRange()
        {
            //arrange hazırlık işlemleri

            var product = new Product { Id = 1, Name = "Test", Price = -5.0 };

            var validationContext = new ValidationContext(product, null, null);
            var validationResults = new List<ValidationResult>();

            //act fonksiyonu çağırma işlemleri burada yapılır

            bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.False(isValid);

            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("Fiyat 0.01 ile 10000.00 arasında olmalıdır."));
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(10000.00)]
        public void Product_ShouldPassValidation_WhenPriceIsOnBoundary(double price) 
        {
            //arrange hazırlık işlemleri

            var product = new Product { Id = 1, Name = "Test", Price = price };

            var context = new ValidationContext(product, null, null);
            var results = new List<ValidationResult>();

            //act fonksiyonu çağırma işlemleri burada yapılır

            bool isValid = Validator.TryValidateObject(product, context, results, true);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Product_ShouldFailValidation_WhenNameIsWhiteSpace() 
        { 
            //arrange hazırlık işlemleri

            var product = new Product { Id = 1, Name = "   ", Price = 10.0 };

            var context = new ValidationContext(product);

            var results = new List<ValidationResult>();


            //act fonksiyonu çağırma işlemleri burada yapılır

            bool isValid = Validator.TryValidateObject(product, context, results, true);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.False(isValid);

            Assert.Contains(results, v => v.ErrorMessage.Contains("Ürün adı boş olamaz."));

        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNull_WhenIdMisMatch() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var service = new ProductService(context);

            var product = new Product { Id = 1, Name = "test", Price = 2.0 };
            await service.CreateProductAsync(product);

            product.Id = 2;


            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await service.UpdateProductAsync(1, product);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.Null(result);

        }

        [Fact]
        public async Task MockandGetProductById()
        {
            //arrange hazırlık işlemleri

            var mockService = new Mock<IProductService>();

            mockService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "mocked", Price = 2.0 });


            // act fonksiyonu çağırma işlemleri burada yapılır

            var result = await mockService.Object.GetProductByIdAsync(1);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal("mocked", result.Name);



        }

    }
}
