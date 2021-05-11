import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object EditComorbiditiesScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_comorbidities", "/Notifications/${notificationId}/Edit/Comorbidities")
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
    }
}
