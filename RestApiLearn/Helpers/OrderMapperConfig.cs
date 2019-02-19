using System.Collections.Generic;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Sorting;

namespace RestApiLearn.Helpers
{
    public static class OrderMapperConfig
    {
        public static void Initialize()
        {
            Dictionary<string, OrderDestination> mapConfig = new Dictionary<string, OrderDestination>
            {
                {
                    nameof(AuthorDto.Name).ToLower(),
                    new OrderDestination {Columns = new[] {nameof(Author.FirstName), nameof(Author.LastName)}}
                },
                {
                    nameof(AuthorDto.Age).ToLower(),
                    new OrderDestination {Columns = new[] {nameof(Author.DateOfBirth)}, Reverse = true}
                }
            };
            OrderMapper.Register<AuthorDto, Author>(mapConfig);
        }
    }
}
