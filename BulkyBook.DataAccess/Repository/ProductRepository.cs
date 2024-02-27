using FruitSA.DataAccess.Repository.IRepository;
using FruitSA.Models;

namespace FruitSA.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void AddRange(List<Product> products)
        {
            _db.Products.AddRange(products);
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                // Update properties
                objFromDb.ProductCode = obj.ProductCode;
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Username = obj.Username;
                objFromDb.UpdatedAt = DateTime.Now;

                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
