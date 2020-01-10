using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Managers
{
    public interface IEFProductManager
    {
        List<Product> GetProducts(out int total, string category = "", int page = 1, int pageSize = 10000);
        Product GetProduct(int id);
        List<string> GetCategories();
        BaseResult SaveProduct(Product model);
        BaseResult DeleteProduct(int productID);
    }
}
