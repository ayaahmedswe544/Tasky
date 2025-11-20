using Microsoft.EntityFrameworkCore;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.VMS.Category;

namespace Tasky.Repositories.Repos
{
    public class CatRepo : ICatRepo
    {
        AppDbContext _context;
        public CatRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCategoryAsync(Category Cat)
        {

            await _context.Categories.AddAsync(Cat);
          var res= await _context.SaveChangesAsync();
          
            return res>0;
        }

        public async Task<bool> DeleteCategoryAsync(int CatId)
        {
            var cat=await _context.Categories.FirstOrDefaultAsync(C=>C.Id==CatId);
            if (cat != null) { 
            _context.Categories.Remove(cat);
             var res=await _context.SaveChangesAsync();
                return res > 0;
            }
            return false;

        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
             var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int CatId)
        {
            Category? cat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == CatId);
            if (cat != null)
            {
                return cat;
            }
            return null ;
        }

        public async Task<bool> UpdateCategoryAsync(Category Cat)
        {
            int id = Cat.Id;
            var existingCat =await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(existingCat != null)
            {
              existingCat.Name = Cat.Name;
                existingCat.Description = Cat.Description;
                var res = await _context.SaveChangesAsync();
                return res > 0;
            }
             return false;
        }
        public Task<bool> IsNameInUseAsync(string CatName)
        {
            var cat = _context.Categories.AnyAsync(c => c.Name == CatName);
            return cat;
        }
    }
}
