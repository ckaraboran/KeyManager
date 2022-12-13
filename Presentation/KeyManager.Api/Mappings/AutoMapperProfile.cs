using KeyManager.Api.DTOs.Responses.Doors;
using KeyManager.Api.DTOs.Responses.Incident;
using KeyManager.Api.DTOs.Responses.Permissions;
using KeyManager.Api.DTOs.Responses.Roles;
using KeyManager.Api.DTOs.Responses.UserRoles;
using KeyManager.Api.DTOs.Responses.Users;

namespace KeyManager.Api.Mappings;

/// <summary>
///     AutoMapper profile for mapping between DTOs and entities
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    ///     Constructor for AutoMapperProfile
    /// </summary>
    public AutoMapperProfile()
    {
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
        CreateMap<UserRoleWithNamesDto, GetUserRoleResponse>();
        CreateMap<UserRoleDto, CreateUserRoleResponse>();
        CreateMap<UserRoleDto, UpdateUserRoleResponse>();
        CreateMap<IncidentWithNamesDto, GetIncidentResponse>();
    }
}