using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductServices.Core.IServices;

namespace ProductServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.All();

            return Ok(products);
        }
    }
}
