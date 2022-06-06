
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeDataAccess.Models;
using ShopBridgeDataAccess.UnitOfWork;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment environment) : base(unitOfWork)
        {
            _hostingEnvironment = environment;
        }
        [HttpGet("{Page}/{PageSize}")]
        public async Task<ActionResult> GetProductList(int Page = 1, int PageSize = 10)
        {
            Pagination pagination = new Pagination();
            pagination.Page = Page;
            pagination.PageSize= PageSize;

            return new OkObjectResult(await _service.ProductRepository.ListProduct(pagination));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            ProductModel productModel = new ProductModel();
            productModel = await _service.ProductRepository.GetProductById(Id);
            return new OkObjectResult(productModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveProduct([FromForm] ProductModel product)
        {
            long result = 0;
            if (product.ProductId > 0)
            {
                ModelState.Remove("Image");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (product.Image != null)
                    {
                        string siteUrl = Convert.ToString(Configuration.GetSection("SiteURL").GetValue<string>("URL"));
                        if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                        {
                            _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        }
                        string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/Product");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var photoname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(product.Image.FileName);
                        var filepath = Path.Combine(path, photoname);
                        using var filtarget = new FileStream(filepath, FileMode.Create);
                        await product.Image.CopyToAsync(filtarget);
                        product.ImagePath = siteUrl + "Images/Product/" + photoname;
                    }

                    if (product.ProductId > 0)
                    {
                        result = await _service.ProductRepository.UpdateProduct(product);
                    }
                    else
                    {
                        result = await _service.ProductRepository.SaveProduct(product);
                    }
                    if (result == 1)
                    {
                        return new OkObjectResult("Success");
                    }
                    else if (result == 2)
                    {
                        return new OkObjectResult("Product Name already exist");
                    }
                    else if (result == 3)
                    {
                        return new OkObjectResult("Product Code already exist");
                    }
                    else
                    {
                        return new OkObjectResult("Product Code already exist");
                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
                    return new OkObjectResult(errors);
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult("Somwthing went wrong");
            }
        }

        [HttpPost("{Id}")]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            long result = 0;

            try
            {
                result = await _service.ProductRepository.DeleteProduct(Id);
                if (result == 1)
                {
                    return new OkObjectResult("Deleted Successfully");
                }
                else
                {
                    return new OkObjectResult("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult("Something went wrong");
            }
        }
    }
}
