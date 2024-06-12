using ETicaret.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ETicaret.Application.Features.Commands.Product.UpdateProduct
{
    
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
        {
            readonly IProductReadRepository _productReadRepository;
            readonly IProductWriteRepository _productWriteRepository;
            readonly ILogger<UpdateProductCommandHandler> _logger;

            public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ILogger<UpdateProductCommandHandler> logger)
            {
                _productReadRepository = productReadRepository;
                _productWriteRepository = productWriteRepository;
                _logger = logger;
            }

            public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
            {
                ETicaretAPI.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
                product.Stock = request.Stock;
                product.Name = request.Name;
                product.Price = request.Price;
                await _productWriteRepository.SaveAsync();
                _logger.LogInformation("Product güncellendi...");
                return new();
            }
        }
    
}
