using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repository.IRepository;

namespace APIPeliculas.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly APIContext _context;
        public CategoryRepository(APIContext context)
        {
            _context = context;            
        }
        public bool CreateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _context.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
           _context.Categories.Remove(category);
            return Save();
        }

        public bool ExistCategory(string name)
        {
            bool value = _context.Categories.Any(
                c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ExistCategory(int id)
        {
            return _context.Categories.Any( c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
           category.CreationDate = DateTime.Now;
            _context.Categories.Update(category);
            return Save();
        }
    }
}
