﻿namespace Application.App.UserWatchlists.Responses;
public class UserWatchlistDto
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public int? ProductId { get; set; }
}
