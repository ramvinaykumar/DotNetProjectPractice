namespace HMS.Core.Dtos.Response
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public IEnumerable<T> Data { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
