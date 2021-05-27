using System.Collections.Generic;
using System.Linq;

namespace KernX.DAL
{
    public readonly struct PagedQuery
    {
        public int Page { get; init; }
        public int Results { get; init; }
        public string OrderBy { get; init; }
        public string SortOrder { get; init; }
    }

    public class PagedResult<T> where T : BaseEntity
    {
        protected PagedResult() => Items = Enumerable.Empty<T>();

        public int CurrentPage { get; init; }
        public int ResultsPerPage { get; init; }
        public int TotalPages { get; init; }
        public long TotalResults { get; init; }

        public IEnumerable<T> Items { get; init; }

        public bool IsEmpty => Items is null || !Items.Any();
        public bool IsNotEmpty => !IsEmpty;

        public static PagedResult<T> Empty => new();

        public static PagedResult<T> Create
            (IEnumerable<T> items, int currentPage, int resultsPerPage, int totalPages, long totalResults)
            => new()
            {
                CurrentPage = currentPage, ResultsPerPage = resultsPerPage, TotalPages = totalPages,
                TotalResults = totalResults, Items = items
            };
    }
}