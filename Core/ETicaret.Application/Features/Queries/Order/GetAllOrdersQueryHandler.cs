using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using MediatR;

namespace ETicaret.Application.Features.Queries.Order
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var data= await _orderService.GetAllOrdersAsync(request.Page,request.Size);

            return new()
            {
                TotalOrderCount = data.TotalOrderCount,
                Orders = data.Orders
            };

            //return data.Select(o => new GetAllOrdersQueryResponse
            //{
            //    CreatedDate = o.CreatedDate,//todo Burada otomapper da kullanılabilir. Hoca daha elverişli olur dedi.
            //    OrderCode = o.OrderCode,
            //    TotalPrice = o.TotalPrice,
            //    UserName = o.UserName,
            //}).ToList();
        }
    }

}
