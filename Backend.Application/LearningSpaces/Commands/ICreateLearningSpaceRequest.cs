namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Commands;

public interface ICreateLearningSpaceRequest
{
    public int? BuildingId { get; }
    public int? FloorLevel { get; }
    public string RoomId { get; }
    public float Width { get; }
    public float Length { get; }
    public float Height { get; }
}
