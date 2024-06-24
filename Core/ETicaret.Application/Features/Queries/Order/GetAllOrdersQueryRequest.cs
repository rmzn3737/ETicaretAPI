using MediatR;

namespace ETicaret.Application.Features.Queries.Order
{
    public class GetAllOrdersQueryRequest:IRequest<List<GetAllOrdersQueryResponse>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}