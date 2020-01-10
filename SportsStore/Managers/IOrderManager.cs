using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Managers
{
    public interface IOrderManager
    {
        List<Order> GetOrders(out int total, int page = 1, int pageSize = 10000, bool shipped = false);
        int SaveOrder(Order order, out string msg);
        void MarkShipped(int orderID, bool shipped);
    }
}
