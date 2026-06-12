using MediatR;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Commands;

public record CreateLaboratoryRequest(
    int? BuildingId,
    int? FloorLevel,
    string RoomId,
    string Color,
    string Texture,
    float Width,
    float Length,
    float Height,
    float XCoordinate,
    float YCoordinate,
    float ZCoordinate) : IRequest<bool>, ICreateLearningSpaceRequest;
