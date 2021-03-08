using System;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities
{
    public class CaseManagerTbService : IEquatable<CaseManagerTbService>
    {
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }

        public string CaseManagerUsername { get; set; }
        public virtual User CaseManager { get; set; }

        public bool Equals(CaseManagerTbService other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return TbServiceCode == other.TbServiceCode && CaseManagerUsername == other.CaseManagerUsername;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((CaseManagerTbService)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TbServiceCode, CaseManagerUsername);
        }
    }
}
