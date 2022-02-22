using System;
using System.Collections.Generic;
using System.Linq;

namespace ntbs_service.Helpers
{
    
    public static class SearchStringHelper
    {
        public static List<string> GetSearchKeywords(string searchKeyword)
        {
            return searchKeyword
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLower())
                .ToList();
        }
    }
}

