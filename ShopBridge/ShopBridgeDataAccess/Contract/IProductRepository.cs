using ShopBridgeDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridgeDataAccess.Contract
{
    public interface IProductRepository
    {
        Task<long> SaveProduct(ProductModel product);
        Task<long> UpdateProduct(ProductModel product);
        Task<long> DeleteProduct(int Id);
        Task<IEnumerable<ProductModel>> ListProduct(Pagination pagination);
        Task<ProductModel> GetProductById(int Id);
    }
}
