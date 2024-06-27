using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.Order;
using MediatR;

namespace ETicaret.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler:IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;

        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool succeeded, CompletedOrderDTO dto) result = await _orderService.CompleteOrderAsync(request.Id);
            if (result.succeeded)
                await _mailService.SendCompletedOrderMailAsync(result.dto.EMail, result.dto.OrderCode,
                    result.dto.OrderDate, result.dto.Username);
            return new ();
        }
    }
}
