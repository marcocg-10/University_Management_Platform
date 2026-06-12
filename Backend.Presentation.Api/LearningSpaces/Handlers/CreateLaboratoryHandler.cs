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
/// Handler for creating a laboratory.
/// </summary>
public static class CreateLaboratoryHandler
{
    /// <summary>
    /// Handles the creation of a laboratory.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="createLaboratoryRequest">Laboratory creation data with string-based parameters.</param>
    /// <returns>CreateLaboratoryResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<CreateLaboratoryResponse>,
        BadRequest<LearningSpaceValidationErrorResponse>,
        Conflict<LearningSpaceConflictErrorResponse>>> HandleAsync(
            [FromServices] ILearningSpaceService learningSpaceService,
            [FromBody] CreateLaboratoryRequest createLaboratoryRequest)
    {
        // Validate RoomId first since it's required
        if (string.IsNullOrWhiteSpace(createLaboratoryRequest.RoomId))
        {
            return TypedResults.BadRequest(new LearningSpaceValidationErrorResponse("Room ID is required and cannot be empty."));
        }

        // Parse and validate BuildingId (optional)
        int? buildingId = null;
        if (!string.IsNullOrWhiteSpace(createLaboratoryRequest.BuildingId))
        {
            if (!int.TryParse(createLaboratoryRequest.BuildingId, out var parsedBuildingId))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Building ID is invalid. The expected value is a number (e.g. 1, 2, 3). " 
                    + $"You sent '{createLaboratoryRequest.BuildingId}'."));
            }
            buildingId = parsedBuildingId;
        }

        // Parse and validate FloorLevel (optional)
        int? floorLevel = null;
        if (!string.IsNullOrWhiteSpace(createLaboratoryRequest.FloorLevel))
        {
            if (!int.TryParse(createLaboratoryRequest.FloorLevel, out var parsedFloorLevel))
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse("The Floor Level is invalid. The expected value is a number (e.g. 1, 2, 3). " 
                    + $"You sent '{createLaboratoryRequest.FloorLevel}'."));
            }
            floorLevel = parsedFloorLevel;
        }

        // Handle optional Color
        string colorValue = "#CDCECF";

        if (!string.IsNullOrWhiteSpace(createLaboratoryRequest.Color))
        {
            try
            {
                _ = LearningSpaceColor.Create(createLaboratoryRequest.Color.Trim());
                colorValue = createLaboratoryRequest.Color.Trim();
            }
            catch (ValidationException exception)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(exception.Message));
            }
        }

        // Handle Texture
        string textureValue = null;

        if (!string.IsNullOrWhiteSpace(createLaboratoryRequest.Texture))
        {
            try
            {
                _ = LearningSpaceTexture.Create(createLaboratoryRequest.Texture.Trim());
                textureValue = createLaboratoryRequest.Texture.Trim();
            }
            catch (ValidationException exception)
            {
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(exception.Message));
            }
        }
        
        // Parse and validate Width
        if (!float.TryParse(createLaboratoryRequest.Width, NumberStyles.Float, CultureInfo.InvariantCulture, out var width))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Width is invalid. " 
                + "The expected value is a number that can include decimals (e.g. 10.5). "
                + $"You sent '{createLaboratoryRequest.Width}'.")); 
        }

        // Parse and validate Length
        if (!float.TryParse(createLaboratoryRequest.Length, NumberStyles.Float, CultureInfo.InvariantCulture, out var length))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Length is invalid. " 
                + "The expected value is a number that can include decimals (e.g. 15.0). "
                + $"You sent '{createLaboratoryRequest.Length}'."));
        }

        // Parse and validate Height
        if (!float.TryParse(createLaboratoryRequest.Height, NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Height is invalid. "
                + "The expected value is a number that can include decimals (e.g. 3.5). "
                + $"You sent '{createLaboratoryRequest.Height}'."));
        }

        // Parse and validate X Coordinate
        if (!float.TryParse(createLaboratoryRequest.XCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var xCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The X Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 25.75). "
                + $"You sent '{createLaboratoryRequest.XCoordinate}'."));
        }

        // Parse and validate Y Coordinate
        if (!float.TryParse(createLaboratoryRequest.YCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var yCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Y Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 42.30). "
                + $"You sent '{createLaboratoryRequest.YCoordinate}'."));
        }

        // Parse and validate Z Coordinate
        if (!float.TryParse(createLaboratoryRequest.ZCoordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out var zCoordinate))
        {
            return TypedResults.BadRequest(
                new LearningSpaceValidationErrorResponse("The Z Coordinate is invalid. "
                + "The expected value is a number that can include decimals (e.g. 10.55). "
                + $"You sent '{createLaboratoryRequest.ZCoordinate}'."));
        }

        Laboratory laboratory;

        try
        {
            // Call the service to create the laboratory
            laboratory = await learningSpaceService.CreateLaboratoryAsync(
                buildingId,
                floorLevel,
                createLaboratoryRequest.RoomId,
                colorValue,
                textureValue!,
                width,
                length,
                height,
                xCoordinate,
                yCoordinate,
                zCoordinate);
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
                var roomIdFromRequest = createLaboratoryRequest.RoomId?.Trim() ?? "unknown";

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
        catch (ForeignKeyException ex)
        {
            // This exception should be caught to provide a more user-friendly message.
            string userFriendlyMessage;

            // Check if it is due to the FK_LearningSpace_Building constraint
            if (ex.Message.Contains("FK_LearningSpace_Building", StringComparison.OrdinalIgnoreCase))
            {
                var idText = buildingId?.ToString() ?? "unknown";
                userFriendlyMessage = $"The specified building with ID '{idText}' does not exist. Please verify the Building ID.";
                
                return TypedResults.BadRequest(
                    new LearningSpaceValidationErrorResponse(userFriendlyMessage));
            }

            // Check if it is due to the FK_LearningSpace_Texture constraint
            if (ex.Message.Contains("FK_LearningSpace_Texture", StringComparison.OrdinalIgnoreCase))
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
        var response = new CreateLaboratoryResponse(laboratoryDto);

        return TypedResults.Ok(response);
    }
}
