using Boilerplate.Application.Commands;

namespace Boilerplate.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Dummy, CreateDummyCommand>().ReverseMap();
        CreateMap<Dummy, UpdateDummyCommand>().ReverseMap();
    }
}