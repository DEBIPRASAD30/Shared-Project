using ShopBridgeDataAccess.Infrastructure;
using ShopBridgeDataAccess.Repository;
using ShopBridgeDataAccess.Contract;
using ShopBridgeDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridgeDataAccess.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }

        public async Task<long> SaveProduct(ProductModel product)
        {
            long result = 0;
            using (var dbconnect = connectionFactory.GetDAL)
            {
                SqlParameter[] sqlparameters =
                {
                     new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(0) },
                    new SqlParameter("@productName", SqlDbType.VarChar) { Value = product.ProductName },
                     new SqlParameter("@productCode", SqlDbType.VarChar) { Value = product.ProductCode },
                     new SqlParameter("@description", SqlDbType.VarChar) { Value = product.Description },
                     new SqlParameter("@image", SqlDbType.NVarChar) { Value = product.ImagePath },
                     new SqlParameter("@price", SqlDbType.BigInt) { Value = product.Price },
                     new SqlParameter("@createdBy", SqlDbType.BigInt) { Value = product.CreatedBy },
                     new SqlParameter("@operationtype", SqlDbType.VarChar) { Value = "INSERT" },
                };
                result = await Task.Run(() => dbconnect.SPExecuteScalar("product_CRUD", sqlparameters));
            }
            return result;
        }

        public async Task<long> UpdateProduct(ProductModel product)
        {
            long result = 0;
            using (var dbconnect = connectionFactory.GetDAL)
            {
                SqlParameter[] sqlparameters =
                {
                     new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(product.ProductId) },
                     new SqlParameter("@productName", SqlDbType.VarChar) { Value = product.ProductName },
                     new SqlParameter("@productCode", SqlDbType.VarChar) { Value = product.ProductCode },
                     new SqlParameter("@description", SqlDbType.VarChar) { Value = product.Description },
                     new SqlParameter("@image", SqlDbType.VarChar) { Value = product.ImagePath },
                     new SqlParameter("@createdBy", SqlDbType.BigInt) { Value = product.CreatedBy },
                     new SqlParameter("@operationtype", SqlDbType.VarChar) { Value = "UPDATE" },
                };
                result = await Task.Run(() => dbconnect.SPExecuteScalar("product_CRUD", sqlparameters));
            }
            return result;
        }

        public async Task<long> DeleteProduct(int Id)
        {
            long result = 0;
            using (var dbconnect = connectionFactory.GetDAL)
            {
                SqlParameter[] sqlparameters =
                {
                     new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(Id) },
                     new SqlParameter("@operationtype", SqlDbType.VarChar) { Value = "DELETE" },
                };
                result = await Task.Run(() => dbconnect.SPExecuteScalar("product_CRUD", sqlparameters));
            }
            return result;
        }

        public async Task<IEnumerable<ProductModel>> ListProduct(Pagination pagination)
        {
            ProductModel productModel = new ProductModel();
            List<ProductModel> listProducts = new List<ProductModel>();
            using (var dbconnect = connectionFactory.GetDAL)
            {
                SqlParameter[] sqlparameters =
                {
                     new SqlParameter("@offsetvalue", SqlDbType.Int) { Value = (pagination.Page -1) * pagination.PageSize},
                     new SqlParameter("@pagingsize", SqlDbType.Int) { Value = pagination.PageSize },
                     new SqlParameter("@operationtype", SqlDbType.VarChar) { Value = "SELECT" },
                };
                DataSet dataSet = await Task.Run(() => dbconnect.SPExecuteDataset("product_CRUD", sqlparameters, "DS"));
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        ProductModel model = new ProductModel();
                        model.ProductId = dataSet.Tables[0].Rows[i]["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(dataSet.Tables[0].Rows[i]["ProductId"]);
                        model.ProductName = dataSet.Tables[0].Rows[i]["ProductName"] == DBNull.Value ? String.Empty : Convert.ToString(dataSet.Tables[0].Rows[i]["ProductName"]);
                        model.ProductCode = dataSet.Tables[0].Rows[i]["ProductCode"] == DBNull.Value ? String.Empty : Convert.ToString(dataSet.Tables[0].Rows[i]["ProductCode"]);
                        model.Description = dataSet.Tables[0].Rows[i]["Description"] == DBNull.Value ? String.Empty : Convert.ToString(dataSet.Tables[0].Rows[i]["Description"]);
                        model.ImagePath = dataSet.Tables[0].Rows[i]["ImagePath"] == DBNull.Value ? String.Empty : Convert.ToString(dataSet.Tables[0].Rows[i]["ImagePath"]);
                        listProducts.Add(model);
                    }
                    return listProducts;
                }
                else
                {
                    var a = Array.Empty<DataTable>();
                    listProducts = new List<ProductModel>(a.Cast<ProductModel>());
                    //listProducts = new List<ProductModel>();
                }
            }
            return listProducts;
        }

        public async Task<ProductModel> GetProductById(int Id)
        {
            ProductModel product = new ProductModel();
            using (var dbconnect = connectionFactory.GetDAL)
            {
                SqlParameter[] sqlparameters =
                {
                     new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(Id) },
                     new SqlParameter("@operationtype", SqlDbType.VarChar) { Value = "SELECTONE" },
                };
                DataTable dataTable = await Task.Run(() => dbconnect.SPExecuteDataTable("product_CRUD", sqlparameters, "DT"));

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    ProductModel model = new ProductModel();
                    model.ProductId = dataTable.Rows[0]["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(dataTable.Rows[0]["ProductId"]);
                    model.ProductName = dataTable.Rows[0]["ProductName"] == DBNull.Value ? String.Empty : Convert.ToString(dataTable.Rows[0]["ProductName"]);
                    model.ProductCode = dataTable.Rows[0]["ProductCode"] == DBNull.Value ? String.Empty : Convert.ToString(dataTable.Rows[0]["ProductCode"]);
                    model.Description = dataTable.Rows[0]["Description"] == DBNull.Value ? String.Empty : Convert.ToString(dataTable.Rows[0]["Description"]);
                    model.ImagePath = dataTable.Rows[0]["ImagePath"] == DBNull.Value ? String.Empty : Convert.ToString(dataTable.Rows[0]["ImagePath"]);
                    product = model;
                }
                else
                {
                    product = new ProductModel();
                }
            }
            return product;
        }
    }
}
