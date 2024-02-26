using FruitSA.Models;

namespace FruitSA.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void AddRange(List<Product> products);
        void Update(Product obj);
    }
}
