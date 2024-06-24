﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.Order;
using ETicaret.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService:IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            //orderCode = orderCode.Substring(0, orderCode.IndexOf(".")+1);
            //orderCode = orderCode.Substring(orderCode.IndexOf(".")+1, orderCode.Length -orderCode.IndexOf(".")-1);
            orderCode = orderCode.Substring(orderCode.IndexOf(",") + 1, orderCode.Length - orderCode.IndexOf(",") - 1);
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                OrderCode=orderCode
            });
            await _orderWriteRepository.SaveAsync();
        }
        
        
        //todo Alttaki kodda return yerine => bunu kullanabiliyoruz, c# ın bir özelliği. Lamda operasyonu, scope parantezlerini silip bunu kullanıyoruz. Tek satırlık işlemlerde yapıyoruz.
        public async Task<ListOrder> GetAllOrdersAsync(int page,int size)

        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

            var data = query.Skip(page * size).Take(size);
                //.Take((page*size)..size)
                
                return new()
                {
                    TotalOrderCount = await query.CountAsync(),
                    Orders= await data.Select(o=>new 
                    {
                        CreateDate=o.CreatedDate,
                        OrderCode=o.OrderCode,
                        TotalPrice=o.Basket.BasketItems.Sum(bi=>bi.Product.Price *bi.Quantity),
                        UserName=o.Basket.User.UserName
                    }).ToListAsync()
                };

        }

    }
}
