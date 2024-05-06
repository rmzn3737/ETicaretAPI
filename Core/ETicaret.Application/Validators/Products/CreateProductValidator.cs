using ETicaret.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Lütfen ürün adını boş bırakmayınız.")
                .MinimumLength(5)
                .MaximumLength(150)
                    .WithMessage("Ürün adı uzunluğu 5 ile 150 karakter aralığında olmalıdır.");
            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stok bilgisini boş bırakmayınız.")
                .Must(s => s >= 0)
                    .WithMessage("Stok bilgisi negatif olamaz.");
            
            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen fiyat bilgisini boş bırakmayınız.")
                .Must(s => s >= 0)
                    .WithMessage("Fiyat bilgisi negatif olamaz.");
        }
    }
}
