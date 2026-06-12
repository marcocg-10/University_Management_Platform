using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceTestData
{
    /// <summary>
    /// An empty learning space list
    /// </summary>
    public List<LearningSpace> LearningSpaceEmptyData { get; } = [];

    /// <summary>
    /// A single learning space
    /// </summary>
    public List<LearningSpace> LearningSpaceSingleEntryData { get; } = [
        new Laboratory(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f))];

    /// <summary>
    /// Multiple learning spaces of different types.
    /// </summary>
    public List<LearningSpace> LearningSpaceMultipleEntryData { get; } = [
       new Laboratory(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f)),
        new Classroom(
            2,
            4,
            7,
            "7-7",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(2.2f, 3.3f, 4.4f),
            LearningSpaceCoordinates.Create(2.2f, 3.3f, 4.4f))];


    /// <summary>
    /// An empty laboratory
    /// </summary>
    public List<Laboratory> LaboratoryEmptyData { get; } = [];

    /// <summary>
    /// A single laboratory
    /// </summary>
    public List<Laboratory> LaboratorySingleEntryData { get; } = [
        new Laboratory(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f))];

    /// <summary>
    /// Multiple laboratories.
    /// </summary>
    public List<Laboratory> LaboratoryMultipleEntryData { get; } = [
       new Laboratory(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f)),
        new Laboratory(
            2,
            4,
            7,
            "7-7",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(2.2f, 3.3f, 4.4f),
            LearningSpaceCoordinates.Create(2.2f, 3.3f, 4.4f))];

    /// <summary>
    /// An empty classroom
    /// </summary>
    public List<Classroom> ClassroomEmptyData { get; } = [];

    /// <summary>
    /// A single classroom
    /// </summary>
    public List<Classroom> ClassroomSingleEntryData { get; } = [
        new Classroom(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T10_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f))];

    /// <summary>
    /// Multiple classrooms.
    /// </summary>
    public List<Classroom> ClassroomMultipleEntryData { get; } = [
       new Classroom(
            1,
            3,
            6,
            "6-6",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Floor_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f),
            LearningSpaceCoordinates.Create(1.1f, 2.2f, 3.3f)),
        new Classroom(
            2,
            4,
            7,
            "7-7",
            LearningSpaceColor.Create("#000000"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(2.2f, 3.3f, 4.4f),
            LearningSpaceCoordinates.Create(2.2f, 3.3f, 4.4f))];
}
