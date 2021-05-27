import scala.concurrent.duration._
import scala.util.Random

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class NtbsLoadTest extends Simulation {

    val httpProtocol = http
        .baseUrl(Config.urlUnderTest)
        .inferHtmlResources(WhiteList(s"""${Config.urlUnderTest}/.*"""), BlackList())

    val notificationFeeder = Iterator.continually(Map("notificationId" -> (300001 + Random.nextInt(10000))))
    val nhsNumberFeeder = Iterator.continually(Map("nhsNumber" -> (9000000001L + Random.nextInt(999999999))))

    val dashboard = DashboardScenarioBuilder.build()
    val searchByFamilyName = SearchScenarioBuilder.buildFamilyNameSearch("Test")
    val searchById = SearchScenarioBuilder.buildIdSearch("${notificationId}")
    val searchByYear = SearchScenarioBuilder.buildYearSearch("1960")
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
        .feed(nhsNumberFeeder)
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
            roundRobinSwitch(searchById, searchByFamilyName, searchByYear),
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
        createFullScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        addOutcomeScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        editContactTracingScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        addTestResultScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        editTravelScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        addSocialContextAddressScenario.inject(constantUsersPerSec(0.0067).during(Config.lengthOfTestInMinutes.minutes).randomized), // 2 per 5 minutes
        readRecentScenario.inject(constantUsersPerSec(0.33).during(Config.lengthOfTestInMinutes.minutes).randomized), // 100 per 5 minutes
        searchAndReadScenario.inject(constantUsersPerSec(0.33).during(Config.lengthOfTestInMinutes.minutes).randomized) // 100 per 5 minutes
    ).protocols(httpProtocol)
}