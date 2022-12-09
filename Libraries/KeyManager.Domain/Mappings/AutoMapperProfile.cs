using KeyManager.Domain.DTOs;
using KeyManager.Domain.Entities;

namespace KeyManager.Domain.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Dummy, DummyDto>().ReverseMap();
    }
}