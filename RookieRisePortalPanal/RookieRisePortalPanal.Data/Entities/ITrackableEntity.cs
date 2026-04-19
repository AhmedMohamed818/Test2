using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Data.Entities
{
    public interface ITrackableEntity
    {
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }


        public bool IsDeleted { get; set; }// soft delete
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
