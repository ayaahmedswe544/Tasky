using Tasky.Models;
using Tasky.VMS.Category;

namespace Tasky.Services.IServs
{
    public interface ICatServs
    {
        public Task<bool> CreateCategoryAsync(CatVM Cat);
        public Task<bool> UpdateCategoryAsync(CatVM Cat);
        public Task<bool> DeleteCategoryAsync(int CatId);
        public Task<CatVM ?> GetCategoryByIdAsync(int CatId);
        public Task<List<CatVM>> GetAllCategoriesAsync();
    }
}
