using APIPeliculas.Models;

namespace APIPeliculas.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool ExistCategory(string name);
        bool ExistCategory(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
