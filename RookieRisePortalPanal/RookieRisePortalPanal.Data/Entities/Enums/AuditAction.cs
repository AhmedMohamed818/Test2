using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Data.Entities.Enums
{
    public enum AuditAction
    {
        Create,
        Update,
        Delete,
        OtpRequested,
        OtpSuccess,
        OtpFailed
    }
}
