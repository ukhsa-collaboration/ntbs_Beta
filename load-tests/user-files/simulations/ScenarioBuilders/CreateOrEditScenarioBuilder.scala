import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }
import io.gatling.http.check.HttpCheck

class CreateOrEditScenarioBuilder(
    protected val pageName: String,
    protected val baseUrl: String,
    protected val initialUrl: String,
    protected val initialChecks: List[HttpCheck],
    protected var filters: List[(String, String)] = List.empty[(String, String)],
    protected var validations: List[(String, String)] = List.empty[(String, String)],
    protected var formParams: Map[String, String] = Map.empty[String, String]
) {
    def withFilters(_filters: List[(String, String)]): CreateOrEditScenarioBuilder = {
        filters = _filters
        this
    }

    def withValidations(_validations: List[(String, String)]): CreateOrEditScenarioBuilder = {
        validations = _validations
        this
    }

    def withFormParams(_formParams: Map[String, String]): CreateOrEditScenarioBuilder = {
        formParams = _formParams
        this
    }

    def build(): StructureBuilder[ChainBuilder]  = {
        var action: StructureBuilder[ChainBuilder]  = exec(NtbsRequestBuilder.getRequest(s"${pageName}_page", initialUrl, initialChecks)).pause(1)

        for ((endpoint, queryString) <- filters) {
            action = action.exec(NtbsRequestBuilder.getRequest(s"${pageName}_filter", s"${baseUrl}/${endpoint}?${queryString}"))
        }

        for ((endpoint, body) <- validations) {
            action = action.exec(NtbsRequestBuilder.postRequest(s"${pageName}_validation", s"${baseUrl}/${endpoint}", body)).pause(1)
        }

        action.exec(
                NtbsRequestBuilder.submitRequest(
                    s"${pageName}_save",
                    s"${baseUrl}?isBeingSubmitted=False",
                    formParams + ("__RequestVerificationToken" -> "${requestVerificationToken}")))
            .pause(2)
    }
}
