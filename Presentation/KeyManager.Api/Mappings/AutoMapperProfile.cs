using KeyManager.Api.DTOs.Responses.Users;

namespace KeyManager.Api.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DummyDto, GetDummyResponse>().ReverseMap();
        CreateMap<DummyDto, CreateDummyResponse>().ReverseMap();
        CreateMap<DummyDto, UpdateDummyResponse>().ReverseMap();
        CreateMap<UserDto, CreateUserResponse>();
        CreateMap<UserDto, UpdateUserResponse>();
        CreateMap<UserDto, GetUserResponse>();
    }
}