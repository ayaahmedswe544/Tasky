using Tasky.Models;
using Tasky.VMS.Category;

namespace Tasky.Repositories.IRepos
{
    public interface ICatRepo
    {
        public Task<bool> CreateCategoryAsync(Category Cat);
        public Task<bool> UpdateCategoryAsync(Category Cat);
        public Task<bool> DeleteCategoryAsync(int CatId);
        public Task<Category?> GetCategoryByIdAsync(int CatId);
        public  Task<IEnumerable<Category>> GetAllCategoriesAsync(
            string userId,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10);

        public Task<bool> IsNameInUseAsync(string CatName);

        public Task<int> GetCategoryCountAsync(int id);
        public Task<int> GetCategoriesCountAsync(string userId,
            string? searchTerm = null
            );
    }
}
