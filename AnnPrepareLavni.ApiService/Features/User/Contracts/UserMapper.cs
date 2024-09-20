using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.User.Contracts;

[Mapper]
public static partial class UserMapper
{
    public static partial UserResponse ToResponse(Models.User user);
    public static partial Models.User ToUser(UserRequest userRequest);
    public static partial void MapToExistingUser(UserRequest userRequest, Models.User existingUser);
}
