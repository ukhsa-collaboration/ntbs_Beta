import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object EditTravelScenarioBuilder {
    def build(): ChainBuilder = {
        EditScenarioBuilder.getBuilder("edit_travel", "/Notifications/${notificationId}/Edit/Travel")
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
    }
}
