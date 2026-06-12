using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for updating a laboratory.
/// </summary>
public static class UpdateLaboratoryHandler
{
    /// <summary>
    /// Handles the update of a laboratory.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="laboratoryId">Laboratory's unique ID from the route.</param>
    /// <param name="updateLaboratoryRequest">Laboratory update data without ID.</param>
    /// <returns>UpdateLaboratoryResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<UpdateLaboratoryResponse>,
        NotFound<LearningSpaceNotFoundErrorResponse>,
        BadRequest<LearningSpaceValidationErrorResponse>,
        Conflict<LearningSpaceConflictErrorResponse>>> HandleAsync(
            [FromServices] ILearningSpaceService learningSpaceService,
            int laboratoryId,
            [FromBody] UpdateLaboratoryRequest updateLaboratoryRequest)
    {
        // Validate RoomId first since it's required
        if (string.IsNullOrWhiteSpace(updateLaboratoryRequest.RoomId))
        {
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse("Room ID is required and cannot be empty."));
        }

        // Parse and validate BuildingId (optional)
        int? buildingId = null;
        if (!string.IsNullOrWhiteSpace(updateLaboratoryRequest.BuildingId))
        {
            if (!int.TryParse(updateLaboratoryRequest.BuildingId, out var parsedBuildingId))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Building ID is invalid. The expected value is a number (e.g. 1, 2, 3). "
                    + $"You sent '{updateLaboratoryRequest.BuildingId}'."));
            }
            buildingId = parsedBuildingId;
        }

        // Parse and validate FloorLevel (optional)
        int? floorLevel = null;
        if (!string.IsNullOrWhiteSpace(updateLaboratoryRequest.FloorLevel))
        {
            if (!int.TryParse(updateLaboratoryRequest.FloorLevel, out var parsedFloorLevel))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Floor Level is invalid. The expected value is a number (e.g. 1, 2, 3). "
                    + $"You sent '{updateLaboratoryRequest.FloorLevel}'."));
            }
            floorLevel = parsedFloorLevel;
        }

        // Handle optional Color
        string colorValue = "#CDCECF";

        if (!string.IsNullOrWhiteSpace(updateLaboratoryRequest.Color))
        {
            try
            {
                _ = LearningSpaceColor.Create(updateLaboratoryRequest.Color.Trim());
                colorValue = updateLaboratoryRequest.Color.Trim();
            }
            catch (ValidationException exception)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(exception.Message));
            }
        }

        // Handle Texture
        string textureValue = null;

        if (!string.IsNullOrWhiteSpace(updateLaboratoryRequest.Texture))
        {
            try
            {
                _ = LearningSpaceTexture.Create(updateLaboratoryRequest.Texture.Trim());
                textureValue = updateLaboratoryRequest.Texture.Trim();
            }
            catch (ValidationException exception)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(exception.Message));
            }
        }
        // Parse and validate Width
        if (!float.TryParse(updateLaboratoryRequest.Width, NumberStyles.Float, CultureInfo.InvariantCulture, out var width))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Width is invalid. "
                + "The expected value is a number that can include decimals (e.g. 10.5). "
                + $"You sent '{updateLaboratoryRequest.Width}'."));
        }

        // Parse and validate Length
        if (!float.TryParse(updateLaboratoryRequest.Length, NumberStyles.Float, CultureInfo.InvariantCulture, out var length))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Length is invalid. "
                + "The expected value is a number that can include decimals (e.g. 15.0). "
                + $"You sent '{updateLaboratoryRequest.Length}'."));
        }

        // Parse and validate Height
        if (!float.TryParse(updateLaboratoryRequest.Height, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Height is invalid. "
                + "The expected value is a number that can include decimals (e.g. 3.5). "
                + $"You sent '{updateLaboratoryRequest.Height}'."));
        }

        // Parse and validate X Coordinate
        if (!float.TryParse(updateLaboratoryRequest.XCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var xCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The X Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 25.75). "
                + $"You sent '{updateLaboratoryRequest.XCoordinate}'."));
        }

        // Parse and validate Y Coordinate
        if (!float.TryParse(updateLaboratoryRequest.YCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var yCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Y Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 42.30). "
                + $"You sent '{updateLaboratoryRequest.YCoordinate}'."));
        }

        // Parse and validate Z Coordinate
        if (!float.TryParse(updateLaboratoryRequest.ZCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var zCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Z Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 10.55). "
                + $"You sent '{updateLaboratoryRequest.ZCoordinate}'."));
        }

        Laboratory laboratory;

        try
        {
            // Call the service to update the laboratory
            laboratory = await learningSpaceService.UpdateLaboratoryAsync(
                laboratoryId,
                buildingId,
                floorLevel,
                updateLaboratoryRequest.RoomId,
                colorValue,
                textureValue,
                width,
                length,
                height,
                xCoordinate,
                yCoordinate,
                zCoordinate);
        }
        catch (LearningSpaceNotFoundException exception)
        {
            return TypedResults.NotFound(
                new LearningSpaceNotFoundErrorResponse(exception.Message));
        }
        catch (ValidationException exception)
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse(exception.Message));
        }
        catch (DuplicateValueInEntityException exception)
        {
            // This exception should be caught to provide a more user-friendly message.
            string userFriendlyMessage;

            // Check if the exception is due to the UNIQUE_Room_Building constraint
            if (exception.Message.Contains("UNIQUE_Room_Building", StringComparison.OrdinalIgnoreCase))
            {
                var roomIdFromRequest = updateLaboratoryRequest.RoomId?.Trim() ?? "unknown";

                // Differentiate message based on whether BuildingId was provided.
                if (buildingId is null)
                {
                    userFriendlyMessage =
                        $"A learning space not associated with any building and with room ID '{roomIdFromRequest}' already exists.";
                }
                else
                {
                    userFriendlyMessage =
                        $"A learning space with room ID '{roomIdFromRequest}' already exists in building {buildingId}.";
                }
            }
            else
            {
                userFriendlyMessage = exception.Message;
            }

            return TypedResults.Conflict(new LearningSpaceConflictErrorResponse(userFriendlyMessage));
        }

        catch (ForeignKeyException exception)
        {
            // This exception should be caught to provide a more user-friendly message.
            string userFriendlyMessage;

            // Check if it is due to the FK_LearningSpace_Building constraint
            if (exception.Message.Contains("FK_LearningSpace_Building", StringComparison.OrdinalIgnoreCase))
            {
                var idText = buildingId?.ToString() ?? "unknown";
                userFriendlyMessage = $"The specified building with ID '{idText}' does not exist. Please verify the Building ID.";

                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(userFriendlyMessage));
            }

            // Check if it is due to the FK_LearningSpace_Texture constraint
            if (exception.Message.Contains("FK_LearningSpace_Texture", StringComparison.OrdinalIgnoreCase))
            {
                var textureText = string.IsNullOrWhiteSpace(textureValue) ? "unknown" : textureValue;
                userFriendlyMessage = $"The specified texture '{textureText}' does not exist. Please verify the Texture value.";
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(userFriendlyMessage));
            }

            // Fallback
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse(
                "A foreign key constraint failed. Please verify the provided references."));
        }
        catch (LearningSpaceCollisionException ex)
        {
            return TypedResults.Conflict(
                new LearningSpaceConflictErrorResponse(ex.Message));
        }
        var laboratoryDto = LearningSpaceDtoMapper.ToDto(laboratory);
        var response = new UpdateLaboratoryResponse(laboratoryDto);

        return TypedResults.Ok(response);
    }
}
