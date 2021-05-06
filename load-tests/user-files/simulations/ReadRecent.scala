
import scala.concurrent.duration._
import scala.util.Random

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class ReadRecent extends Simulation {

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
		"sec-fetch-site" -> "same-origin",
		"sec-fetch-user" -> "?1",
		"upgrade-insecure-requests" -> "1",
		"user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

	val notificationFeeder = Iterator.continually(Map("notificationId" -> (300001 + Random.nextInt(100))))

	val readRecentScenario = scenario("ReadRecent")
		.exec(http("dashboard")
			.get("/")
			.headers(headers_0)
			.check(status.is(200)))
		.pause(4)
		.feed(notificationFeeder)
		.exec(http("notification_read")
			.get("/Notifications/${notificationId}/")
			.headers(headers_0)
			.check(status.is(200)))

	val searchAndReadScenario = scenario("SearchAndRead")
		.exec(http("dashboard")
			.get("/")
			.headers(headers_0)
			.check(status.is(200)))
		.pause(2)
		.exec(http("search_page")
			.get("/Search")
			.headers(headers_0)
			.check(status.is(200)))
		.pause(5)
		.exec(http("perform_search")
			.get("/Search?SearchParameters.IdFilter=301000&SearchParameters.FamilyName=&SearchParameters.PartialDob.Day=&SearchParameters.PartialDob.Month=&SearchParameters.PartialDob.Year=&SearchParameters.Postcode=&SearchParameters.PartialNotificationDate.Day=&SearchParameters.PartialNotificationDate.Month=&SearchParameters.PartialNotificationDate.Year=&SearchParameters.GivenName=&SearchParameters.SexId=&SearchParameters.TBServiceCode=&SearchParameters.CountryId=")
			.headers(headers_0)
			.check(status.is(200)))
		.pause(5)
		.exec(http("notification_read")
			.get("/Notifications/301000/")
			.headers(headers_0)
			.check(status.is(200)))

	setUp(
		readRecentScenario.inject(constantUsersPerSec(0.33).during(1.minutes).randomized),
		searchAndReadScenario.inject(constantUsersPerSec(0.33).during(1.minutes).randomized)
	).protocols(httpProtocol)
}