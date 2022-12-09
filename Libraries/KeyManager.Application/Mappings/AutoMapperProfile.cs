using KeyManager.Application.Commands;

namespace KeyManager.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Dummy, CreateDummyCommand>().ReverseMap();
        CreateMap<Dummy, UpdateDummyCommand>().ReverseMap();
    }
}