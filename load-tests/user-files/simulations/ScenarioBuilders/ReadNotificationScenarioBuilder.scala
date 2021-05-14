import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object ReadNotificationScenarioBuilder {
    def build(): ChainBuilder = {
        exec(NtbsRequestBuilder.getRequest("notification_read", "/Notifications/${notificationId}/")).pause(2)
    }
}
