import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object EditClinicalDetailsScenarioBuilder {
    def build(): StructureBuilder[ChainBuilder] = {
        EditScenarioBuilder.getBuilder("edit_clinical_details", "/Notifications/${notificationId}/Edit/ClinicalDetails")
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
    }
}
