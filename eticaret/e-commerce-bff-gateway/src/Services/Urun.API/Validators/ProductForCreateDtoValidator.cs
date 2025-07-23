using FluentValidation;
using Urun.API.DTOs;

namespace Urun.API.Validators
{
    public class ProductForCreateDtoValidator : AbstractValidator<ProductForCreateDto>
    {
        public ProductForCreateDtoValidator() 
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz.")
                .Length(3, 100)
                .WithMessage("Ürün adı en az 3 ve en fazla 100 karakter olmalıdır.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Ürün açıklaması boş olamaz.")
                .Length(1, 500)
                .WithMessage("Ürün açıklaması en az 1 ve en fazla 500 karakter olmalıdır.");

            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage("Ürün fiyatı boş olamaz.")
                .GreaterThan(0)
                .WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(p => p.StockQuantity)
                .NotEmpty()
                .WithMessage("Ürün stok adeti boş olamaz")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ürün stok adeti negatif olamaz.");
        
        }
    }
}
