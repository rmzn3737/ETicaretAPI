using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class Order:BaseEntity
    {
        public int CustomerID { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }//Value Object yapabilirdik, yani şehir, mahalle vb. parçalara bölebiliriz.
        public ICollection<Product> Products { get; set; }//Bir siparişin birden fazla productı olduğunu ifade ediyor.
        public Customer Customer { get; set; }
    }
}
