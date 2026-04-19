using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.AccountService.DTO
{
    public class UserResultDto
    {
        public string Email { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public string Token { get; set; }
    }
}
