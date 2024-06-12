using Application.App.ProductReviews.Commands;
using Application.App.ProductReviews.Responses;
using Application.App.Queries;
using AuctionApp.Application.App.ProductReviews.Queries;
using AuctionApp.Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.ProductReviews;
using Presentation.Common.Requests.ProductReviews;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductReviewsController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public ProductReviewsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [SwaggerOperation(OperationId = nameof(GetProductReview))]
    public async Task<ActionResult<ProductReviewDto>> GetProductReview(int id)
    {
        var productReviewDto = await _mediator.Send(new GetProductReviewByIdQuery() { Id = id });

        return Ok(productReviewDto);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = nameof(CreateProductReview))]
    public async Task<ActionResult<ProductReviewDto>> CreateProductReview(CreateProductReviewRequest createProductReviewRequest)
    {
        var userId = GetUserId();

        var productReviewCommand = _mapper.Map<CreateProductReviewRequest, CreateProductReviewCommand>(createProductReviewRequest);

        productReviewCommand.UserId = userId;

        var productReviewDto = await _mediator.Send(productReviewCommand);

        return Ok(productReviewDto);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(OperationId = nameof(UpdateProductReview))]
    public async Task<ActionResult<ProductReviewDto>> UpdateProductReview(int id, UpdateProductReviewRequest updateProductReviewRequest)
    {
        var userId = GetUserId();

        var productReviewCommand = _mapper.Map<UpdateProductReviewRequest, UpdateProductReviewCommand>(updateProductReviewRequest);

        productReviewCommand.Id = id;

        productReviewCommand.UserId = userId;

        var productReviewDto = await _mediator.Send(productReviewCommand);

        return Ok(productReviewDto);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(OperationId = nameof(DeleteProductReview))]
    public async Task<ActionResult> DeleteProductReview(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteProductReviewCommand() { Id = id, UserId = userId });

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetPagedReviews))]
    public async Task<ActionResult<PaginatedResult<ProductReviewDto>>> GetPagedReviews([FromQuery] int productId, [FromQuery] int pageIndex)
    {
        var result = await _mediator.Send(new GetProductReviewsOfProductQuery() { ProductId = productId, PageIndex = pageIndex });

        return Ok(result);
    }
}