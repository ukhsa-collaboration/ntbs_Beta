using System.Collections.Generic;
using System.Linq;
using Bogus;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace load_test_data_generation.Notifications
{
    internal static class NotificationSiteGenerator
    {
        private static readonly Faker<NotificationSite> testNotificationSite = new Faker<NotificationSite>()
            .RuleFor(ns => ns.SiteId, f => (int)f.Random.Enum<SiteId>())
            .RuleFor(ns => ns.SiteDescription, f => string.Empty);

        public static List<NotificationSite> GenerateNotificationSites()
        {
            var random = new Randomizer();
            return Enumerable.Range(0, random.Int(1, 2))
                .Select(_ => testNotificationSite.Generate())
                // If we happen to have generated the same site twice then remove the duplicate.
                // Tech debt: find (or write) a better way to pick a subset of a random size.
                .GroupBy(site => site.SiteId)
                .Select(grouping => grouping.First())
                .ToList();
        }
    }
}
