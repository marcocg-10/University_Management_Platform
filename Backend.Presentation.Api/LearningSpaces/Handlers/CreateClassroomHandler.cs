using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for creating a classroom.
/// </summary>
public static class CreateClassroomHandler
{
    /// <summary>
    /// Handles the creation of a classroom.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="createClassroomRequest">Classroom creation data with string-based parameters.</param>
    /// <returns>CreateClassroomResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<CreateClassroomResponse>,
        BadRequest<LearningSpaceValidationErrorResponse>,
        Conflict<LearningSpaceConflictErrorResponse>>> HandleAsync(
            [FromServices] ILearningSpaceService learningSpaceService,
            [FromBody] CreateClassroomRequest createClassroomRequest)
    {
        // Validate RoomId first since it's required
        var rawRoomId = createClassroomRequest.RoomId;
        if (string.IsNullOrWhiteSpace(rawRoomId))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("Room ID is required and cannot be empty."));
        }
        var roomId = rawRoomId.Trim();

        // Parse and validate BuildingId (optional)
        int? buildingId = null;
        if (!string.IsNullOrWhiteSpace(createClassroomRequest.BuildingId))
        {
            if (!int.TryParse(createClassroomRequest.BuildingId.Trim(), out var parsedBuildingId))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Building ID is invalid. The expected value is a number (e.g. 1, 2, 3). "
                    + $"You sent '{createClassroomRequest.BuildingId}'."));
            }
            buildingId = parsedBuildingId;
        }

        // Parse and validate FloorLevel (optional)
        int? floorLevel = null;
        if (!string.IsNullOrWhiteSpace(createClassroomRequest.FloorLevel))
        {
            if (!int.TryParse(createClassroomRequest.FloorLevel.Trim(), out var parsedFloorLevel))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Floor Level is invalid. The expected value is a number (e.g. 1, 2, 3). "
                    + $"You sent '{createClassroomRequest.FloorLevel}'."));
            }
            floorLevel = parsedFloorLevel;
        }

        // Handle optional Color
        string colorValue = "#CDCECF";

        if (!string.IsNullOrWhiteSpace(createClassroomRequest.Color))
        {
            var trimmedColor = createClassroomRequest.Color.Trim();
            try
            {
                _ = LearningSpaceColor.Create(trimmedColor);
                colorValue = trimmedColor;
            }
            catch (ValidationException ex)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(ex.Message));
            }
        }

        // Handle Texture
        string? textureValue = null;

        if (!string.IsNullOrWhiteSpace(createClassroomRequest.Texture))
        {
            var trimmedTexture = createClassroomRequest.Texture.Trim();
            try
            {
                _ = LearningSpaceTexture.Create(trimmedTexture);
                textureValue = trimmedTexture;
            }
            catch (ValidationException ex)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(ex.Message));
            }
            catch (LearningSpaceDataException ex)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(ex.Message));
            }
        }

        // Parse and validate Length
        static bool TryParseFloat(string? raw, string fieldName, out float value, out string errorMessage)
        {
            if (float.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                errorMessage = "";
                return true;
            }

            errorMessage = $"The {fieldName} is invalid. The expected value is a number that can include decimals (e.g. 10.5). You sent '{raw}'.";
            return false;
        }

        if (!TryParseFloat(createClassroomRequest.Width, "Width", out var width, out var widthErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(widthErr));

        if (!TryParseFloat(createClassroomRequest.Length, "Length", out var length, out var lengthErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(lengthErr));

        if (!TryParseFloat(createClassroomRequest.Height, "Height", out var height, out var heightErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(heightErr));

        if (!TryParseFloat(createClassroomRequest.XCoordinate, "X Coordinate", out var xCoordinate, out var xErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(xErr));

        if (!TryParseFloat(createClassroomRequest.YCoordinate, "Y Coordinate", out var yCoordinate, out var yErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(yErr));

        if (!TryParseFloat(createClassroomRequest.ZCoordinate, "Z Coordinate", out var zCoordinate, out var zErr))
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse(zErr));

        Classroom classroom;

        try
        {
            // Call the service to create the classroom
            classroom = await learningSpaceService.CreateClassroomAsync(
                buildingId,
                floorLevel,
                roomId,
                colorValue,
                textureValue,
                width,
                length,
                height,
                xCoordinate,
                yCoordinate,
                zCoordinate);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse(ex.Message));
        }
        catch (DuplicateValueInEntityException ex)
        {
            string userFriendlyMessage;
            if (ex.Message.Contains("UNIQUE_Room_Building", StringComparison.OrdinalIgnoreCase))
            {
                userFriendlyMessage = buildingId is null
                    ? $"A learning space not associated with any building and with room ID '{roomId}' already exists."
                    : $"A learning space with room ID '{roomId}' already exists in building {buildingId}.";
            }
            else
            {
                userFriendlyMessage = ex.Message;
            }

            return TypedResults.Conflict(
                new LearningSpaceConflictErrorResponse(userFriendlyMessage));
        }
        catch (ForeignKeyException ex)
        {
            if (ex.Message.Contains("FK_LearningSpace_Building", StringComparison.OrdinalIgnoreCase))
            {
                var idText = buildingId?.ToString() ?? "unknown";
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(
                        $"The specified building with ID '{idText}' does not exist. Please verify the Building ID."));
            }

            if (ex.Message.Contains("FK_LearningSpace_Texture", StringComparison.OrdinalIgnoreCase))
            {
                var textureText = string.IsNullOrWhiteSpace(textureValue) ? "unknown" : textureValue;
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(
                        $"The specified texture '{textureText}' does not exist. Please verify the Texture value."));
            }

            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse(
                    "A foreign key constraint failed. Please verify the provided references."));
        }
        catch (LearningSpaceCollisionException ex)
        {
            return TypedResults.Conflict(
                new LearningSpaceConflictErrorResponse(ex.Message));
        }

        var classroomDto = LearningSpaceDtoMapper.ToDto(classroom);
        var response = new CreateClassroomResponse(classroomDto);

        return TypedResults.Ok(response);
    }
}
