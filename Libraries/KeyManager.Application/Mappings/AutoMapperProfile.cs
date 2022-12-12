using KeyManager.Application.Commands;
using KeyManager.Application.Commands.Doors;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Application.Commands.Roles;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Application.Commands.Users;
using KeyManager.Infrastructure.Security;

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
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.Password, opt
                => opt.MapFrom((src, dest)
                    => ClayPasswordHasher.HashPassword(dest, src.Password)));
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<Permission, CreatePermissionCommand>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Permission, UpdatePermissionCommand>().ReverseMap();
        CreateMap<UserRole, CreateUserRoleCommand>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
        CreateMap<UserRole, UpdateUserRoleCommand>().ReverseMap();
    }
}