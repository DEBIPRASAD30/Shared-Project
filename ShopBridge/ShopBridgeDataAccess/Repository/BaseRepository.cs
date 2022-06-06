using ShopBridgeDataAccess.Infrastructure;

namespace ShopBridgeDataAccess.Repository
{
    public abstract class BaseRepository
    {
        protected readonly IConnectionFactory connectionFactory;
        public BaseRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
    }
}
