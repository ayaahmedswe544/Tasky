using Tasky.Models;

namespace Tasky.VMS.Category
{
    public class CatListVM
    {
        public List<CatVM> CatsVM { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCats { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchTerm { get; set; }

    

}
}
