import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.http.check.HttpCheck
import io.gatling.http.request.builder.HttpRequestBuilder

object NtbsRequestBuilder {
    private val defaultChecks: List[HttpCheck] = List(status.is(200))

    def getRequest(name: String, url: String, checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        http(name)
            .get(url)
            .headers(Headers.get_headers)
            .check(List.concat(checks, defaultChecks): _*)
    }

    def postRequest(name: String, url: String, body: String, checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        http(name)
            .post(url)
            .headers(Headers.validate_headers)
            .body(StringBody(body))
            .check(List.concat(checks, defaultChecks): _*)
    }

    def submitRequest(name: String, url: String, formParams: Map[String, String], checks: List[HttpCheck] = List.empty[HttpCheck]): HttpRequestBuilder = {
        var saveForm: HttpRequestBuilder = http(name)
            .post(url)
            .headers(Headers.save_headers)
        for ((key, value) <- formParams) {
            saveForm = saveForm.formParam(key, value)
        }
        saveForm.check(List.concat(checks, defaultChecks): _*)
    }
}
