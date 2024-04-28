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
        readonly private ICustomerWriteRepository _customerWriteRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IOrderReadRepository _orderReadRepository;

        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IOrderWriteRepository orderWriteRepository,
            ICustomerWriteRepository customerWriteRepository,
            ICustomerReadRepository customerReadRepository,
            IOrderReadRepository orderReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _customerReadRepository = customerReadRepository;
            _orderReadRepository = orderReadRepository;
        }
        [HttpGet]
        public async Task Get()
        {

            Order order = await _orderReadRepository.GetByIdAsync("c0b1fd8c-61e0-4010-9478-067a00c072d7");
            order.Address = "Türkiye, İstanbul";
            await _orderWriteRepository.SaveAsync();
            
            /*var customerId = Guid.NewGuid();
            await _customerWriteRepository.AddAsync(new() { Id = customerId,Name="Alex De Souza" });

           await _orderWriteRepository.AddAsync(new() { Description = "bla bla bla", Address = "Türkiye, Konya",CustomerID=customerId });
           await _orderWriteRepository.AddAsync(new() { Description = "bla2 bla2 bla2", Address = "Türkiye, Ege",CustomerID = customerId });
           await _orderWriteRepository.SaveAsync();*/
        }
        
        
    }
}
