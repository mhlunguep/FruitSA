using FruitSA.DataAccess.Repository.IRepository;
using FruitSA.Models;

namespace FruitSA.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool Any(Func<Category, bool> predicate)
        {
            return _db.Categories.Any(predicate);
        }

        public void Update(Category obj)
        {
            obj.UpdatedAt = DateTime.Now;
            _db.Categories.Update(obj);
        }

    }
}
