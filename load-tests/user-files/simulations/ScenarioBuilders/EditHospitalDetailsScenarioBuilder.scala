import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object EditHospitalDetailsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_hospital_details", "/Notifications/${notificationId}/Edit/HospitalDetails")
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
    }
}
