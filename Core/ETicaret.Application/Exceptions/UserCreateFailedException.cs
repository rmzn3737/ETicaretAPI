using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Exceptions
{
    public class UserCreateFailedException: Exception
    {
        public UserCreateFailedException():base("Kullanıcı Oluşturulurken beklenmeyen bir hata ile karşılaşıldı.")
        {
            
        }

        public UserCreateFailedException(string? message) : base(message)
        {

        }

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
            //throw new NotImplementedException();
        }
    }
}
