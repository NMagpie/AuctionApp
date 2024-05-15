using Application.App.Lots.Commands;
using AutoMapper;
using Presentation.Common.Models.Lots;
using Presentation.Common.Requests.Lots;

namespace Presentation.Common.Profiles;
public class LotProfile : Profile
{
    public LotProfile()
    {
        CreateMap<CreateLotRequest, CreateLotCommand>();

        CreateMap<UpdateLotRequest, UpdateLotCommand>();
    }
}
