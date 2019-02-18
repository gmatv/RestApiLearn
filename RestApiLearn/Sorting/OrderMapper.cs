using System;
using System.Collections.Generic;
using System.Linq;
using RestApiLearn.Exceptions;

namespace RestApiLearn.Sorting
{
    public static class OrderMapper
    {
        private static Dictionary<Type, Dictionary<string, OrderDestination>> _typeMap =
            new Dictionary<Type, Dictionary<string, OrderDestination>>();

        public static void Register<TSource, TDestination>()
        {
            var map = new Dictionary<string, OrderDestination>();
            var sourceNames = typeof(TSource).GetProperties().Select(p => p.Name.ToLower());
            var destNames = typeof(TDestination).GetProperties().Select(p => p.Name.ToLower()).ToList();
            foreach (var sourceName in sourceNames)
            {
                if (destNames.Contains(sourceName))
                {
                    var orderDestination = new OrderDestination
                    {
                        Columns = new[] {sourceName}
                    };
                    map.Add(sourceName, orderDestination);
                }
            }

            _typeMap.Add(typeof(TDestination), map);
        }

        public static string MapTo<TDestination>(string orderBy)
        {

            if (!_typeMap.TryGetValue(typeof(TDestination), out Dictionary<string, OrderDestination> map))
            {
                ThrowException();
            }

            var mappedFields = new List<string>();

            // 1. Split by comma
            var fieldsWithDirections = orderBy.Split(',');

            // 2. Extract direction
            // 3. Map by field
            // 4. Merge to one string
            foreach (var fieldsWithDirection in fieldsWithDirections)
            {
                var parts = fieldsWithDirection.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0 || parts.Length > 2)
                {
                    ThrowException();
                }

                if (!map.ContainsKey(parts[0]))
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
                OrderDestination orderDestination = map[sourceFieldName];
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
