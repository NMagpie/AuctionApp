using Application.App.Products.Responses;
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
        var product = await _repository.GetByIdWithInclude<Product>(request.Id, product => product.Creator)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var productDto = _mapper.Map<Product, ProductDto>(product);

        return productDto;
    }
}
