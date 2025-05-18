namespace TECH.Areas.Admin.Models.Search
{
    public class ThietBiViewModelSearch
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Keyword { get; set; }
        public int? MaPhong { get; set; }
    }
}
