using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.VMS.Category;

namespace Tasky.Services.Servs
{
    public class CatServs : ICatServs
    {
        private ICatRepo _catRepo;
        private ITaskRepo _taskRepo;
        public CatServs(ICatRepo catRepo,ITaskRepo taskRepo)
        {
            _catRepo = catRepo;
            _taskRepo = taskRepo;

        }

        public async Task<bool> CreateCategoryAsync(CreateVM Cat)
        {
            Category category = new Category
            {
                Name = Cat.Name,
                Description = Cat.Description,
                AppUserId = Cat.AppUserId

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

        public async Task<IEnumerable<CatVM>>GetAllCategoriesAsync(string userId,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var Cats = await _catRepo.GetAllCategoriesAsync(userId,searchTerm,sortOrder,pageNumber,pageSize);
            List<CatVM> catVMs = new List<CatVM>();
            foreach (var cat in Cats)
            {
                var count= await _taskRepo.GetTotalTaskCountAsync(userId, cat.Id, null, false, null);
                CatVM catVM = new CatVM
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description,
                    TaskCount = count
                };
                catVMs.Add(catVM);
            }
            return catVMs;
        }

        public async Task<bool> IsNameInUseAsync(string CatName)
        {
            var res= await _catRepo.IsNameInUseAsync(CatName);
            return res;
        }

        Task<int> ICatServs.GetCategoryCountAsync(string userId, string? searchTerm)
        {
            throw new NotImplementedException();
        }

        async Task<int> ICatServs.GetCategoriesCountAsync(string userId, string? searchTerm)
        {
           int count=await _catRepo.GetCategoriesCountAsync(userId, searchTerm);

            return count;
        }

        //Task<int> ICatServs.GetCategoryCountAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
