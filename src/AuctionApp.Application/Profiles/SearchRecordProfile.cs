using AuctionApp.Application.App.SearchQueries.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace AuctionApp.Application.Profiles;

public class SearchRecordProfile : Profile
{
    public SearchRecordProfile()
    {
        CreateMap<SearchRecord, SearchRecordDto>();
    }
}
