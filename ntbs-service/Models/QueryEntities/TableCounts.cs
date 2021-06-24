using System;

namespace ntbs_service.Models.QueryEntities
{
    public class TableCounts
    {
        public DateTime CountTime { get; set; }
        public int MigrationNotificationsViewCount { get; set;}
        public int MigrationMBovisAnimalExposureViewCount { get; set;}
        public int MigrationMBovisExposureToKnownCaseViewCount { get; set;}
        public int MigrationMBovisOccupationExposuresViewCount { get; set;}
        public int MigrationMBovisUnpasteurisedMilkConsumptionViewCount { get; set;}
        public int MigrationSocialContextAddressViewCount { get; set;}
        public int MigrationSocialContextVenueViewCount { get; set;}
        public int TransfersViewCount { get; set;}
        public int TreatmentOutcomesCount { get; set;}
        public int EtsNotificationsCount { get; set;}
        public int LtbrNotificationsCount { get; set;}
        public int ETS_NotificationCount { get; set;}
        public int LTBR_DiseasePeriodCount { get; set;}
        public int LTBR_PatientEpisodeCount { get; set;}
        public int NotificationClusterMatchCount { get; set;}
        public int NotificationSpecimenMatchCount { get; set;}
        public int EtsSpecimenMatchCount { get; set;}
    }
}
