using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class ProductImageFile:File
    { 
        //public int Width { get; set;
        public bool Showcase { get; set; }//Vitrin görseli ayarı, vitrin görseli mi değil mi ?
        public ICollection<Product>  Products { get; set; } 
    }
}
