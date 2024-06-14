using Application.App.Products.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public int CreatorId { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public decimal InitialPrice { get; set; }

    public string Category { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{

    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = (await _repository.GetByPredicate<Category>(c => c.Name == request.Category)).SingleOrDefault()
            ?? throw new EntityNotFoundException("Category cannot be found");

        var product = _mapper.Map<CreateProductCommand, Product>(request);

        product.Category = category;

        await _repository.Add(product);

        await _repository.SaveChanges();

        var productDto = _mapper.Map<Product, ProductDto>(product);

        return productDto;
    }
}
