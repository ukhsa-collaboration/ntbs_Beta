import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object TreatmentEventsScenarioBuilder {
    def buildEditTreatmentEvents(): ChainBuilder = {
        EditScenarioBuilder.getBuilder("edit_treatment_events", "/Notifications/${notificationId}/Edit/TreatmentEvents")
            .withFormParams(Map(
                "NotificationId" -> "${notificationId}",
                "actionName" -> "Create"))
            .build()
    }

    def buildAddTreatmentEvent(): ChainBuilder = {
        EditScenarioBuilder.getBuilder("add_treatment_event", "/Notifications/${notificationId}/Edit/TreatmentEvent/New")
            .withValidations(List(
                "ValidateTreatmentEventDate" -> """{"day":"05","month":"05","year":"2021","key":"EventDate"}""",
                "ValidateTreatmentEventProperty" -> """{"value":"0","shouldValidateFull":false,"key":"TreatmentEventType","TreatmentEventType":"0"}"""))
            .withFormParams(Map(
                "TreatmentEvent.TreatmentOutcomeId" -> "",
                "TreatmentEvent.CaseManagerId" -> "",
                "TreatmentEvent.TbServiceCode" -> "",
                "FormattedEventDate.Day" -> "05",
                "FormattedEventDate.Month" -> "05",
                "FormattedEventDate.Year" -> "2021",
                "TreatmentEvent.TreatmentEventType" -> "1",
                "SelectedTreatmentOutcomeType" -> "6",
                "SelectedTreatmentOutcomeSubType" -> "11",
                "TreatmentEvent.Note" -> ""))
            .build()
    }
}
