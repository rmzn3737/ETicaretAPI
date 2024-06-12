
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace ETicaret.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetAllProductQueryHandler> _logger;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }
        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürünlerin tamamı listelendi...");
           // throw new Exception("Global exception oluşturduk ve test için bu hatayı bilinçli fırlattık...");
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();//Skip(pagination.Page * pagination.Size).Take(pagination.Size);

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
