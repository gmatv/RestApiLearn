using System;
using System.Collections.Generic;
using RestApiLearn.Entities;
using RestApiLearn.Exceptions;

namespace RestApiLearn.Dto.Sorting
{
    /// <summary>
    /// TODO: Make default mapping for same fields
    /// TODO: Unit test
    /// </summary>
    public class AuthorOrder
    {

        private readonly Dictionary<string, OrderDestination> _mapping = new Dictionary<string, OrderDestination>
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

        public string MapToEntities(string orderBy)
        {
            var mappedFields = new List<string>();

            // 1. Split by comma
            var fieldsWithDirections = orderBy.Split(',');

            // 2. Extract direction
            // 3. Map by field
            // 4. Merge to one string
            foreach (var fieldsWithDirection in fieldsWithDirections)
            {
                var parts = fieldsWithDirection.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0 ||parts.Length > 2)
                {
                    ThrowException();
                }

                if (!_mapping.ContainsKey(parts[0]))
                {
                    ThrowException();
                }

                bool ascending;

                if (parts.Length > 1)
                {
                    var part2 = parts[1].ToLower();
                    if (part2.StartsWith("asc"))
                    {
                        ascending = true;
                    }
                    else if (part2.StartsWith("desc"))
                    {
                        ascending = false;
                    }
                    else
                    {
                        ThrowException();
                    }
                }
                else
                {
                    ascending = true;
                }

                string sourceFieldName = parts[0].ToLower();
                OrderDestination orderDestination = _mapping[sourceFieldName];
                if (orderDestination.Reverse)
                {
                    ascending = !ascending;
                }

                foreach (var column in orderDestination.Columns)
                {
                    string destField = column + (ascending ? " asc" : " desc");
                    mappedFields.Add(destField);
                }
            }

            return string.Join(',', mappedFields);

            void ThrowException()
            {
                throw new ValidationException($"Parameter orderBy is incorrect: '{orderBy}'");
            }
        }
    }
}
