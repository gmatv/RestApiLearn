using RestApiLearn.Entities;
using RestApiLearn.Sorting;
using Xunit;

namespace RestApiLearnTest.Sorting
{
    public class OrderMapperTest: IClassFixture<OrderMapperConfigSetup>
    {

        [Theory]
        [InlineData("genre", "genre asc")]
        [InlineData("genre desc", "genre desc")]
        [InlineData("name asc", "FirstName asc, LastName asc")]
        [InlineData("Name", "FirstName asc, LastName asc")]
        [InlineData("age", "DateOfBirth desc")]
        [InlineData("age desc", "DateOfBirth asc")]
        public void MapToTest(string orderBy, string expected)
        {
            var mapTo = OrderMapper.MapTo<Author>(orderBy);

            Assert.Equal(expected, mapTo, true);
        }
    }
}
