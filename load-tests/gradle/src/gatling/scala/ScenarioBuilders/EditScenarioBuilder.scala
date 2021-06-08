import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

object EditScenarioBuilder {
    def getBuilder(pageName: String, baseUrl: String): CreateOrEditScenarioBuilder = {
        new CreateOrEditScenarioBuilder(
            pageName,
            baseUrl,
            baseUrl,
            List(css("""input[name="__RequestVerificationToken"]""", "value").saveAs("requestVerificationToken")))
    }
}
