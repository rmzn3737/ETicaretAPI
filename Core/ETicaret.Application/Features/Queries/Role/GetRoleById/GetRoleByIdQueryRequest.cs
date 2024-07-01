﻿using MediatR;

namespace ETicaret.Application.Features.Queries.Role.GetRoleById;

public class GetRoleByIdQueryRequest:IRequest<GetRoleByIdQueryResponse>
{
    public string Id { get; set; }
}