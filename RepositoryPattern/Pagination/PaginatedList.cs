using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RepositoryPattern.Pagination
{
    public class Links
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Previous { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Next { get; set; }
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string Route { get; set; }
        public Links Links { get; set; }
        public bool HasPreviousPage => Items.Count > 0 && CurrentPage > 1;
        public bool HasNextPage => (CurrentPage < TotalPages);

        public PaginatedList(List<T> items, int totalRecords, int pageNumber, int pageSize, string route)
        {
            TotalRecords = totalRecords;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int) Math.Ceiling(totalRecords / (double) pageSize);
            Items.AddRange(items);

            if (route != null)
            {
                Route = route;
                Links = new Links()
                {
                    Previous = HasPreviousPage ? $"{route}?PageNumber={CurrentPage - 1}&Route=1&PageSize={pageSize}" : null,
                    Next = HasNextPage ? $"{route}?PageNumber={CurrentPage + 1}&Route=1&PageSize={pageSize}" : null
                };
            }
        }

        // public static PaginatedList<T> Get(List<T> list, PagingParameters pagingParameters)
        // {
        //     if (pagingParameters == null)
        //     {
        //         return new PaginatedList<T>(list, list.Count, 1, list.Count, string.Empty);
        //     }
        //
        //     if (pagingParameters.PageSize > PagingParameters.MaxPageSize)
        //     {
        //         pagingParameters.PageSize = PagingParameters.MaxPageSize;
        //     }
        //
        //     var totalCount = list.Count;
        //     var items = list.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize);
        //     return new PaginatedList<T>(items.ToList(), totalCount, pagingParameters.PageNumber, pagingParameters.PageSize,
        //         pagingParameters.Route);
        // }

        // public static PaginatedList<TConverted> Convert<TFrom, TConverted>(PaginatedList<TFrom> from, Func<TFrom, TConverted> convertFunc)
        // {
        //     var convertedItems = from.Items.Select(convertFunc.Invoke);
        //     var converted = new PaginatedList<TConverted>(convertedItems.ToList(), from.TotalRecords, from.CurrentPage, from.PageSize, from.Route);
        //     return converted;
        // }
    }
}