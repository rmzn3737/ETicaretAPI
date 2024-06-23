using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.Order;
using ETicaret.Application.Repositories;
using ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            //orderCode = orderCode.Substring(0, orderCode.IndexOf(".")+1);
            //orderCode = orderCode.Substring(orderCode.IndexOf(".")+1, orderCode.Length -orderCode.IndexOf(".")-1);
            orderCode = orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                OrderCode=orderCode
            });
            await _orderWriteRepository.SaveAsync();
        }
    }
}
