import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object EditContactTracingScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_contact_tracing", "/Notifications/${notificationId}/Edit/ContactTracing")
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
    }
}
