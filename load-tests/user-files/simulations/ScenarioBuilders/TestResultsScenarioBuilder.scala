import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object TestResultsScenarioBuilder {
    def buildEditTestResults(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_test_results", "/Notifications/${notificationId}/Edit/TestResults")
            .withValidations(List(
                "ValidateTestDataProperty" -> """{"value":"true","shouldValidateFull":false,"key":"HasTestCarriedOut","HasTestCarriedOut":"true"}"""))
            .withFormParams(Map(
                "NotificationId" -> "${notificationId}",
                "TestData.NotificationId" -> "${notificationId}",
                "TestData.HasTestCarriedOut" -> "true",
                "actionName" -> "Create"))
            .build()
    }

    def buildAddTestResult(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("add_test_result", "/Notifications/${notificationId}/Edit/ManualTestResult/New")
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
    }
}
