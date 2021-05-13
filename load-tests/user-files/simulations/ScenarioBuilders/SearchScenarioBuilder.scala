import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.ChainBuilder

object SearchScenarioBuilder {
    def buildIdSearch(id: String): ChainBuilder = {
        build("perform_id_search", id = id)
    }

    def buildFamilyNameSearch(familyName: String): ChainBuilder = {
        build("perform_family_name_search", familyName = familyName)
    }

    def buildYearSearch(year: String): ChainBuilder = {
        build("perform_year_search", year = year)
    }

    private def build(requestName: String, id: String = "", familyName: String = "", year: String = ""): ChainBuilder = {
        exec(NtbsRequestBuilder.getRequest("search_page", "/Search"))
            .pause(2)
            .exec(NtbsRequestBuilder.getRequest(requestName, s"/Search?SearchParameters.IdFilter=${id}&SearchParameters.FamilyName=${familyName}&SearchParameters.PartialDob.Day=&SearchParameters.PartialDob.Month=&SearchParameters.PartialDob.Year=${year}&SearchParameters.Postcode=&SearchParameters.PartialNotificationDate.Day=&SearchParameters.PartialNotificationDate.Month=&SearchParameters.PartialNotificationDate.Year=&SearchParameters.GivenName=&SearchParameters.SexId=&SearchParameters.TBServiceCode=&SearchParameters.CountryId="))
            .pause(2)
    }
}
