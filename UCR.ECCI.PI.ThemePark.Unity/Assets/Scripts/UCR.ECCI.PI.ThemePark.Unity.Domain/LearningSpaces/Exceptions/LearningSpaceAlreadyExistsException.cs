using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a learning space that already exists.
    /// </summary>
    public class LearningSpaceAlreadyExistsException : DomainException
    {
        /// <summary>
        /// Gets the room identifier that already exists.
        /// </summary>
        public string RoomId { get; }

        /// <summary>
        /// Gets the building identifier where the room already exists.
        /// </summary>
        public int? BuildingId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningSpaceAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="roomId">The room identifier that already exists.</param>
        /// <param name="buildingId">The building identifier where the room already exists.</param>
        public LearningSpaceAlreadyExistsException(string roomId, int? buildingId)
            : base($"A learning space with room ID '{roomId}' already exists in building {buildingId?.ToString() ?? "unspecified"}.")
        {
            RoomId = roomId;
            BuildingId = buildingId;
        }
    }
}
