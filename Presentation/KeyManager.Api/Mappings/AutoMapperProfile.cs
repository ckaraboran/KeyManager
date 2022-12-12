using KeyManager.Api.DTOs.Responses.Doors;
using KeyManager.Api.DTOs.Responses.Permissions;
using KeyManager.Api.DTOs.Responses.Roles;
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
        CreateMap<DoorDto, CreateDoorResponse>();
        CreateMap<DoorDto, UpdateDoorResponse>();
        CreateMap<DoorDto, GetDoorResponse>();
        CreateMap<RoleDto, CreateRoleResponse>();
        CreateMap<RoleDto, UpdateRoleResponse>();
        CreateMap<RoleDto, GetRoleResponse>();
        CreateMap<PermissionWithNamesDto, GetPermissionResponse>();
        CreateMap<PermissionDto, CreatePermissionResponse>();
        CreateMap<PermissionDto, UpdatePermissionResponse>();
    }
}