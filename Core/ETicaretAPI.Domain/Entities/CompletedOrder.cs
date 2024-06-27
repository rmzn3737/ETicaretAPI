using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Domain.Entities
{
    public class CompletedOrder:BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order  Order { get; set; }
    }
}
