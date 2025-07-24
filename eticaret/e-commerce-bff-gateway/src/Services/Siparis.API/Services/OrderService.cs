using Siparis.API.DTOs;
using Siparis.API.Models;
using Siparis.API.Repositories;
using MassTransit;
using Shared.Contracts;

namespace Siparis.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory, ILogger<OrderService> logger, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Order> CreateAsync(int userId, string userEmail, OrderForCreateDto orderDto)
        {
          
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://urun-servisi:8080");

            double totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in orderDto.OrderItems)
            {
                ProductDetailDto? product = null;
                try
                {
                    var response = await client.GetAsync($"/api/Product/{itemDto.ProductId}");
                    if (response.IsSuccessStatusCode)
                    {
                        product = await response.Content.ReadFromJsonAsync<ProductDetailDto>();
                    }
                    else
                    {
                        _logger.LogWarning("Ürün bilgisi çekilemedi. ProductId: {ProductId}, Status: {StatusCode}", itemDto.ProductId, response.StatusCode);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Urun.API'ye istek atılırken hata oluştu. ProductId: {ProductId}", itemDto.ProductId);
                    continue;
                }

                if (product != null)
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = product.Price,
                        ProductName = product.Name
                    });
                    totalPrice += itemDto.Quantity * product.Price;
                }
            }

            if (!orderItems.Any())
            {
                throw new Exception("Sipariş oluşturmak için geçerli hiçbir ürün bulunamadı.");
            }

            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };
            
       
            var savedOrder = await _orderRepository.CreateAsync(newOrder);
            _logger.LogInformation("Sipariş veritabanına başarıyla kaydedildi. SiparisId: {SiparisId}", savedOrder.Id);

            
            var eventMessage = new SiparisOlusturulduEvent
            {
                SiparisId = savedOrder.Id,
                KullaniciId = savedOrder.UserId,
                KullaniciEmail = userEmail, 
                ToplamTutar = savedOrder.TotalPrice,
                SiparisKalemleri = savedOrder.OrderItems.Select(item => new SiparisKalemiDto
                {
                    UrunId = item.ProductId,
                    UrunAdi = item.ProductName,
                    Fiyat = item.Price,
                    Adet = item.Quantity
                }).ToList()
            };

            
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation("SiparisOlusturulduEvent, RabbitMQ'ya yayınlandı. SiparisId: {SiparisId}", savedOrder.Id);

           
            return savedOrder;
        }


    }
}
