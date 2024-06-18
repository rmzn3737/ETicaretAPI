using MediatR;

namespace ETicaret.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryRequest:IRequest<List<GetBasketItemsQueryResponse>>
    {
    }
}