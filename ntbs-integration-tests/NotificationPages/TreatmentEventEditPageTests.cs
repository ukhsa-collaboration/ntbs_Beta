using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TreatmentEventEditPageTests : TestRunnerNotificationBase
    {
        private const int CURLY_BRACKET_EVENT_ID = 16;
        protected override string NotificationSubPath => NotificationSubPaths.EditTreatmentEvent(CURLY_BRACKET_EVENT_ID);
        public TreatmentEventEditPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory)
        {
        }
        
        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_TREATMENTEVENT,
                    NotificationStatus = NotificationStatus.Notified,
                    TreatmentEvents = new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            TreatmentEventId = CURLY_BRACKET_EVENT_ID,
                            EventDate = new DateTime(2012, 1, 1),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            Note = "{{abc}}"
                        }
                    }
                }
            };
        }
        
        [Fact]
        public async Task GetEditOfTreatemntEvent_RemovesCurlyBrackets()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_TREATMENTEVENT;
            var url = GetCurrentPathForId(notificationId);
            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var detailsContainer = document.GetElementById("treatment-events").TextContent;
            var notes = document.QuerySelector<IHtmlTextAreaElement>("#treatmentevent-note").Value;

            Assert.DoesNotContain("{", detailsContainer);
            Assert.DoesNotContain("}", detailsContainer);
            Assert.Contains("abc", detailsContainer);
            
            Assert.DoesNotContain("{", notes);
            Assert.DoesNotContain("}", notes);
            Assert.Equal("abc", notes);
        }
    }
}

