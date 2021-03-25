using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TreatmentEventEditPageTests : TestRunnerNotificationBase
    {
        private const int OUTCOME_TREATMENT_EVENT_ID = 10;
        private const int RESTART_TREATMENT_EVENT_ID = 11;
        private static readonly DateTime RESTART_TREATMENT_EVENT_DATE = new DateTime(2013, 2, 2);
        private static readonly string RESTART_TREATMENT_EVENT_NOTE = "This is a test string";
        private const int EDIT_TREATMENT_EVENT_ID = 12;
        private const int DELETE_TREATMENT_EVENT_ID = 13;
        private const int TRANSFER_IN_EVENT_ID = 14;

        private const int TREATMENT_OUTCOME_ID = 100;
        private const TreatmentOutcomeType TREATMENT_OUTCOME_TYPE = TreatmentOutcomeType.Lost;
        private const TreatmentOutcomeSubType TREATMENT_OUTCOME_SUBTYPE = TreatmentOutcomeSubType.TbCausedDeath;


        protected override string NotificationSubPath => NotificationSubPaths.EditTreatmentEvents;

        public TreatmentEventEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_TREATMENT_EVENTS,
                    NotificationStatus = NotificationStatus.Notified,
                    TreatmentEvents = new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            TreatmentEventId = OUTCOME_TREATMENT_EVENT_ID,
                            EventDate = new DateTime(2012, 1, 1),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcomeId = TREATMENT_OUTCOME_ID
                        },
                        new TreatmentEvent
                        {
                            TreatmentEventId = RESTART_TREATMENT_EVENT_ID,
                            EventDate = RESTART_TREATMENT_EVENT_DATE,
                            TreatmentEventType = TreatmentEventType.TreatmentRestart,
                            Note = RESTART_TREATMENT_EVENT_NOTE
                        },
                        new TreatmentEvent
                        {
                            TreatmentEventId = EDIT_TREATMENT_EVENT_ID,
                            EventDate = RESTART_TREATMENT_EVENT_DATE,
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcomeId = TREATMENT_OUTCOME_ID
                        },
                        new TreatmentEvent
                        {
                            TreatmentEventId = DELETE_TREATMENT_EVENT_ID,
                            EventDate = RESTART_TREATMENT_EVENT_DATE,
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            TreatmentEventId = TRANSFER_IN_EVENT_ID,
                            EventDate = RESTART_TREATMENT_EVENT_DATE,
                            TreatmentEventType = TreatmentEventType.TransferIn
                        }
                    }
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_RESTART,
                    NotificationStatus = NotificationStatus.Notified
                },
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_OUTCOME,
                    NotificationStatus = NotificationStatus.Notified
                }
            };
        }

        public static IList<TreatmentOutcome> GetSeedingOutcomes()
        {
            return new List<TreatmentOutcome>
            {
                new TreatmentOutcome
                {
                    TreatmentOutcomeId = TREATMENT_OUTCOME_ID,
                    TreatmentOutcomeType = TREATMENT_OUTCOME_TYPE,
                    TreatmentOutcomeSubType = TREATMENT_OUTCOME_SUBTYPE
                }
            };
        }

        [Fact]
        public async Task GetTreatmentEvents_ReturnsOk_ForNotified()
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTreatmentEvents_RendersTreatmentTable_ForPopulatedEntries()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFICATION_WITH_TREATMENT_EVENTS);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var treatmentTable = document.QuerySelector("#treatment-events");
            Assert.NotNull(treatmentTable);

            var treatmentRestartWithNoteRow = treatmentTable.QuerySelector($"#treatment-event-{RESTART_TREATMENT_EVENT_ID}");
            Assert.NotNull(treatmentRestartWithNoteRow);
            var restartWithNoteTextContent = treatmentRestartWithNoteRow.TextContent;
            Assert.Contains(RESTART_TREATMENT_EVENT_DATE.ConvertToString(), restartWithNoteTextContent);
            Assert.Contains(RESTART_TREATMENT_EVENT_NOTE, restartWithNoteTextContent);
            Assert.Contains(TreatmentEventType.TreatmentRestart.GetDisplayName(), restartWithNoteTextContent);

            var treatmentOutcomeRow = treatmentTable.QuerySelector($"#treatment-event-{OUTCOME_TREATMENT_EVENT_ID}");
            Assert.NotNull(treatmentOutcomeRow);
            var treatmentOutcomeTextContent = treatmentOutcomeRow.TextContent;
            // Will not duplicate check for date rendering
            Assert.Contains(TREATMENT_OUTCOME_TYPE.GetDisplayName(), treatmentOutcomeTextContent);
            Assert.Contains(TREATMENT_OUTCOME_SUBTYPE.GetDisplayName(), treatmentOutcomeTextContent);
        }

        [Fact]
        public async Task PostNewTreatmentRestart_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_RESTART;
            var url = GetPathForId(NotificationSubPaths.AddTreatmentEvent, notificationId);
            var document = await GetDocumentForUrlAsync(url);
            const string noteText = "Hello is it me you're looking for";

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedEventDate.Day"] = "01",
                ["FormattedEventDate.Month"] = "01",
                ["FormattedEventDate.Year"] = "2019",
                ["TreatmentEvent.TreatmentEventType"] = ((int)TreatmentEventType.TreatmentRestart).ToString(),
                ["TreatmentEvent.Note"] = noteText
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTreatmentEvents, notificationId));
            var treatmentEventsDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));
            var treatmentEventRows = treatmentEventsDocument.QuerySelectorAll("#treatment-events tbody tr");
            var row = treatmentEventRows.First();

            var textContent = row.TextContent;
            Assert.Contains(new DateTime(2019, 1, 1).ConvertToString(), textContent);
            Assert.Contains(TreatmentEventType.TreatmentRestart.GetDisplayName(), textContent);
            Assert.Contains(noteText, textContent);
        }

        [Fact]
        public async Task PostNewTreatmentOutcome_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_OUTCOME;
            var url = GetPathForId(NotificationSubPaths.AddTreatmentEvent, notificationId);
            var document = await GetDocumentForUrlAsync(url);
            const string noteText = "I can see it in your smile";

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedEventDate.Day"] = "01",
                ["FormattedEventDate.Month"] = "01",
                ["FormattedEventDate.Year"] = "2020",
                ["TreatmentEvent.TreatmentEventType"] = ((int)TreatmentEventType.TreatmentOutcome).ToString(),
                ["SelectedTreatmentOutcomeType"] = ((int)TREATMENT_OUTCOME_TYPE).ToString(),
                ["SelectedTreatmentOutcomeSubType"] = ((int)TREATMENT_OUTCOME_SUBTYPE).ToString(),
                ["TreatmentEvent.Note"] = noteText
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTreatmentEvents, notificationId));
            var treatmentEventsDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));
            var treatmentEventRows = treatmentEventsDocument.QuerySelectorAll("#treatment-events tbody tr");
            Assert.Single(treatmentEventRows);
            var row = treatmentEventRows.First();

            var textContent = row.TextContent;
            Assert.Contains(new DateTime(2020, 1, 1).ConvertToString(), textContent);
            Assert.Contains(TreatmentEventType.TreatmentOutcome.GetDisplayName(), textContent);
            Assert.Contains(noteText, textContent);
            Assert.Contains(TREATMENT_OUTCOME_TYPE.GetDisplayName(), textContent);
            Assert.Contains(TREATMENT_OUTCOME_SUBTYPE.GetDisplayName(), textContent);
        }

        [Fact]
        public async Task PostEditTreatmentEvent_ReturnsSuccessAndEditsResultInTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_TREATMENT_EVENTS;
            const int treatmentEventId = EDIT_TREATMENT_EVENT_ID;
            var dateTimeTargetValue = new DateTime(2015, 5, 5);
            const string noteTargetValue = "I can see it in your eyes";

            var url = GetPathForId(NotificationSubPaths.EditTreatmentEvent(treatmentEventId), notificationId);
            var document = await GetDocumentForUrlAsync(url);
            var preEditRow = document.QuerySelector($"#treatment-event-{EDIT_TREATMENT_EVENT_ID}");
            Assert.NotNull(preEditRow);
            Assert.DoesNotContain(noteTargetValue, preEditRow.TextContent);
            Assert.Contains(TreatmentEventType.TreatmentOutcome.GetDisplayName(), preEditRow.TextContent);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedEventDate.Day"] = dateTimeTargetValue.Day.ToString(),
                ["FormattedEventDate.Month"] = dateTimeTargetValue.Month.ToString(),
                ["FormattedEventDate.Year"] = dateTimeTargetValue.Year.ToString(),
                ["TreatmentEvent.TreatmentEventType"] = ((int)TreatmentEventType.TreatmentRestart).ToString(),
                ["TreatmentEvent.Note"] = noteTargetValue
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTreatmentEvents, notificationId));
            var treatmentEventsDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));

            var treatmentRestartWithNoteRow = treatmentEventsDocument.QuerySelector($"#treatment-event-{treatmentEventId}");
            Assert.NotNull(treatmentRestartWithNoteRow);
            var treatmentRestartTextContent = treatmentRestartWithNoteRow.TextContent;
            Assert.Contains(dateTimeTargetValue.ConvertToString(), treatmentRestartTextContent);
            Assert.Contains(noteTargetValue, treatmentRestartTextContent);
            Assert.Contains(TreatmentEventType.TreatmentRestart.GetDisplayName(), treatmentRestartTextContent);
        }


        [Fact]
        public async Task PostDelete_ReturnsSuccessAndRemovesResult()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_TREATMENT_EVENTS;
            var url = GetPathForId(NotificationSubPaths.EditTreatmentEvent(DELETE_TREATMENT_EVENT_ID), notificationId);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string> { };
            var result = await Client.SendPostFormWithData(document, formData, url, "Delete");

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTreatmentEvents, notificationId));
            var treatmentEventsDocument = await GetDocumentForUrlAsync(GetRedirectLocation(result));

            Assert.Null(treatmentEventsDocument.GetElementById($"social-context-venue-{DELETE_TREATMENT_EVENT_ID}"));
        }

        [Fact]
        public async Task PostNewTreatmentEventWithMissingFields_ReturnsValidationErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFIED_ID;
            var url = GetPathForId(NotificationSubPaths.AddTreatmentEvent, notificationId);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedEventDate.Day"] = "",
                ["FormattedEventDate.Month"] = "",
                ["FormattedEventDate.Year"] = "",
                ["TreatmentEvent.TreatmentEventType"] = "",
                ["TreatmentEvent.Note"] = ""
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertValidationErrorResponse();
            var resultDocument = await GetDocumentAsync(result);

            resultDocument.AssertErrorSummaryMessage(
                "TreatmentEvent-EventDate",
                "event-date",
                string.Format(ValidationMessages.RequiredEnter, "Event Date"));
            resultDocument.AssertErrorSummaryMessage(
                "TreatmentEvent-TreatmentEventType",
                "treatmentevent-type",
                string.Format(ValidationMessages.RequiredSelect, "Event"));
        }

        [Fact]
        public async Task PostNewTreatmentOutcomeEventNoOutcome_ReturnsValidationError()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFIED_ID;
            var url = GetPathForId(NotificationSubPaths.AddTreatmentEvent, notificationId);
            var document = await GetDocumentForUrlAsync(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["TreatmentEvent.TreatmentEventType"] = ((int)TreatmentEventType.TreatmentOutcome).ToString(),
            };
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            result.AssertValidationErrorResponse();
            var resultDocument = await GetDocumentAsync(result);

            resultDocument.AssertErrorSummaryMessage(
                "SelectedTreatmentOutcomeType",
                "treatmentoutcome-type",
                string.Format(ValidationMessages.RequiredSelect, "Outcome value"));
        }


        [Fact]
        public async Task WhenEmptyOrInvalid_ValidateSelectedTreatmentOutcomeTypeProperty_ReturnsExpectedInvalidResponse()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_OUTCOME;
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                Key = "SelectedTreatmentOutcomeType",
                Value = null
            };
            const string handlerPath = "ValidateSelectedTreatmentOutcomeTypeProperty";
            var notificationSubPath = NotificationSubPaths.AddTreatmentEvent;
            var endpointPath = $"{notificationSubPath}/{handlerPath}";

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetPathForId($"{endpointPath}", id),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(string.Format(ValidationMessages.RequiredSelect, "Outcome value"), result);
        }

        [Fact]
        public async Task WhenValid_ValidateSelectedTreatmentOutcomeTypeProperty_ReturnsExpectedResponse()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_FOR_ADD_TREATMENT_OUTCOME;
            var initialPage = await Client.GetAsync(GetCurrentPathForId(id));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                Key = "SelectedTreatmentOutcomeType",
                Value = ((int)TreatmentOutcomeType.Completed).ToString()
            };
            const string handlerPath = "ValidateSelectedTreatmentOutcomeTypeProperty";
            var notificationSubPath = NotificationSubPaths.AddTreatmentEvent;
            var endpointPath = $"{notificationSubPath}/{handlerPath}";

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetPathForId($"{endpointPath}", id),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTreatmentEvents_DoesNotShowEditLink_ForNonEditableTreatmentEvents()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFICATION_WITH_TREATMENT_EVENTS);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var treatmentTable = document.QuerySelector("#treatment-events");
            Assert.NotNull(treatmentTable);

            var treatmentRestartRow = treatmentTable.QuerySelector($"#treatment-event-{RESTART_TREATMENT_EVENT_ID}");
            Assert.NotNull(treatmentRestartRow);
            Assert.NotNull(treatmentRestartRow.QuerySelector($"#edit-link-{RESTART_TREATMENT_EVENT_ID}"));

            var treatmentTransferInRow = treatmentTable.QuerySelector($"#treatment-event-{TRANSFER_IN_EVENT_ID}");
            Assert.NotNull(treatmentTransferInRow);
            Assert.Null(treatmentTransferInRow.QuerySelector($"#edit-link-{TRANSFER_IN_EVENT_ID}"));
        }
    }
}
