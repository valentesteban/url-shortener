using url_shortener.Entities;

namespace url_shortener.Data.Interfaces;

public interface ICategoryService
{
    public List<Category> getAll();
    public Category? getById(int id);
    public Category? getByName(string name);
    public void createCategory(string name);
    public void updateCategory(int id, string name);
    public void deleteCategory(int id);
}