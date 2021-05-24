import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object DashboardScenarioBuilder {
    def build(): ChainBuilder = {
        exec(NtbsRequestBuilder.getRequest("dashboard", "/")).pause(2)
    }
}
