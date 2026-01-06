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
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Categories.Where(c => c.AppUserId == userId).ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm)).ToList();
            }
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(c => c.Name).ToList();
            }
            else
            {
                query = query.OrderBy(c => c.Name).ToList();
            }
            query = query
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize).ToList();
            return query;
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
                existingCat.AppUserId = Cat.AppUserId;
                var res = await _context.SaveChangesAsync();
                return res > 0;
            }
             return false;
        }
        public async Task<bool> IsNameInUseAsync(string CatName)
        {
            var cat =await _context.Categories.AnyAsync(c => c.Name == CatName);
            return cat;
        }

        public async Task<int> GetCategoryCountAsync(int id)
        {
            var categories= _context.Categories.Where(c => c.Id == id).ToList();
            if(categories == null)
            {
                return 0;
            }
            int count= categories.Count;
            return count;   
        }

        public async Task<int> GetCategoriesCountAsync(string userId, string? searchTerm)
        {
            var query = _context.Categories.Where(c => c.AppUserId == userId);
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(t =>
                    t.Name.ToLower().Contains(lowerSearch) ||
                    t.Description.ToLower().Contains(lowerSearch));
            }

            return await query.CountAsync();

        }
    }
}
