﻿namespace BookTrail.API.Shared
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}