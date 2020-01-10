using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SportsStore.Models;
using System.IO;

namespace SportsStore.Managers
{
    public class EFProductManager : BaseManager, IEFProductManager 
    {
        public EFProductManager(string _connectionString, string _webRootPath) : base(_connectionString, _webRootPath) { }

        public List<Product> GetProducts(out int total, string category = "", int page = 1, int pageSize = 10000)
        {
            total = 0;
            List<Product> products = new List<Product>();
            using (var conn = new SqlConnection(_connectionString))
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("page", page);
                _params.Add("pageSize", pageSize);
                _params.Add("category", category);
                _params.Add("total", DbType.Int32, direction: ParameterDirection.Output);
                conn.Open();
                products = conn.Query<Product>("spst_GetProducts", _params, commandType:CommandType.StoredProcedure).ToList();
                total = _params.Get<int>("total");
            }
            return products;
        }

        public List<string> GetCategories()
        {
            List<string> res = new List<string>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                res = conn.Query<string>("spst_GetCategories", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return res;
        }

        public Product GetProduct(int id)
        {
            Product p = new Product();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    DynamicParameters _params = new DynamicParameters();
                    _params.Add("productID", id);
                    _params.Add("total", DbType.Int32, direction: ParameterDirection.Output);
                    conn.Open();
                    p = conn.QueryFirst<Product>("spst_GetProducts", _params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                p = null;
            }
            
            return p;
        }

        public BaseResult SaveProduct(Product model)
        {
            BaseResult result = new BaseResult();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    DynamicParameters _params = new DynamicParameters();
                    _params.Add("productID", model.ProductID);
                    _params.Add("Description", model.Description);
                    _params.Add("Name", model.Name);
                    _params.Add("Price", model.Price);
                    _params.Add("Category", model.Category);
                    _params.Add("LogoPath", model.LogoPath);
                    conn.Open();
                    conn.Execute("spst_SaveProduct", _params, commandType: CommandType.StoredProcedure);
                    result.IsSuccess = true;
                }
            }
            catch (SqlException ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public BaseResult DeleteProduct(int productID)
        {
            BaseResult result = new BaseResult();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    DynamicParameters _params = new DynamicParameters();
                    _params.Add("productID", productID);
                    conn.Open();
                    string filePath = conn.QueryFirst<string>("spst_DeleteProduct", _params, commandType: CommandType.StoredProcedure);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        DeleteProductLogoFileAsync(filePath);
                    }
                    result.IsSuccess = true;
                }
            }
            catch (SqlException ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        /// <summary>
        /// Удаляет лого товара
        /// </summary>
        /// <param name="fileAbsPath">относительный путь к лого</param>
        /// <returns></returns>
        private async Task<bool> DeleteProductLogoFileAsync(string fileAbsPath)
        {
            bool result = true;
            await Task.Run(()=> {
                string fullPAth = _webRootPath + fileAbsPath;
                if (File.Exists(fullPAth))
                {
                    File.Delete(fullPAth);
                }
            });
            return result;
        }
    }
}
