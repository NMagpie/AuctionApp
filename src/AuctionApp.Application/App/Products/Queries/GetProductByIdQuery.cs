﻿using Application.App.Products.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetById<Product>(request.Id)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var lotDto = _mapper.Map<Product, ProductDto>(lot);

        return lotDto;
    }
}
