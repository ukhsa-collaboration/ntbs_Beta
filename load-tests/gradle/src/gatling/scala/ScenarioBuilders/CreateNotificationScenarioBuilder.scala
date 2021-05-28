import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object CreateNotificationScenarioBuilder {
    def build(): ChainBuilder = {
        new CreateOrEditScenarioBuilder(
            "create_patient_details",
            "/Notifications/${notificationId}/Edit/PatientDetails",
            "/Notifications/Create",
            List(
                css("""input[name="__RequestVerificationToken"]""", "value")
                    .saveAs("requestVerificationToken"),
                currentLocationRegex(s"${Config.urlUnderTest}/Notifications/(.*)/Edit/PatientDetails")
                    .saveAs("notificationId")))
        .withValidations(List(
            "ValidatePatientDetailsProperty" -> """{"value":"9111222333","shouldValidateFull":false,"key":"NhsNumber","NhsNumber":"9111222333"}""",
            "NhsNumberDuplicates" -> """{"notificationId":"${notificationId}","nhsNumber":"${nhsNumber}"}""",
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
            "PatientDetails.NhsNumber" -> "${nhsNumber}",
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
    }
}
