using Microsoft.AspNetCore.Mvc;

namespace BookTrail.API.Shared
{
    public class QueryParameters
    {
        private const int MaxPageSize = 50;
        private readonly int _defaultPageNumber = 1;
        private readonly int _defaultPageSize = 10;

        private int? _pageNumber;
        private int? _pageSize;

        [FromQuery(Name = "p")]
        public int? PageNumber
        {
            get => _pageNumber ?? _defaultPageNumber;
            set => _pageNumber = value;
        }

        [FromQuery(Name = "ps")]
        public int? PageSize
        {
            get => _pageSize.HasValue
                ? _pageSize > MaxPageSize ? MaxPageSize : _pageSize.Value
                : _defaultPageSize;
            set => _pageSize = value;
        }

        [FromQuery(Name = "sb")] public string SortBy { get; set; }

        [FromQuery(Name = "desc")] public bool SortDescending { get; set; }

        public int GetPageNumber()
        {
            return PageNumber ?? _defaultPageNumber;
        }

        public int GetPageSize()
        {
            return PageSize ?? _defaultPageSize;
        }
    }
}