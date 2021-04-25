using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern.Pagination
{
    public abstract class PaginationUtils
    {
        public static PaginatedList<T> Paginate<T>(List<T> list, PagingParameters pagingParameters)
        {
            if (pagingParameters == null)
            {
                return new PaginatedList<T>(list, list.Count, 1, list.Count, string.Empty);
            }

            if (pagingParameters.PageSize > PagingParameters.MaxPageSize)
            {
                pagingParameters.PageSize = PagingParameters.MaxPageSize;
            }

            var totalCount = list.Count;
            var items = list.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize);
            return new PaginatedList<T>(items.ToList(), totalCount, pagingParameters.PageNumber, pagingParameters.PageSize,
                pagingParameters.GetRoute());
        }
        
        public static PaginatedList<TConverted> ConvertItems<TFrom, TConverted>(PaginatedList<TFrom> from, Func<TFrom, TConverted> convertFunc)
        {
            var convertedItems = from.Items.Select(convertFunc.Invoke);
            var converted = new PaginatedList<TConverted>(convertedItems.ToList(), from.TotalRecords, from.CurrentPage, from.PageSize, from.Route);
            return converted;
        }
    }
}