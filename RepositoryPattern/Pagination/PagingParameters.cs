#nullable enable
namespace RepositoryPattern.Pagination
{
    public class PagingParameters
    {
        public const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        private string Route { get; set; } = string.Empty;

        public void WithRoute(string route)
        {
            Route = route;
        }

        public string GetRoute()
        {
            return Route;
        }
    }
}