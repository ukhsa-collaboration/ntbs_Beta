using System.Collections.Generic;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public static class Continents
    {
        public static IEnumerable<Continent> GetContinents()
        {
            return new List<Continent>
            {
                new Continent { ContinentId = 1, Name = "Africa", Code = "AF" },
                new Continent { ContinentId = 2, Name = "Antarctica", Code = "AN" },
                new Continent { ContinentId = 3, Name = "Asia", Code = "AS" },
                new Continent { ContinentId = 4, Name = "Central Europe", Code = "CEU" },
                new Continent { ContinentId = 5, Name = "East Asia", Code = "EAS" },
                new Continent { ContinentId = 6, Name = "East Europe", Code = "EEU" },
                new Continent { ContinentId = 7, Name = "East Mediterranean", Code = "EMD" },
                new Continent { ContinentId = 8, Name = "Europe", Code = "EU" },
                new Continent { ContinentId = 9, Name = "North Africa", Code = "NAF" },
                new Continent { ContinentId = 10, Name = "North America", Code = "NA" },
                new Continent { ContinentId = 11, Name = "North America and Oceania", Code = "NAO" },
                new Continent { ContinentId = 12, Name = "Oceania", Code = "OC" },
                new Continent { ContinentId = 13, Name = "South America", Code = "SA" },
                new Continent { ContinentId = 14, Name = "South and Central America, and the Caribbean", Code = "SCA" },
                new Continent { ContinentId = 15, Name = "South Asia", Code = "SAS" },
                new Continent { ContinentId = 16, Name = "South East Asia", Code = "SEA" },
                new Continent { ContinentId = 17, Name = "Sub-Saharan Africa", Code = "SSA" },
                new Continent { ContinentId = 18, Name = "Unknown", Code = "UU" },
                new Continent { ContinentId = 19, Name = "West Europe", Code = "WEU" }
            };
        }
    }
}
