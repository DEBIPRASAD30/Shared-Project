using System;
using ShopBridgeDataAccess.Infrastructure;
using ShopBridgeDataAccess.Contract;
using ShopBridgeDataAccess.Repository;

namespace ShopBridgeDataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IConnectionFactory _connectionFactory;
        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }



        #region  IProductRepository 
        public IProductRepository IProductRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (IProductRepository == null)
                {
                    IProductRepository = new ProductRepository(_connectionFactory);
                }
                return IProductRepository;
            }
        }
        #endregion



        #region IDisposable Support
        void IUnitOfWork.Complete()
        {
            throw new NotImplementedException();
        }

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
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
