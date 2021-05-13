import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object EditSocialRiskFactorsScenarioBuilder {
    def build(): ChainBuilder = {
        EditScenarioBuilder.getBuilder("edit_social_risk_factors", "/Notifications/${notificationId}/Edit/SocialRiskFactors")
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
    }
}
