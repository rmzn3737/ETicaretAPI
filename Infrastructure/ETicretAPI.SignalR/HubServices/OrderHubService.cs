﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Hubs;
using ETicretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ETicretAPI.SignalR.HubServices
{
    public class OrderHubService:IOrderHubService
    {
        readonly IHubContext<OrderHub> _hubContext;

        public OrderHubService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OrderAddedMessageAsync(string message)
            =>await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage,message);
    }
}
