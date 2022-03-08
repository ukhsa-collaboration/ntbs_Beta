using ntbs_service.Helpers;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class SearchStringHelperTest
    {
        [Fact]
        public void CorrectlyHandles_EmptyString()
        {
            //Arrange
            const string searchKeyword = "";

            //Act
            var searchKeywords = SearchStringHelper.GetSearchKeywords(searchKeyword);

            //Assert
            Assert.Empty(searchKeywords);
        }

        [Fact]
        public void CorrectlySanitises_EmptySpaces()
        {
            //Arrange
            const string searchKeyword = " test   ";

            //Act
            var searchKeywords = SearchStringHelper.GetSearchKeywords(searchKeyword);

            //Assert
            Assert.Single(searchKeywords);
            Assert.Equal("test", searchKeywords[0]);
        }
        
        [Fact]
        public void CorrectlySplits_SearchKeywordWithSpaces()
        {
            //Arrange
            const string searchKeyword = "abc def g";

            //Act
            var searchKeywords = SearchStringHelper.GetSearchKeywords(searchKeyword);

            //Assert
            Assert.Equal(3, searchKeywords.Count);
            Assert.Equal("abc", searchKeywords[0]);
            Assert.Equal("def", searchKeywords[1]);
            Assert.Equal("g", searchKeywords[2]);
        }

        [Fact]
        public void MakesSearchKeywords_LowerCase()
        {
            //Arrange
            const string searchKeyword = "AbcDEF GHiJ";
            
            //Act
            var searchKeywords = SearchStringHelper.GetSearchKeywords(searchKeyword);

            //Assert
            Assert.Equal(2, searchKeywords.Count);
            Assert.Equal("abcdef", searchKeywords[0]);
            Assert.Equal("ghij", searchKeywords[1]);
        }
    }
}
