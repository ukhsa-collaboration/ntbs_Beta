using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public class SocialContextVenueRepository : ItemRepository<SocialContextVenue>
    {
        public SocialContextVenueRepository(NtbsContext context) : base(context) { }

        protected override DbSet<SocialContextVenue> GetDbSet()
        {
            return _context.SocialContextVenue;
        }

        protected override SocialContextVenue GetEntityToUpdate(Notification notification, SocialContextVenue venue)
        {
            return notification.SocialContextVenues
                .First(s => s.SocialContextVenueId == venue.SocialContextVenueId);
        }
    }

    public class SocialContextAddressRepository : ItemRepository<SocialContextAddress>
    {
        public SocialContextAddressRepository(NtbsContext context) : base(context) { }

        protected override DbSet<SocialContextAddress> GetDbSet()
        {
            return _context.SocialContextAddress;
        }

        protected override SocialContextAddress GetEntityToUpdate(Notification notification, SocialContextAddress address)
        {
            return notification.SocialContextAddresses
                .First(s => s.SocialContextAddressId == address.SocialContextAddressId);
        }
    }
}
