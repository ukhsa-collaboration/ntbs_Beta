import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object SubmitDraftNotificationScenarioBuilder {
    def build(): ChainBuilder = {
        exec(NtbsRequestBuilder.getRequest(
                "edit_treatment_events_page",
                "/Notifications/${notificationId}/Edit/TreatmentEvents",
                List(css("""input[name="__RequestVerificationToken"]""", "value").saveAs("requestVerificationToken"))))
            .pause(1)
            .exec(NtbsRequestBuilder.submitRequest(
                "submit_draft",
                "/Notifications/${notificationId}/Edit/TreatmentEvents",
                Map(
                    "actionName" -> "Submit",
                    "NotificationId" -> "${notificationId}",
                    "__RequestVerificationToken" -> "${requestVerificationToken}")))
    }
}
