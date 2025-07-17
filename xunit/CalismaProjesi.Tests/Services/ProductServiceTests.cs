using CalismaProjesi.Services;
using CalismaProjesi.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq.Expressions;

namespace CalismaProjesi.Tests.Services
{
    public class ProductServiceTests
    {

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            //arrange hazırlık işlemleri
            //önce moq yapılmalı
            var mockRepo = new Mock<IProductService>();
            var TestProduct = new List<Product>
            {
                new Product { Id = 1, Name = "deneme1", Price=60.0 },
                new Product { Id = 2, Name = "deneme2", Price=70.0 }
            };


            mockRepo.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(TestProduct);

            var productService = mockRepo.Object;



            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await productService.GetAllProductsAsync();



            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result[0]);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("deneme1", result[0].Name);
            Assert.Equal(60.0, result[0].Price);

            Assert.NotNull(result[1]);
            Assert.Equal(2, result[1].Id);
            Assert.Equal("deneme2", result[1].Name);
            Assert.Equal(70.0, result[1].Price);

        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(30, false)]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenIdExists(int id, bool shouldExist)
        {
            //arrange hazırlık işlemleri
            //önce moq yapılmalı

            var mockRepo = new Mock<IProductService>() ;

            var TestProduct = new Product { Id = 1, Name = "deneme1", Price = 60.0 };

            mockRepo.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(TestProduct);
            mockRepo.Setup(repo => repo.GetProductByIdAsync(30)).ReturnsAsync((Product?)null);


            var productService = mockRepo.Object;

            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await productService.GetProductByIdAsync(id);


            //assert karşılaştırma işlemleri burada yapılır

            if (shouldExist)
            { 
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("deneme1", result.Name);
                Assert.Equal(60.0, result.Price);

            }
            else
            {
                Assert.Null(result);                
            }

        }


        public static IEnumerable<object[]> GetValidProducts()
        {
            yield return new object[] { new Product { Id = 1, Name = "deneme1", Price = 60.0 } };
            yield return new object[] { new Product { Id = 2, Name = "deneme2", Price = 70.0 } };
        }

        [Theory]
        [MemberData(nameof(GetValidProducts))]
        public async Task CreateProductAsync_ShouldReturnCreatedProduct_WhenProductIsValid(Product product) 
        {
            //arrange hazırlık işlemleri
            //moq yapılmalı

            var mockRepo = new Mock<IProductService>();

            mockRepo.Setup(repo => repo.CreateProductAsync(product)).ReturnsAsync((Product p) => p);

            var productService = mockRepo.Object;

            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await productService.CreateProductAsync(product);


            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);

        }

        public static IEnumerable<object[]> GetProductUpdateData()
        {
            yield return new object[]
            {
        1,
        new Product { Id = 1, Name = "Kalem", Price = 15 }
            };

            yield return new object[]
            {
        2,
        new Product { Id = 2, Name = "Defter", Price = 25 }
            };
        }

        [Theory]
        [MemberData(nameof(GetProductUpdateData))]
        public async Task UpdateProductAsync_ShouldReturnUpdatedProduct_WhenProductIsValid(int id, Product product)
        {
            //arrange hazırlık işlemleri
            //moq yapılmalı
            var mockRepo = new Mock<IProductService>();

            mockRepo.Setup(r=> r.GetProductByIdAsync(id)).ReturnsAsync(product);

            mockRepo.Setup(r=> r.UpdateProductAsync(id, product)).ReturnsAsync((int id, Product p) => p);


            var productService = mockRepo.Object;


            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await productService.UpdateProductAsync(id, product);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);               
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);


        }


        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists(int id) 
        {
            //arrange hazırlık işlemleri
            //moq yapılmalı

            var mockRepo = new Mock<IProductService>();

            mockRepo.Setup(repo => repo.DeleteProductAsync(id)).ReturnsAsync(true);

            var productService = mockRepo.Object;

            //act fonksiyonu çağırma işlemleri burada yapılır

            var result = await productService.DeleteProductAsync(id);

            //assert karşılaştırma işlemleri burada yapılır

            Assert.True(result);

        }


    }
}
