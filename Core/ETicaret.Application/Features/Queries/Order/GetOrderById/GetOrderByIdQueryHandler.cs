using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using MediatR;

namespace ETicaret.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler:IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;
        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetOrderByIdAsync(request.Id);
            return new()
            {
                Id=data.Id,
                OrderCode = data.OrderCode,
                Address = data.Address,
                BasketItems = data.BasketItems,
                Description = data.Description,
                CreatedDate = data.CreatedDate,
            };
        }
    }
}
