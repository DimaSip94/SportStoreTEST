using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SportsStore.Models;

namespace SportsStore.Managers
{
    public class OrderManager : BaseManager, IOrderManager
    {
        public OrderManager(string _connectionString, string _webRootPath) :base(_connectionString, _webRootPath) { }

        public List<Order> GetOrders(out int total, int page = 1, int pageSize = 10000, bool shipped = false)
        {
            total = 0;
            List<Order> res = new List<Order>();
            using (var conn = Connection)
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("page", page);
                _params.Add("pageSize", pageSize);
                _params.Add("shipped", shipped);
                conn.Open();
                var dataGrid = conn.QueryMultiple("spst_GetOrders", _params, commandType: CommandType.StoredProcedure);
                total = dataGrid.Read<int>().FirstOrDefault();
                res = dataGrid.Read<Order>().ToList();
                List<CartLine> lines = dataGrid.Read<CartLine>().ToList();
                List<Product> products = dataGrid.Read<Product>().ToList();
                foreach (var r in res)
                {
                    r.Lines = new List<CartLine>();
                    foreach (var l in lines.Where(x=>x.OrderID==r.OrderID))
                    {
                        l.Product = products.FirstOrDefault(x => x.CartLineID == l.CartLineID);
                        r.Lines.Add(l);
                    }
                }
            }
            return res;
        }

        public int SaveOrder(Order order, out string msg)
        {
            int newOrderID = 0; 
            msg = "";
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters _params = new DynamicParameters();
                    _params.Add("@Name", order.Name);
                    _params.Add("@Line1", order.Line1);
                    _params.Add("@Line2", order.Line2);
                    _params.Add("@Line3", order.Line3);
                    _params.Add("@State", order.State);
                    _params.Add("@Zip", order.Zip);
                    _params.Add("@City", order.City);
                    _params.Add("@Country", order.Country);
                    _params.Add("@GiftWrap", order.GiftWrap);
                    _params.Add("@CartLines", ToTableValuedParameter(order.Lines.Select(x=>new CartLineType {ProductID=x.Product.ProductID, Quantity=x.Quantity}).ToArray()));
                    conn.Open();
                    newOrderID = conn.QueryFirst<int>("SaveOrder", _params, commandType: CommandType.StoredProcedure);
                }       
            }
            catch (SqlException sqlEx)
            {
                msg = sqlEx.Message;
                newOrderID = 0;
            } 
            catch(Exception exc)
            {
                msg = exc.Message;
                newOrderID = 0;
            }
            return newOrderID;
        }

        public void MarkShipped(int orderID, bool shipped)
        {
            using (var conn = Connection)
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@orderID", orderID);
                _params.Add("@shipped", shipped);
                conn.Open();
                conn.Execute("spst_GetOrders", _params, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
