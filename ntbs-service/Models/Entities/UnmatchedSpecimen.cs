using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    public class UnmatchedSpecimen : SpecimenBase
    {
        public IEnumerable<SpecimenPotentialMatch> PotentialMatches { get; set; }
    }
}
