namespace Boilerplate.Api.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DummyDto, GetDummyResponse>().ReverseMap();
        CreateMap<DummyDto, CreateDummyResponse>().ReverseMap();
        CreateMap<DummyDto, UpdateDummyResponse>().ReverseMap();
    }
}