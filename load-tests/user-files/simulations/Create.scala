import scala.concurrent.duration._

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._

class Create extends Simulation {

    val httpProtocol = http
        .baseUrl("https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io")
        .inferHtmlResources(WhiteList("""https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io/.*"""), BlackList())

    val notificationFeeder = Iterator.continually(Map("notificationId" -> (300001 + Random.nextInt(10000))))

    val searchByFamilyName = new SearchScenarioBuilder(familyName = "Test").build()
    val searchById = new SearchScenarioBuilder(id = "${notificationId}").build()

    val createNotificationWithPatientDetails = new CreateNotificationScenarioBuilder(
            "create_patient_details",
            "/Notifications/${notificationId}/Edit/PatientDetails",
            "/Notifications/Create")
        .withValidations(List(
            "ValidatePatientDetailsProperty" -> """{"value":"9111222333","shouldValidateFull":false,"key":"NhsNumber","NhsNumber":"9111222333"}""",
            "NhsNumberDuplicates" -> """{"notificationId":"${notificationId}","nhsNumber":"9111222333"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"Test","shouldValidateFull":false,"key":"GivenName","GivenName":"Test"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"Testerson","shouldValidateFull":false,"key":"FamilyName","FamilyName":"Testerson"}""",
            "ValidatePatientDetailsDate" -> """{"day":"01","month":"02","year":"1933","key":"Dob"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"1","shouldValidateFull":false,"key":"SexId","SexId":"1"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"1","shouldValidateFull":false,"key":"EthnicityId","EthnicityId":"1"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"235","shouldValidateFull":false,"key":"CountryId","CountryId":"235"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"123 Fake Street","shouldValidateFull":false,"key":"Address","Address":"123 Fake Street"}""",
            "ValidatePostcode" -> """{"shouldValidateFull":false,"postcode":"BS1 1PN"}""",
            "ValidatePatientDetailsProperty" -> """{"value":"5","shouldValidateFull":false,"key":"OccupationId","OccupationId":"5"}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "PatientDetails.NhsNumberNotKnown" -> "false",
            "PatientDetails.NhsNumber" -> "9111222333",
            "PatientDetails.GivenName" -> "Test",
            "PatientDetails.FamilyName" -> "Testerson",
            "FormattedDob.Day" -> "01",
            "FormattedDob.Month" -> "02",
            "FormattedDob.Year" -> "1933",
            "PatientDetails.SexId" -> "1",
            "PatientDetails.EthnicityId" -> "1",
            "PatientDetails.LocalPatientId" -> "",
            "PatientDetails.CountryId" -> "235",
            "PatientDetails.YearOfUkEntry" -> "",
            "PatientDetails.Address" -> "123 Fake Street",
            "PatientDetails.NoFixedAbode" -> "false",
            "PatientDetails.Postcode" -> "BS1 1PN",
            "PatientDetails.OccupationId" -> "5",
            "PatientDetails.OccupationOther" -> "",
            "actionName" -> "Save"))
        .build()

    val editHospitalDetails = new EditScenarioBuilder("edit_hospital_details", "/Notifications/${notificationId}/Edit/HospitalDetails")
        .withFilters(List(
            "GetFilteredListsByTbService" -> "value=TBS0357",
            "GetFilteredListsByTbService" -> "value=TBS0028"))
        .withValidations(List(
            "ValidateNotificationDate" -> """{"day":"05","month":"05","year":"2021","key":"NotificationDate","notificationId":"${notificationId}"}""",
            "ValidateHospitalDetailsProperty" -> """{"value":"TBS0028","shouldValidateFull":false,"key":"TBServiceCode","TBServiceCode":"TBS0028"}""",
            "ValidateHospitalDetailsProperty" -> """{"value":"bcfc88e8-ead4-4e40-9d7e-be7896adbd4a","shouldValidateFull":false,"key":"HospitalId","HospitalId":"bcfc88e8-ead4-4e40-9d7e-be7896adbd4a"}""",
            "ValidateHospitalDetailsProperty" -> """{"value":null,"shouldValidateFull":false,"key":"Consultant","Consultant":""}""",
            "ValidateHospitalDetailsProperty" -> """{"value":"1158","shouldValidateFull":false,"key":"CaseManagerId","CaseManagerId":"1158"}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "FormattedNotificationDate.Day" -> "05",
            "FormattedNotificationDate.Month" -> "05",
            "FormattedNotificationDate.Year" -> "2021",
            "HospitalDetails.TBServiceCode" -> "TBS0028",
            "HospitalDetails.HospitalId" -> "bcfc88e8-ead4-4e40-9d7e-be7896adbd4a",
            "HospitalDetails.Consultant" -> "",
            "HospitalDetails.CaseManagerId" -> "1158",
            "actionName" -> "Save"))
        .build()

    val editClinicalDetails = new EditScenarioBuilder("edit_clinical_details", "/Notifications/${notificationId}/Edit/ClinicalDetails")
        .withFilters(List(
            "ValidateNotificationSites" -> "valueList%5B0%5D=PULMONARY&shouldValidateFull=False",
            "ValidateNotificationSites" -> "valueList%5B0%5D=PULMONARY&valueList%5B1%5D=BONE_SPINE&shouldValidateFull=False"))
        .withValidations(List(
            "ValidateClinicalDetailsYearComparison" -> """{"newYear":"2001","shouldValidateFull":"False","existingYear":"1933","propertyName":"BCG vaccination year"}""",
            "ValidateClinicalDetailsDate" -> """{"day":"01","month":"05","year":"2021","key":"FirstPresentationDate"}""",
            "ValidateClinicalDetailsDate" -> """{"day":"02","month":"05","year":"2021","key":"TBServicePresentationDate"}""",
            "ValidateClinicalDetailsDate" -> """{"day":"05","month":"05","year":"2021","key":"DiagnosisDate"}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "PatientBirthYear" -> "1933",
            "NotificationSiteMap[PULMONARY]" -> "true",
            "NotificationSiteMap[LARYNGEAL]" -> "false",
            "NotificationSiteMap[MILIARY]" -> "false",
            "NotificationSiteMap[BONE_SPINE]" -> "true",
            "NotificationSiteMap[BONE_OTHER]" -> "false",
            "NotificationSiteMap[CNS_MENINGITIS]" -> "false",
            "NotificationSiteMap[CNS_OTHER]" -> "false",
            "NotificationSiteMap[LYMPH_INTRA]" -> "false",
            "NotificationSiteMap[LYMPH_EXTRA]" -> "false",
            "NotificationSiteMap[CRYPTIC]" -> "false",
            "NotificationSiteMap[GASTROINTESTINAL]" -> "false",
            "NotificationSiteMap[GENITOURINARY]" -> "false",
            "NotificationSiteMap[OCULAR]" -> "false",
            "NotificationSiteMap[PLEURAL]" -> "false",
            "NotificationSiteMap[PERICARDIAL]" -> "false",
            "NotificationSiteMap[SKIN]" -> "false",
            "NotificationSiteMap[OTHER]" -> "false",
            "OtherSite.SiteId" -> "17",
            "OtherSite.SiteDescription" -> "",
            "ClinicalDetails.BCGVaccinationState" -> "Yes",
            "ClinicalDetails.BCGVaccinationYear" -> "2001",
            "ClinicalDetails.HIVTestState" -> "0",
            "FormattedSymptomDate.Day" -> "",
            "FormattedSymptomDate.Month" -> "",
            "FormattedSymptomDate.Year" -> "",
            "ClinicalDetails.IsSymptomatic" -> "false",
            "FormattedFirstPresentationDate.Day" -> "01",
            "FormattedFirstPresentationDate.Month" -> "05",
            "FormattedFirstPresentationDate.Year" -> "2021",
            "ClinicalDetails.HealthcareSetting" -> "ContactTracing",
            "ClinicalDetails.HealthcareDescription" -> "",
            "FormattedTbServicePresentationDate.Day" -> "02",
            "FormattedTbServicePresentationDate.Month" -> "05",
            "FormattedTbServicePresentationDate.Year" -> "2021",
            "FormattedDiagnosisDate.Day" -> "05",
            "FormattedDiagnosisDate.Month" -> "05",
            "FormattedDiagnosisDate.Year" -> "2021",
            "FormattedTreatmentDate.Day" -> "",
            "FormattedTreatmentDate.Month" -> "",
            "FormattedTreatmentDate.Year" -> "",
            "ClinicalDetails.DidNotStartTreatment" -> "true",
            "FormattedHomeVisitDate.Day" -> "",
            "FormattedHomeVisitDate.Month" -> "",
            "FormattedHomeVisitDate.Year" -> "",
            "ClinicalDetails.HomeVisitCarriedOut" -> "No",
            "ClinicalDetails.IsPostMortem" -> "false",
            "ClinicalDetails.IsDotOffered" -> "Yes",
            "ClinicalDetails.DotStatus" -> "DotReceived",
            "ClinicalDetails.EnhancedCaseManagementStatus" -> "No",
            "ClinicalDetails.TreatmentRegimen" -> "StandardTherapy",
            "FormattedMdrTreatmentDate.Day" -> "",
            "FormattedMdrTreatmentDate.Month" -> "",
            "FormattedMdrTreatmentDate.Year" -> "",
            "ClinicalDetails.TreatmentRegimenOtherDescription" -> "",
            "ClinicalDetails.Notes" -> "",
            "actionName" -> "Save"))
        .build()

    val editTestResults = new EditScenarioBuilder("edit_test_results", "/Notifications/${notificationId}/Edit/TestResults")
        .withValidations(List(
            "ValidateTestDataProperty" -> """{"value":"true","shouldValidateFull":false,"key":"HasTestCarriedOut","HasTestCarriedOut":"true"}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "TestData.NotificationId" -> "${notificationId}",
            "TestData.HasTestCarriedOut" -> "true",
            "actionName" -> "Create"))
        .build()

    val addNewTestResult = new EditScenarioBuilder("add_test_result", "/Notifications/${notificationId}/Edit/ManualTestResult/New")
        .withFilters(List(
            "FilteredSampleTypesForManualTestType" -> "value=4"))
        .withValidations(List(
            "ValidateTestResultForEditDate" -> """{"day":"05","month":"05","year":"2021","key":"TestDate"}"""))
        .withFormParams(Map(
            "FormattedTestDate.Day" -> "05",
            "FormattedTestDate.Month" -> "05",
            "FormattedTestDate.Year" -> "2021",
            "TestResultForEdit.ManualTestTypeId" -> "4",
            "TestResultForEdit.SampleTypeId" -> "",
            "TestResultForEdit.Result" -> "ConsistentWithTbOther"))
        .build()

    val editContactTracing = new EditScenarioBuilder("edit_contact_tracing", "/Notifications/${notificationId}/Edit/ContactTracing")
        .withValidations(List(
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":null,"adultsScreened":null,"childrenScreened":null,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":null,"childrenActiveTb":null,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":null,"childrenScreened":null,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":null,"childrenActiveTb":null,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":null,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":null,"childrenActiveTb":null,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":null,"childrenActiveTb":null,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":0,"childrenActiveTb":null,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":null,"childrenLatentTb":null,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":null,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":0,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":null,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":0,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":0,"childrenStartedTreatment":null,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":0,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":0,"childrenStartedTreatment":0,"adultsFinishedTreatment":null,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":0,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":0,"childrenStartedTreatment":0,"adultsFinishedTreatment":0,"childrenFinishedTreatment":null}""",
            "ValidateContactTracing" -> """{"adultsIdentified":4,"childrenIdentified":1,"adultsScreened":0,"childrenScreened":0,"adultsLatentTb":0,"childrenLatentTb":0,"adultsActiveTb":0,"childrenActiveTb":0,"adultsStartedTreatment":0,"childrenStartedTreatment":0,"adultsFinishedTreatment":0,"childrenFinishedTreatment":0}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "ContactTracing.AdultsIdentified" -> "4",
            "ContactTracing.ChildrenIdentified" -> "1",
            "ContactTracing.AdultsScreened" -> "0",
            "ContactTracing.ChildrenScreened" -> "0",
            "ContactTracing.AdultsActiveTB" -> "0",
            "ContactTracing.ChildrenActiveTB" -> "0",
            "ContactTracing.AdultsLatentTB" -> "0",
            "ContactTracing.ChildrenLatentTB" -> "0",
            "ContactTracing.AdultsStartedTreatment" -> "0",
            "ContactTracing.ChildrenStartedTreatment" -> "0",
            "ContactTracing.AdultsFinishedTreatment" -> "0",
            "ContactTracing.ChildrenFinishedTreatment" -> "0",
            "actionName" -> "Save"))
        .build()

    val editSocialRiskFactors = new EditScenarioBuilder("edit_social_risk_factors", "/Notifications/${notificationId}/Edit/SocialRiskFactors")
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "SocialRiskFactors.AlcoholMisuseStatus" -> "No",
            "SocialRiskFactors.RiskFactorDrugs.IsCurrentView" -> "false",
            "SocialRiskFactors.RiskFactorDrugs.InPastFiveYearsView" -> "false",
            "SocialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgoView" -> "false",
            "SocialRiskFactors.RiskFactorDrugs.Status" -> "No",
            "SocialRiskFactors.RiskFactorHomelessness.IsCurrentView" -> "false",
            "SocialRiskFactors.RiskFactorHomelessness.InPastFiveYearsView" -> "false",
            "SocialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgoView" -> "false",
            "SocialRiskFactors.RiskFactorHomelessness.Status" -> "No",
            "SocialRiskFactors.RiskFactorImprisonment.Status" -> "Yes",
            "SocialRiskFactors.RiskFactorImprisonment.IsCurrentView" -> "false",
            "SocialRiskFactors.RiskFactorImprisonment.InPastFiveYearsView" -> "false",
            "SocialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgoView" -> "true",
            "SocialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgoView" -> "false",
            "SocialRiskFactors.MentalHealthStatus" -> "No",
            "SocialRiskFactors.RiskFactorSmoking.Status" -> "Yes",
            "SocialRiskFactors.RiskFactorSmoking.IsCurrentView" -> "false",
            "SocialRiskFactors.RiskFactorSmoking.InPastFiveYearsView" -> "true",
            "SocialRiskFactors.RiskFactorSmoking.InPastFiveYearsView" -> "false",
            "SocialRiskFactors.RiskFactorSmoking.MoreThanFiveYearsAgoView" -> "false",
            "SocialRiskFactors.AsylumSeekerStatus" -> "No",
            "SocialRiskFactors.ImmigrationDetaineeStatus" -> "No",
            "actionName" -> "Save"))
        .build()

    val editTravel = new EditScenarioBuilder("edit_travel", "/Notifications/${notificationId}/Edit/Travel")
        .withValidations(List(
            "Validatetravel" -> """{"totalNumberOfCountries":null,"country1Id":null,"country2Id":null,"country3Id":null,"stayLengthInMonths1":null,"stayLengthInMonths2":null,"stayLengthInMonths3":null,"shouldValidateFull":false,"hasTravel":"No"}""",
            "Validatevisitor" -> """{"totalNumberOfCountries":null,"country1Id":null,"country2Id":null,"country3Id":null,"stayLengthInMonths1":null,"stayLengthInMonths2":null,"stayLengthInMonths3":null,"shouldValidateFull":false,"hasVisitor":"No"}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "TravelDetails.TotalNumberOfCountries" -> "",
            "TravelDetails.Country1Id" -> "",
            "TravelDetails.StayLengthInMonths1" -> "",
            "TravelDetails.Country2Id" -> "",
            "TravelDetails.StayLengthInMonths2" -> "",
            "TravelDetails.Country3Id" -> "",
            "TravelDetails.StayLengthInMonths3" -> "",
            "TravelDetails.HasTravel" -> "No",
            "VisitorDetails.TotalNumberOfCountries" -> "",
            "VisitorDetails.Country1Id" -> "",
            "VisitorDetails.StayLengthInMonths1" -> "",
            "VisitorDetails.Country2Id" -> "",
            "VisitorDetails.StayLengthInMonths2" -> "",
            "VisitorDetails.Country3Id" -> "",
            "VisitorDetails.StayLengthInMonths3" -> "",
            "VisitorDetails.HasVisitor" -> "No",
            "actionName" -> "Save"))
        .build()

    val editComorbidities = new EditScenarioBuilder("edit_comorbidities", "/Notifications/${notificationId}/Edit/Comorbidities")
        .withValidations(List(
            "ValidateImmunosuppression" -> """{"status":"No","hasBioTherapy":false,"hasTransplantation":false,"hasOther":false,"otherDescription":""}"""))
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "ComorbidityDetails.DiabetesStatus" -> "No",
            "ComorbidityDetails.HepatitisBStatus" -> "No",
            "ComorbidityDetails.HepatitisCStatus" -> "No",
            "ComorbidityDetails.LiverDiseaseStatus" -> "No",
            "ComorbidityDetails.RenalDiseaseStatus" -> "No",
            "HasBioTherapy" -> "false",
            "HasTransplantation" -> "false",
            "HasOther" -> "false",
            "OtherDescription" -> "",
            "ImmunosuppressionStatus" -> "No",
            "actionName" -> "Save"))
        .build()

    val editSocialContextAddresses = new EditScenarioBuilder("edit_social_context_addresses", "/Notifications/${notificationId}/Edit/SocialContextAddresses")
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "actionName" -> "Create"))
        .build()

    val addSocialContextAddress = new EditScenarioBuilder("add_social_context_address", "/Notifications/${notificationId}/Edit/SocialContextAddress/New")
        .withValidations(List(
            "ValidateSocialContextProperty" -> """{"value":"124 Fake Street","shouldValidateFull":false,"key":"Address","Address":"124 Fake Street"}""",
            "ValidateSocialContextProperty" -> """{"value":"BS1 1PN","shouldValidateFull":false,"key":"Postcode","Postcode":"BS1 1PN"}""",
            "ValidateSocialContextDate" -> """{"day":"01","month":"01","year":"2015","key":"DateFrom"}"""))
        .withFormParams(Map(
            "Address.Address" -> "124 Fake Street",
            "Address.Postcode" -> "BS1 1PN",
            "FormattedDateFrom.Day" -> "01",
            "FormattedDateFrom.Month" -> "01",
            "FormattedDateFrom.Year" -> "2015",
            "FormattedDateTo.Day" -> "31",
            "FormattedDateTo.Month" -> "12",
            "FormattedDateTo.Year" -> "2020",
            "Address.Details" -> ""))
        .build()

    val editTreatmentEvents = new EditScenarioBuilder("edit_treatment_events", "/Notifications/${notificationId}/Edit/TreatmentEvents")
        .withFormParams(Map(
            "NotificationId" -> "${notificationId}",
            "actionName" -> "Create"))
        .build()

    val addTreatmentEvent = new EditScenarioBuilder("add_treatment_event", "/Notifications/${notificationId}/Edit/TreatmentEvent/New")
        .withValidations(List(
            "ValidateTreatmentEventDate" -> """{"day":"05","month":"05","year":"2021","key":"EventDate"}""",
            "ValidateTreatmentEventProperty" -> """{"value":"0","shouldValidateFull":false,"key":"TreatmentEventType","TreatmentEventType":"0"}"""))
        .withFormParams(Map(
            "TreatmentEvent.TreatmentOutcomeId" -> "",
            "TreatmentEvent.CaseManagerId" -> "",
            "TreatmentEvent.TbServiceCode" -> "",
            "FormattedEventDate.Day" -> "05",
            "FormattedEventDate.Month" -> "05",
            "FormattedEventDate.Year" -> "2021",
            "TreatmentEvent.TreatmentEventType" -> "1",
            "SelectedTreatmentOutcomeType" -> "6",
            "SelectedTreatmentOutcomeSubType" -> "11",
            "TreatmentEvent.Note" -> ""))
        .build()

    val subitDraftNotification = new SubmitDraftNotificationBuilder().build()

    val dashboard = new DashboardScenarioBuilder().build()
    val notificationRead = new NotificationReadScenarioBuilder().build()

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
        createFullScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        addOutcomeScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        editContactTracingScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        addTestResultScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        editTravelScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        addSocialContextAddressScenario.inject(constantUsersPerSec(0.0067).during(1.minutes).randomized), // 2 per 5 minutes
        readRecentScenario.inject(constantUsersPerSec(0.33).during(1.minutes).randomized), // 100 per 5 minutes
        searchAndReadScenario.inject(constantUsersPerSec(0.33).during(1.minutes).randomized) // 100 per 5 minutes
    ).protocols(httpProtocol)
}