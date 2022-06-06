using ShopBridgeDataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace ShopBridge.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public IConfiguration Configuration { get; }

        public readonly IUnitOfWork _service;
        public BaseController(IUnitOfWork service)
        {
            #region Configuration Builder add the appsetting.json key and value         
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            _configuration = Configuration;
            #endregion

            #region UnitOfWork
            _service = service;
            #endregion
        }
    }
}
