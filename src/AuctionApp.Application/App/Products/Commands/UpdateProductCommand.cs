using Application.App.Products.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.Products.Commands;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal InitialPrice { get; set; }

    public string Category { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{

    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetById<Product>(request.Id)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var category = (await _repository.GetByPredicate<Category>(c => c.Name == request.Category)).SingleOrDefault()
            ?? throw new EntityNotFoundException("Category cannot be found");

        if (product.CreatorId != request.CreatorId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (product.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit products of 5 minutes before its selling start");
        }

        _mapper.Map(request, product);

        product.Category = category;

        await _repository.SaveChanges();

        var productDto = _mapper.Map<Product, ProductDto>(product);

        return productDto;
    }
}
