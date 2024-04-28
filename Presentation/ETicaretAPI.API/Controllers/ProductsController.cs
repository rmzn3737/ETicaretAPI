using ETicaret.Application.Abstractions;
using ETicaret.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Concretes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;//private readonly IProductService _productService;
        readonly private IProductReadRepository _productReadRepository;
        readonly private IOrderWriteRepository _orderWriteRepository;

        public ProductsController(
            IProductWriteRepository productWriteRepository, 
            IProductReadRepository productReadRepository, 
            IOrderWriteRepository orderWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
        }
        [HttpGet]
        public async Task Get()
        {
           await _productWriteRepository.AddAsync(new() { Name="C Product", Price=1.500F,Stock=10,CreatedDate=DateTime.UtcNow});
            await _productWriteRepository.SaveAsync();
            //await _productWriteRepository.AddRangeAsync(new()
            // {
            //     new(){ Id=Guid.NewGuid(),Name="Product 1",Price=100,CreatedDate =DateTime.UtcNow,Stock=10 },
            //     new(){ Id=Guid.NewGuid(),Name="Product 2",Price=200,CreatedDate =DateTime.UtcNow,Stock=20 },
            //     new(){ Id=Guid.NewGuid(),Name="Product 3",Price=300,CreatedDate =DateTime.UtcNow,Stock=30 },
            // });
            //var count = await _productWriteRepository.SaveAsync();

            /*Product eldeEdilenProduct= await _productReadRepository.GetByIdAsync("c79b3cf9-959c-4458-94d4-edb1499a7d0b",false);
            //eldeEdilenProduct.Name = "Ahmet";
            eldeEdilenProduct.Name = "Mehmet";
            await _productWriteRepository.SaveAsync();*/
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }


        //public ProductsController(IProductService productService)
        //{
        //     _productService = productService;
        //}

        ////[HttpGet]
        ////public IActionResult GetProducts()
        ////{

        ////   var products = _productService.GetProducts();
        ////    return Ok(products);
        ////}
    }
}
