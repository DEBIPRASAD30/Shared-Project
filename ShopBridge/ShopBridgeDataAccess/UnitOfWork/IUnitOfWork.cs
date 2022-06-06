using System;
using ShopBridgeDataAccess.Contract;

namespace ShopBridgeDataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
       
        IProductRepository ProductRepository { get; }
        
        void Complete();
    }
}
