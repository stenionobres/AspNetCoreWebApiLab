using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AspNetCoreWebApiLab.Api.Tools
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(",");
            var orderByString = string.Empty;

            // apply each orderby clause in reverse order otherwise, the
            // IQueryable will be ordered in the wrong order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                orderByString = orderByString +
                    (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                    + propertyName
                    + (orderDescending ? " descending" : " ascending");
            }

            return source.OrderBy(orderByString);
        }

        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> source, string filter)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                return source;
            }

            var filterAfterSplit = filter.Split(",");
            var filterString = string.Empty;
            var filterParams = new string[filterAfterSplit.Length];
            var parameterIndex = 0;

            foreach (var filterClause in filterAfterSplit)
            {
                var trimmedFilterClause = filterClause.Trim();
                var parametersSplitedByEqual = trimmedFilterClause.Split("=");
                var propertyName = parametersSplitedByEqual[0].Trim();
                var filterPart = $"{propertyName} = @{parameterIndex}";

                filterString += parameterIndex < filterAfterSplit.Length - 1 
                                ? $"{filterPart} and "
                                : $"{filterPart}";

                filterParams[parameterIndex] = parametersSplitedByEqual[1].Trim();
                parameterIndex++;
            }

            return source.Where(filterString, filterParams);
        }
    }
}
