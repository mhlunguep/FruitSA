using FruitSA.Models;

namespace FruitSA.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        bool Any(Func<Category, bool> predicate);
        void Update(Category obj);
    }
}
