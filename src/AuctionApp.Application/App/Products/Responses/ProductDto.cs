﻿using Application.App.Responses;
using Application.App.Users.Responses;
using Domain.Auth;

namespace Application.App.Products.Responses;
public class ProductDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int? CreatorId { get; set; }

    public UserDto? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public decimal? InitialPrice { get; set; }

    public HashSet<int> BidIds { get; set; } = [];

    public HashSet<CategoryDto> Categories { get; set; } = [];
}