using System;
using System.Collections.Generic;
using ntbs_service.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class SearchResults
    {
        public IList<Notification> notifications;
        public int numberOfResults;
    }
}