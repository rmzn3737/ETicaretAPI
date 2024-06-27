using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Exceptions
{
    public class PasswordCahngeFailedException:Exception
    {
        public PasswordCahngeFailedException() : base("Şifre güncellenirken bir sorun oluştu.")
        {

        }

        public PasswordCahngeFailedException(string? message) : base(message)
        {

        }

        public PasswordCahngeFailedException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
