using Application.App.Products.Commands;
using Application.App.Products.Responses;
using Application.App.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.Products;
using Presentation.Common.Requests.Products;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductsController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var productDto = await _mediator.Send(new GetProductByIdQuery() { Id = id });

        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest createProductRequest)
    {
        var userId = GetUserId();

        var productCommand = _mapper.Map<CreateProductRequest, CreateProductCommand>(createProductRequest);

        productCommand.CreatorId = userId;

        var productDto = await _mediator.Send(productCommand);

        return Ok(productDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductRequest updateProductRequest)
    {
        var userId = GetUserId();

        var productCommand = _mapper.Map<UpdateProductRequest, UpdateProductCommand>(updateProductRequest);

        productCommand.Id = id;

        productCommand.CreatorId = userId;

        var productDto = await _mediator.Send(productCommand);

        return Ok(productDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteProductCommand() { Id = id, UserId = userId });

        return Ok();
    }
}