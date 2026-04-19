using RookieRisePortalPanal.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Data.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string TableName { get; set; } = null!;
        public AuditAction Action { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }


        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Description { get; set; }
    }
}
