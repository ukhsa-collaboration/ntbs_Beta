
import scala.concurrent.duration._

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class SearchAndRead extends Simulation {

	val httpProtocol = http
		.baseUrl("https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io")
		.inferHtmlResources(WhiteList("""https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io/.*"""), BlackList())

	val headers_0 = Map(
		"accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
		"accept-encoding" -> "gzip, deflate, br",
		"accept-language" -> "en-US,en;q=0.9",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0",
		"sec-fetch-dest" -> "document",
		"sec-fetch-mode" -> "navigate",
		"sec-fetch-site" -> "none",
		"sec-fetch-user" -> "?1",
		"upgrade-insecure-requests" -> "1",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

	val headers_1 = Map(
		"User-Agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0")

	val headers_5 = Map(
		"accept" -> "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8",
		"accept-encoding" -> "gzip, deflate, br",
		"accept-language" -> "en-US,en;q=0.9",
		"pragma" -> "no-cache",
		"sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
		"sec-ch-ua-mobile" -> "?0",
		"sec-fetch-dest" -> "image",
		"sec-fetch-mode" -> "no-cors",
		"sec-fetch-site" -> "same-origin",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

	val headers_6 = Map(
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



	val scn = scenario("SearchAndRead")
		.exec(http("request_0")
			.get("/")
			.headers(headers_0))
		.pause(2)
		.exec(http("request_6")
			.get("/Search")
			.headers(headers_6))
		.pause(20)
		.exec(http("request_11")
			.get("/Search?SearchParameters.IdFilter=301000&SearchParameters.FamilyName=&SearchParameters.PartialDob.Day=&SearchParameters.PartialDob.Month=&SearchParameters.PartialDob.Year=&SearchParameters.Postcode=&SearchParameters.PartialNotificationDate.Day=&SearchParameters.PartialNotificationDate.Month=&SearchParameters.PartialNotificationDate.Year=&SearchParameters.GivenName=&SearchParameters.SexId=&SearchParameters.TBServiceCode=&SearchParameters.CountryId=")
			.headers(headers_6))
		.pause(5)
		.exec(http("request_16")
			.get("/Notifications/301000/")
			.headers(headers_6))

	setUp(scn.inject(atOnceUsers(1))).protocols(httpProtocol)
}