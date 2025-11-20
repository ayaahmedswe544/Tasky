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
        public Task<List<Category>> GetAllCategoriesAsync();
        public Task<bool> IsNameInUseAsync(string CatName);
       
    }
}
