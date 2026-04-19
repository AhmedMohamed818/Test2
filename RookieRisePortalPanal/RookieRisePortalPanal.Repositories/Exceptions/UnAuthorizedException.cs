using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Repositories.Exceptions
{
    public class UnAuthorizedException(string message = "Invalid Email") : Exception(message)
    {
    }
}
