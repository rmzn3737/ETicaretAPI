using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using MediatR;

namespace ETicaret.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler:IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.CompleteOrderAsync(request.Id);
            return new ();
        }
    }
}
