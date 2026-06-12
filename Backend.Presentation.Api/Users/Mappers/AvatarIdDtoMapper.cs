using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;

internal static class AvatarIdDtoMapper
{
    internal static AvatarIdDto ToDto(string avatarId)
    {
        return new AvatarIdDto(avatarId);
    }

    internal static bool ToEntity(this AvatarIdDto dto, out AvatarId? avatarId, out string? error)
    {
        avatarId = null;
        error = null;
        if (!AvatarId.TryCreate(dto.AvatarId, out avatarId, out error))
        {
            return false;
        }
        return true;
    }
}