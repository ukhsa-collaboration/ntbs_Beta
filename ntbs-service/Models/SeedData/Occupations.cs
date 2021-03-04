using System.Collections.Generic;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public static class Occupations
    {
        public static IEnumerable<Occupation> GetOccupations()
        {
            return new[]
            {
                new Occupation { OccupationId = 1, Sector = "Agricultural/animal care", Role = "Works with cattle" },
                new Occupation { OccupationId = 2, Sector = "Agricultural/animal care", Role = "Works with wild animals" },
                new Occupation { OccupationId = 3, Sector = "Agricultural/animal care", Role = "Other" },
                new Occupation { OccupationId = 4, Sector = "Education", Role = "Full-time student" },
                new Occupation { OccupationId = 5, Sector = "Education", Role = "Lecturer" },
                new Occupation { OccupationId = 6, Sector = "Education", Role = "Teacher incl. nursery" },
                new Occupation { OccupationId = 7, Sector = "Education", Role = "Other" },
                new Occupation { OccupationId = 8, Sector = "Health care", Role = "Community care worker" },
                new Occupation { OccupationId = 9, Sector = "Health care", Role = "Dentist" },
                new Occupation { OccupationId = 10, Sector = "Health care", Role = "Doctor" },
                new Occupation { OccupationId = 11, Sector = "Health care", Role = "Nurse" },
                new Occupation { OccupationId = 12, Sector = "Health care", Role = "Other" },
                new Occupation { OccupationId = 13, Sector = "Laboratory/Pathology", Role = "Laboratory staff" },
                new Occupation { OccupationId = 14, Sector = "Laboratory/Pathology", Role = "Microbiologist" },
                new Occupation { OccupationId = 15, Sector = "Laboratory/Pathology", Role = "Pathologist" },
                new Occupation { OccupationId = 16, Sector = "Laboratory/Pathology", Role = "PM attendant" },
                new Occupation { OccupationId = 17, Sector = "Laboratory/Pathology", Role = "Other" },
                new Occupation { OccupationId = 18, Sector = "Social/prison service", Role = "Homeless sector worker" },
                new Occupation { OccupationId = 19, Sector = "Social/prison service", Role = "Prison/detention official" },
                new Occupation { OccupationId = 20, Sector = "Social/prison service", Role = "Probation officer" },
                new Occupation { OccupationId = 21, Sector = "Social/prison service", Role = "Social worker" },
                new Occupation { OccupationId = 22, Sector = "Social/prison service", Role = "Other" },
                new Occupation { OccupationId = 23, Sector = "Other", Role = "Child" },
                new Occupation { OccupationId = 24, Sector = "Other", Role = "Housewife/househusband" },
                new Occupation { OccupationId = 25, Sector = "Other", Role = "Prisoner" },
                new Occupation { OccupationId = 26, Sector = "Other", Role = "Retired" },
                new Occupation { OccupationId = 27, Sector = "Other", Role = "Unemployed" },
                new Occupation { OccupationId = 28, Sector = "Other", Role = "Other", HasFreeTextField = true }
            };
        }
    }
}
