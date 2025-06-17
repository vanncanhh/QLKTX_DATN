namespace TECH.Areas.Admin.Models.Search
{
    public class DotDangKyKTXSearch
    {
        public string? Keyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
