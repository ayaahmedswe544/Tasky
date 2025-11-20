using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.VMS.Category;

namespace Tasky.Services.Servs
{
    public class CatServs : ICatServs
    {
        private ICatRepo _catRepo;
        public CatServs(ICatRepo catRepo)
        {
            _catRepo = catRepo;
        }

        public async Task<bool> CreateCategoryAsync(CatVM Cat)
        {
            Category category = new Category
            {
                Name = Cat.Name,
                Description = Cat.Description
            };
            return await _catRepo.CreateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int CatId)
        {
            var cat=await _catRepo.GetCategoryByIdAsync(CatId);
            if (cat != null)
            {
                return await _catRepo.DeleteCategoryAsync(CatId);
            }
            return false;
        }

        public async Task<List<CatVM>> GetAllCategoriesAsync()
        {
            var Cats =await _catRepo.GetAllCategoriesAsync();
            List<CatVM> catVMs = new List<CatVM>();
            foreach (var cat in Cats)
            {
                CatVM catVM = new CatVM
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description
                };
                catVMs.Add(catVM);
            }
            return catVMs;
        }

        public async Task<CatVM?> GetCategoryByIdAsync(int CatId)
        {
            var Cat=await _catRepo.GetCategoryByIdAsync(CatId);
            if (Cat != null)
            {
                CatVM catVM = new CatVM
                {
                    Id = Cat.Id,
                    Name = Cat.Name,
                    Description = Cat.Description
                };
                return catVM;
            }
            return null;
        }

        public async Task<bool> UpdateCategoryAsync(CatVM Cat)
        {
            Category category = new Category
            {
                Id = Cat.Id,
                Name = Cat.Name,
                Description = Cat.Description
            };
            var res = await _catRepo.UpdateCategoryAsync(category);
            return res;

        }
    }
}
