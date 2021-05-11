import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.core.action.builder.ActionBuilder
import io.gatling.core.structure. { StructureBuilder, ChainBuilder }
import io.gatling.http.check.HttpCheck
import io.gatling.http.request.builder.HttpRequestBuilder

class CreateNotificationScenarioBuilder(
    pageName: String,
    baseUrl: String,
    protected val createUrl: String,
    filters: List[(String, String)] = List.empty[(String, String)],
    validations: List[(String, String)] = List.empty[(String, String)],
    formParams: Map[String, String] = Map.empty[String, String]
) extends CreateOrEditScenarioBuilder(
    pageName,
    baseUrl,
    createUrl,
    List(
        css("""input[name="__RequestVerificationToken"]""", "value").saveAs("requestVerificationToken"),
        currentLocationRegex("https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io/Notifications/(.*)/Edit/PatientDetails").saveAs("notificationId")),
    filters,
    validations,
    formParams)

class EditScenarioBuilder(
    pageName: String,
    baseUrl: String,
    filters: List[(String, String)] = List.empty[(String, String)],
    validations: List[(String, String)] = List.empty[(String, String)],
    formParams: Map[String, String] = Map.empty[String, String]
) extends CreateOrEditScenarioBuilder(
    pageName,
    baseUrl,
    baseUrl,
    List(css("""input[name="__RequestVerificationToken"]""", "value").saveAs("requestVerificationToken")),
    filters,
    validations,
    formParams)

abstract class CreateOrEditScenarioBuilder(
    protected val pageName: String,
    protected val baseUrl: String,
    protected val initialUrl: String,
    protected val initialChecks: List[HttpCheck],
    protected var filters: List[(String, String)] = List.empty[(String, String)],
    protected var validations: List[(String, String)] = List.empty[(String, String)],
    protected var formParams: Map[String, String] = Map.empty[String, String]
) extends NtbsScenarioBuilder {
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
        var action: StructureBuilder[ChainBuilder]  = exec(getRequest(s"${pageName}_page", initialUrl, initialChecks)).pause(1)

        for ((endpoint, queryString) <- filters) {
            action = action.exec(getRequest(s"${pageName}_filter", s"${baseUrl}/${endpoint}?${queryString}"))
        }

        for ((endpoint, body) <- validations) {
            action = action.exec(postRequest(s"${pageName}_validation", s"${baseUrl}/${endpoint}", body)).pause(1)
        }

        action.exec(
                submitRequest(
                    s"${pageName}_save",
                    s"${baseUrl}?isBeingSubmitted=False",
                    formParams + ("__RequestVerificationToken" -> "${requestVerificationToken}")))
            .pause(2)
    }
}

class SearchScenarioBuilder(private val id: String = "", private val familyName: String = "") extends NtbsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        exec(getRequest("search_page", "/Search"))
            .pause(2)
            .exec(getRequest("perform_search", s"/Search?SearchParameters.IdFilter=${id}&SearchParameters.FamilyName=${familyName}&SearchParameters.PartialDob.Day=&SearchParameters.PartialDob.Month=&SearchParameters.PartialDob.Year=&SearchParameters.Postcode=&SearchParameters.PartialNotificationDate.Day=&SearchParameters.PartialNotificationDate.Month=&SearchParameters.PartialNotificationDate.Year=&SearchParameters.GivenName=&SearchParameters.SexId=&SearchParameters.TBServiceCode=&SearchParameters.CountryId="))
            .pause(2)
    }
}

class SubmitDraftNotificationBuilder extends NtbsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        exec(getRequest(
                "edit_treatment_events_page",
                "/Notifications/${notificationId}/Edit/TreatmentEvents",
                List(css("""input[name="__RequestVerificationToken"]""", "value").saveAs("requestVerificationToken"))))
            .pause(1)
            .exec(submitRequest(
                "submit_draft",
                "/Notifications/${notificationId}/Edit/TreatmentEvents",
                Map(
                    "actionName" -> "Submit",
                    "NotificationId" -> "${notificationId}",
                    "__RequestVerificationToken" -> "${requestVerificationToken}")))
    }
}

class DashboardScenarioBuilder extends NtbsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        exec(getRequest("dashboard", "/")).pause(2)
    }
}

class NotificationReadScenarioBuilder extends NtbsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        exec(getRequest("notification_read", "/Notifications/${notificationId}/")).pause(2)
    }
}

abstract class NtbsScenarioBuilder {
    private val defaultChecks: List[HttpCheck] = List(status.is(200))

    def build(): StructureBuilder[ChainBuilder]

    protected def getRequest(name: String, url: String, checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        http(name)
            .get(url)
            .headers(Headers.get_headers)
            .check(List.concat(checks, defaultChecks): _*)
    }

    protected def postRequest(name: String, url: String, body: String, checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        http(name)
            .post(url)
            .headers(Headers.validate_headers)
            .body(StringBody(body))
            .check(List.concat(checks, defaultChecks): _*)
    }

    protected def submitRequest(name: String, url: String, formParams: Map[String, String], checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        var saveForm: HttpRequestBuilder = http(name)
            .post(url)
            .headers(Headers.save_headers)
        for ((key, value) <- formParams) {
            saveForm = saveForm.formParam(key, value)
        }
        saveForm.check(List.concat(checks, defaultChecks): _*)
    }
}

object Headers {
    val get_headers = Map(
		"accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
		"accept-encoding" -> "gzip, deflate, br",
		"accept-language" -> "en-US,en;q=0.9",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0",
		"sec-fetch-dest" -> "document",
		"sec-fetch-mode" -> "navigate",
		"sec-fetch-site" -> "same-origin",
		"sec-fetch-user" -> "?1",
		"upgrade-insecure-requests" -> "1",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

	val validate_headers = Map(
		"accept" -> "application/json, text/plain, */*",
		"accept-encoding" -> "gzip, deflate, br",
		"accept-language" -> "en-US,en;q=0.9",
		"content-type" -> "application/json;charset=UTF-8",
		"origin" -> "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io",
		"requestverificationtoken" -> "${requestVerificationToken}",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0",
		"sec-fetch-dest" -> "empty",
		"sec-fetch-mode" -> "cors",
		"sec-fetch-site" -> "same-origin",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

	val save_headers = Map(
		"accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
		"accept-encoding" -> "gzip, deflate, br",
		"accept-language" -> "en-US,en;q=0.9",
		"origin" -> "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0",
		"sec-fetch-dest" -> "document",
		"sec-fetch-mode" -> "navigate",
		"sec-fetch-site" -> "same-origin",
		"sec-fetch-user" -> "?1",
		"upgrade-insecure-requests" -> "1",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")
}
