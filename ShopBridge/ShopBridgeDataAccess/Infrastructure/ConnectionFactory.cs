

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ShopBridgeDataAccess.Infrastructure
{
    public  class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;
        private readonly string _userlogconnectionString;
        private readonly string _errorlogconnectionString;
        public ConnectionFactory(IConfiguration iconfiguration)
        {
            //Default Connection
            connectionString = iconfiguration.GetConnectionString("DefaultConnection");
            //User Log Connection
            //_userlogconnectionString = iconfiguration.GetConnectionString("LogConnection");
            ////Error Log Connection
            //_errorlogconnectionString = iconfiguration.GetConnectionString("ErrorLogConnection");
        }

        public DAL GetDAL
        {
            get
            {
                SqlConnection conn = new SqlConnection
                {
                    ConnectionString = connectionString
                };
                DAL dal = new DAL(conn);
                return dal;
            }
        }

        //public DAL GetUserLogDAL
        //{
        //    get
        //    {
        //        SqlConnection conn = new SqlConnection
        //        {
        //            ConnectionString = _userlogconnectionString
        //        };
        //        DAL dal = new DAL(conn);
        //        return dal;
        //    }
        //}

        //public DAL GetErrorLogDAL
        //{
        //    get
        //    {
        //        SqlConnection conn = new SqlConnection
        //        {
        //            ConnectionString = _errorlogconnectionString
        //        };
        //        DAL dal = new DAL(conn);
        //        return dal;
        //    }
        //}

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
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}