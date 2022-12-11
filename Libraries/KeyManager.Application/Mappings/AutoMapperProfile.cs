using KeyManager.Application.Commands;
using KeyManager.Application.Commands.Doors;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Application.Commands.Roles;
using KeyManager.Application.Commands.Users;

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
        CreateMap<Door, UpdateDoorCommand>().ReverseMap();
        CreateMap<Role, CreateRoleCommand>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleCommand>().ReverseMap();
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<Permission, CreatePermissionCommand>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Permission, UpdatePermissionCommand>().ReverseMap();
    }
}