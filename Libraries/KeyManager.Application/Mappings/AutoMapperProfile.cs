using KeyManager.Application.Commands;
using KeyManager.Application.Commands.Doors;

namespace KeyManager.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Dummy, CreateDummyCommand>().ReverseMap();
        CreateMap<Dummy, DummyDto>().ReverseMap();
        CreateMap<Dummy, UpdateDummyCommand>().ReverseMap();
        CreateMap<Door, CreateDoorCommand>().ReverseMap();
        CreateMap<Door, DoorDto>().ReverseMap();
    }
}