using System;
using System.Data;
using System.Data.SqlClient;

namespace ShopBridgeDataAccess.Infrastructure
{
    public class DAL : IDisposable
    {
        #region Local Variable
        public string errortext = "";
        readonly SqlConnection con;
        readonly SqlDataAdapter sqlDataAdapter;
        readonly SqlCommand sqlCommand;
        #endregion

        #region Connction Open and Close
        public long lastid = 0;
        public DAL(SqlConnection conn)
        {
            con = conn;
            sqlCommand = new SqlCommand();
            sqlDataAdapter = new SqlDataAdapter();
            lastid = 0;
        }
        public void connect()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
                SqlConnection.ClearPool(con);
            }
        }

        public bool checkconnection()
        {
            try
            {
                con.Open();
                con.Close();
            }
            catch (Exception ex)
            {
                errortext = ex.Message + con.ConnectionString;
                return (false);
            }
            return (true);
        }
        #endregion

        #region Stored Procedures
        public int SPExecuteNonQuery(string proc, SqlParameter[] sqlParameters)
        {
            int IsSuccess;
            try
            {
                if (proc != null)
                {
                    SqlCommand sqlCommand = new SqlCommand(proc, con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters != null)
                    {
                        foreach (SqlParameter sqlParameter in sqlParameters)
                        {
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    this.connect();
                    IsSuccess = sqlCommand.ExecuteNonQuery();
                    this.disconnect();
                    return IsSuccess;
                }
                else
                {
                    IsSuccess = 0;
                    return IsSuccess;
                }
            }
            catch (SqlException sqlex)
            {
                _ = sqlex.Message;
                IsSuccess = 0;
                return IsSuccess;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
            finally
            {
                this.disconnect();
            }
        }
        public DataSet SPExecuteDataset(string proc, SqlParameter[] sqlParameters, string virtualtable)
        {
            try
            {
                if (proc != null)
                {
                    SqlCommand sqlCommand = new SqlCommand(proc, con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 2400;
                    DataSet dataSet = new DataSet();
                    if (sqlParameters != null)
                    {
                        foreach (SqlParameter sqlParameter in sqlParameters)
                        {
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    this.connect();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet, virtualtable);
                    return dataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.disconnect();
            }
        }

        public DataTable SPExecuteDataTable(string proc, SqlParameter[] sqlParameters, string virtualtable)
        {
            try
            {
                if (proc != null)
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        SqlCommand sqlCommand = new SqlCommand(proc, con);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParameters != null)
                        {
                            foreach (SqlParameter sqlParameter in sqlParameters)
                            {
                                sqlCommand.Parameters.Add(sqlParameter);
                            }
                        }
                        this.connect();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        sqlDataAdapter.SelectCommand.CommandTimeout = 240;
                        sqlDataAdapter.Fill(dataSet, virtualtable);
                    }
                    catch (Exception ex)
                    {
                        errortext = ex.Message;
                    }
                    finally
                    {
                        this.disconnect();
                    }
                    return (dataSet.Tables[virtualtable]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                this.disconnect();
            }

        }

        public long SPExecuteScalar(string proc, SqlParameter[] sqlParameters)
        {
            long result;
            try
            {
                if (proc != null)
                {
                    SqlCommand sqlCommand = new SqlCommand(proc, con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters != null)
                    {
                        foreach (SqlParameter param in sqlParameters)
                        {
                            sqlCommand.Parameters.Add(param);
                        }
                    }
                    con.Open();
                    sqlCommand.CommandTimeout = 99999;
                    object obj = (object)sqlCommand.ExecuteScalar();
                    result = Convert.ToInt64(obj);
                    con.Close();
                    return result;
                }
                else
                {
                    result = 0;
                    return result;
                }
            }
            catch (SqlException sqlex)
            {
                _ = sqlex.Message;
                result = 0;
                return result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
            finally
            {
                this.disconnect();
            }
        }

        //public long SPBulkUploadExecuteScalar(DataTable datatable, String TableName)
        //{
        //    long r;
        //    try
        //    {
        //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //        {
        //            sqlBulkCopy.DestinationTableName = TableName;
        //            con.Open();
        //            sqlBulkCopy.WriteToServer(datatable);
        //            con.Close();
        //            r = 1;
        //            return r;
        //        }
        //    }
        //    catch (SqlException sqlex)
        //    {
        //        _ = sqlex.Message;
        //        r = 0;
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = ex.Message;
        //        return 0;
        //    }
        //    finally
        //    {
        //        this.disconnect();
        //    }
        //}

        #endregion

        #region SQL Query      

        public long SQLExecuteNonQuery(string sql)
        {
            long result;
            try
            {
                this.connect();
                sqlCommand.Connection = con;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                result = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errortext = ex.Message;
                result = -1;
            }
            finally
            {
                this.disconnect();
            }

            return (result);
        }
        public long SQLExecuteScalar(string sql)
        {
            long result;
            try
            {
                this.connect();
                sqlCommand.Connection = con;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                result = (long)sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
                result = 0;
            }
            finally
            {
                this.disconnect();
            }
            return (result);
        }
        public DataTable SQLGetDataTablw(string sql)
        {
            DataSet dataSet = new DataSet();
            try
            {
                this.connect();
                sqlCommand.CommandTimeout = 99999;
                sqlCommand.Connection = con;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
            }
            finally
            {
                this.disconnect();
            }
            return (dataSet.Tables[0]);
        }
        public DataSet SQLGetDataSet(string sql)
        {
            DataSet dataSet = new DataSet();
            try
            {
                this.connect();
                sqlCommand.CommandTimeout = 99999;
                sqlCommand.Connection = con;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
            }
            finally
            {
                this.disconnect();
            }
            return dataSet;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConnectionFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
