import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import io.gatling.core.structure.{ StructureBuilder, ChainBuilder }

object SearchScenarioBuilder {
    def build(id: String = "", familyName: String = ""): StructureBuilder[ChainBuilder] = {
        exec(NtbsRequestBuilder.getRequest("search_page", "/Search"))
            .pause(2)
            .exec(NtbsRequestBuilder.getRequest("perform_search", s"/Search?SearchParameters.IdFilter=${id}&SearchParameters.FamilyName=${familyName}&SearchParameters.PartialDob.Day=&SearchParameters.PartialDob.Month=&SearchParameters.PartialDob.Year=&SearchParameters.Postcode=&SearchParameters.PartialNotificationDate.Day=&SearchParameters.PartialNotificationDate.Month=&SearchParameters.PartialNotificationDate.Year=&SearchParameters.GivenName=&SearchParameters.SexId=&SearchParameters.TBServiceCode=&SearchParameters.CountryId="))
            .pause(2)
    }
}
