using Tasky.Models;
using Tasky.VMS.Category;

namespace Tasky.Services.IServs
{
    public interface ICatServs
    {
        public Task<bool> CreateCategoryAsync(CreateVM Cat);
        public Task<bool> UpdateCategoryAsync(CatVM Cat);
        public Task<bool> DeleteCategoryAsync(int CatId);
        public Task<CatVM ?> GetCategoryByIdAsync(int CatId);
        public  Task<IEnumerable<CatVM>> GetAllCategoriesAsync(
           string userId,
           string? searchTerm = null,
           string? sortOrder = "asc",
           int pageNumber = 1,
           int pageSize = 10);
        public Task<bool> IsNameInUseAsync(string CatName);
        public Task<int> GetCategoriesCountAsync(string userId,
    string? searchTerm = null
    );

        
    }
}
