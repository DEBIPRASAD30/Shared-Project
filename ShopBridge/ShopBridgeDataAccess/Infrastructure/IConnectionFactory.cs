using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeDataAccess.Infrastructure
{
    public interface IConnectionFactory
    {
        DAL GetDAL { get; }
        //DAL GetUserLogDAL { get; }
        //DAL GetErrorLogDAL { get; }
    }
}
