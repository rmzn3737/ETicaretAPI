using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler:IRequestHandler<ChangeShowcaseImageCommandRequest,ChangeShowcaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                    .Include(p => p.Products) //todo Join atıyoruz.
                    .SelectMany(p => p.Products, (pif, p) => new
                    {
                        pif, //Product İmage File
                        p //Product
                    });
            var data = await query.FirstOrDefaultAsync(p =>
                p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase == true);


            data.pif.Showcase = false;

            var image =await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageId));
            image.pif.Showcase = true;
            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
