using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Sorting;
using Xunit;

namespace RestApiLearnTest.Sorting
{
    public class OrderMapperTest
    {
        public OrderMapperTest()
        {
            OrderMapper.Register<AuthorDto, Author>();            
        }

        [Theory]
        [InlineData("genre", "genre asc")]
        public void MapToTest(string orderBy, string expected)
        {
            var mapTo = OrderMapper.MapTo<Author>(orderBy);

            Assert.Equal(expected, mapTo);
        }
    }
}
