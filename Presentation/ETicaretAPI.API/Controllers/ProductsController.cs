using ETicaret.Application.Features.Commands.Product.CreateProduct;
using ETicaret.Application.Features.Commands.Product.RemoveProduct;
using ETicaret.Application.Features.Commands.Product.UpdateProduct;
using ETicaret.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaret.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaret.Application.Features.Queries.Product.GetAllProduct;
using ETicaret.Application.Features.Queries.ProductImageFile;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ETicaret.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using Microsoft.AspNetCore.Authorization;


namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class ProductsController : ControllerBase
    {
        /*readonly private IProductWriteRepository _productWriteRepository;//private readonly IProductService _productService;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;*/

        private readonly IMediator _mediator;



        public ProductsController(/*IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService, IConfiguration configuration, */IMediator mediator)
        {
            /*_productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;

            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;*/
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);

            /*//Task.Delay(5000);
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();//Skip(pagination.Page * pagination.Size).Take(pagination.Size);

            return Ok(new
            {
                totalCount,
                products

            });*/
        }

        /*[HttpGet("{Id}")]
        public async Task <IActionResult > Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse _getByIdProductQueryResponse= await _mediator.Send(getByIdProductQueryRequest);
            return Ok(_getByIdProductQueryResponse);
        }*/
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse createProductCommandResponse = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest) //Update
        {
            UpdateProductCommandResponse updateProductCommandResponse = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task <IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
             RemoveProductCommandResponse removeProductCommandResponse = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse uploadProductImageCommandResponse = await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }


        
        [HttpGet("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            /*Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            //await Task.Delay(2000); //spinnerın çalışıp çalışmadığını görmek için koymuştuk.
            return Ok(product.ProductImageFiles.Select(p => new
            {
                //Path = $"{_configuration["LocalStorage:Path"]}/{p.Path}", //todo burası Azure bağlanırsak resmin urlini elde ettiğimiz yer. Localde alttaki gibi çalışacağız.
                //p.Path,
                Path = $"{_configuration["BaseLocalStorageUrl"]}\\{p.Path}",
                p.FileName,
                p.Id
            }));*/

            List<GetProductImagesQueryResponse> getProductImagesQueryResponse = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(getProductImagesQueryResponse);
        }


        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]RemoveProductImageCommandRequest removeProductImageCommandRequest,[FromQuery] string imageId)
        {
            /*Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();*/
            removeProductImageCommandRequest.ImageId=imageId;
            RemoveProductImageCommandResponse removeProductImageCommandResponse =
                await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }

        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<ActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }

        //todo #34- Asp.NET Core 6 + Angular İle Mini E-Ticaret _ CQRS ve Mediator Pattern Üzerine Teorik Anlatım. Bu ders bitti CQRS Design Pattern'in terik altyapısı konuşuldu, artık cu controller sınıfını CQRS e taşıyaşcağız.

        /*[HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }*/

        #region LocalStorage için Chat gPT önerisi metod.

        //[HttpGet("[action]/{id}")]
        //public IActionResult GetProductImagesTwo(string productId)
        //{
        //    var images = new List<ImageModel>
        //    {
        //        new ImageModel { Path = "wwwroot/photo-images/translatedimagetr-3.png", FileName = "translatedimagetr-3.png" },
        //        new ImageModel { Path = "wwwroot/photo-images/KiRAZ.jpg", FileName = "KiRAZ.jpg" },
        //        new ImageModel { Path = "wwwroot/photo-images/translatedimageen.png", FileName = "translatedimageen.png" }
        //    };

        //    var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
        //    var imageUrls = images.Select(image => new
        //    {
        //        Url = $"{baseUrl}/{image.Path.Replace("wwwroot/", "")}",
        //        image.FileName
        //    });

        //    return Ok(imageUrls);
        //}

        //public class ImageModel
        //{
        //    public string Path { get; set; }
        //    public string FileName { get; set; }
        //}
        #endregion

        //[HttpGet("get")]
        //public string Get()
        //{
        //    return "get";
        //}

    }

}
