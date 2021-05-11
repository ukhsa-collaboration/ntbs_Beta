import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object SocialContextAddressesScenarioBuilder {
    def buildEditSocialContextAddresses(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_social_context_addresses", "/Notifications/${notificationId}/Edit/SocialContextAddresses")
            .withFormParams(Map(
                "NotificationId" -> "${notificationId}",
                "actionName" -> "Create"))
            .build()
    }

    def buildAddSocialContextAddress(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("add_social_context_address", "/Notifications/${notificationId}/Edit/SocialContextAddress/New")
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
    }
}
