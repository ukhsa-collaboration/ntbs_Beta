import scala.concurrent.duration._
import scala.util.Random

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class NtbsLoadTest extends Simulation {

    val httpProtocol = http
        .baseUrl("https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io")
        .inferHtmlResources(WhiteList("""https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io/.*"""), BlackList())

    val notificationFeeder = Iterator.continually(Map("notificationId" -> (300001 + Random.nextInt(10000))))

    val dashboard = DashboardScenarioBuilder.build()
    val searchByFamilyName = SearchScenarioBuilder.build(familyName = "Test")
    val searchById = SearchScenarioBuilder.build(id = "${notificationId}")
    val notificationRead = ReadNotificationScenarioBuilder.build()

    val createNotificationWithPatientDetails = CreateNotificationScenarioBuilder.build()
    val editHospitalDetails = EditHospitalDetailsScenarioBuilder.build()
    val editClinicalDetails = EditClinicalDetailsScenarioBuilder.build()

    val editTestResults = TestResultsScenarioBuilder.buildEditTestResults()
    val addNewTestResult = TestResultsScenarioBuilder.buildAddTestResult()

    val editContactTracing = EditContactTracingScenarioBuilder.build()
    val editSocialRiskFactors = EditSocialRiskFactorsScenarioBuilder.build()
    val editTravel = EditTravelScenarioBuilder.build()
    val editComorbidities = EditComorbiditiesScenarioBuilder.build()

    val editSocialContextAddresses = SocialContextAddressesScenarioBuilder.buildEditSocialContextAddresses()
    val addSocialContextAddress = SocialContextAddressesScenarioBuilder.buildAddSocialContextAddress()

    val editTreatmentEvents = TreatmentEventsScenarioBuilder.buildEditTreatmentEvents()
    val addTreatmentEvent = TreatmentEventsScenarioBuilder.buildAddTreatmentEvent()

    val subitDraftNotification = SubmitDraftNotificationScenarioBuilder.build()

    val createFullScenario = scenario("Create")
        .exec(
            searchByFamilyName,
            createNotificationWithPatientDetails,
            editHospitalDetails,
            editClinicalDetails,
            editTestResults,
            addNewTestResult,
            editContactTracing,
            editSocialRiskFactors,
            editTravel,
            editComorbidities,
            editSocialContextAddresses,
            addSocialContextAddress,
            editTreatmentEvents,
            addTreatmentEvent,
            subitDraftNotification)

    val readRecentScenario = scenario("ReadRecent")
        .feed(notificationFeeder)
        .exec(
            dashboard,
            notificationRead)

    val searchAndReadScenario = scenario("SearchAndRead")
        .feed(notificationFeeder)
        .exec(
            dashboard,
            searchById,
            notificationRead)

    val addOutcomeScenario = scenario("AddOutcome")
        .feed(notificationFeeder)
        .exec(
            editTreatmentEvents,
            addTreatmentEvent)

    val editContactTracingScenario = scenario("EditContactTracing")
        .feed(notificationFeeder)
        .exec(editContactTracing)

    val addTestResultScenario = scenario("AddTestResult")
        .feed(notificationFeeder)
        .exec(
            editTestResults,
            addNewTestResult)

    val editTravelScenario = scenario("EditTravel")
        .feed(notificationFeeder)
        .exec(editTravel)

    val addSocialContextAddressScenario = scenario("AddSocialContextAddress")
        .feed(notificationFeeder)
        .exec(
            editSocialContextAddresses,
            addSocialContextAddress)

    setUp(
        createFullScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        addOutcomeScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        editContactTracingScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        addTestResultScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        editTravelScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        addSocialContextAddressScenario.inject(constantUsersPerSec(0.0067).during(5.minutes).randomized), // 2 per 5 minutes
        readRecentScenario.inject(constantUsersPerSec(0.33).during(5.minutes).randomized), // 100 per 5 minutes
        searchAndReadScenario.inject(constantUsersPerSec(0.33).during(5.minutes).randomized) // 100 per 5 minutes
    ).protocols(httpProtocol)
}