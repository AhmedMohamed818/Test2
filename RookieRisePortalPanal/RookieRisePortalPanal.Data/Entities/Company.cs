using Microsoft.AspNetCore.Identity;

namespace RookieRisePortalPanal.Data.Entities
{
    public class Company : ITrackableEntity
    {
        public Guid CompanyId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }

        //  Navigation (One Company → One Users)
        public AppUser User { get; set; } = null!;




        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }


        public bool IsDeleted { get; set; } = false; // soft delete
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}