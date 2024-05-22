using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;

using ETicaret.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;


namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;//private readonly IProductService _productService;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;



        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService, IConfiguration configuration)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;

            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            //Task.Delay(5000); 
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(pagination.Page * pagination.Size).Take(pagination.Size);

            return Ok(new
            {
                totalCount,
                products

            });
        }

        [HttpGet("{id}")]

        public async Task <IActionResult > Get(string id)
        {
            
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            if (ModelState.IsValid)
            {

            }
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price,
            });

            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model) //Update
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name    = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task <IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            //foreach (var r in result)
            //{
            //product.ProductImageFiles.Add(new()
            //{
            //FileName = r.fileName,
            //Path = r.pathOrContainerName,
            //Storage = _storageService.StorageName,
            //Products = new List<Product>() { product }
            //});
            //}

            //todo ***** Alttaki ve üsttki yöntem resim eklemek için birbirinin alternatifi.

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
                {
                    FileName = r.fileName,
                    Path = r.pathOrContainerName,
                    Storage = _storageService.StorageName,
                    Products = new List<Product>() { product }
                }
            ).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            //await Task.Delay(2000); //spinnerın çalışıp çalışmadığını görmek için koymuştuk.
            return Ok(product.ProductImageFiles.Select(p => new
            {
                //Path = $"{_configuration["LocalStorage:Path"]}/{p.Path}", //todo burası Azure bağlanırsak resmin urlini elde ettiğimiz yer. Localde alttaki gibi çalışacağız.
                //p.Path,
                Path = $"{_configuration["BaseLocalStorageUrl"]}\\{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

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




    }

}
