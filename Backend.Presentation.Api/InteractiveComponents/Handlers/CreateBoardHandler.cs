using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles the creation of a <see cref="Board"/> interactive component via the API.
/// </summary>
public static class CreateBoardHandler
{
    /// <summary>
    /// Processes a request to create a new board.
    /// </summary>
    /// <param name="createBoardRequest">
    /// The request object containing all board creation details, including:
    /// <list type="bullet">
    /// <item>Color</item>
    /// <item>MarkerColor</item>
    /// <item>Texture</item>
    /// <item>PlateId</item>
    /// <item>CoordinateX, CoordinateY, CoordinateZ</item>
    /// <item>Width, Height, Depth</item>
    /// <item>XAxisRotation, YAxisRotation, ZAxisRotation</item>
    /// <item>LearningSpaceId</item>
    /// </list>
    /// </param>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for business logic related to interactive components.
    /// </param>
    internal static async Task<Results<
        Ok<CreateBoardResponse>,
        BadRequest<InteractiveComponentValidationErorrResponse>,
        Conflict<InteractiveComponentConflictErrorResponse>>> HandleAsync(
            [FromServices] IInteractiveComponentService interactiveComponentService,
            [FromBody] CreateBoardRequest createBoardRequest)
    {
        Board entity;
        try
        {
            entity = createBoardRequest.Board.ToEntity();
            entity = await interactiveComponentService.CreateBoardAsync(
                entity.Color.Value,
                entity.MarkerColor.Value,
                entity.Texture,
                entity.PlateId.Value,
                entity.Coordinates.X,
                entity.Coordinates.Y,
                entity.Coordinates.Z,
                entity.Dimensions.Width,
                entity.Dimensions.Height,
                entity.Dimensions.Depth,
                entity.Rotations.XAxisRotation,
                entity.Rotations.YAxisRotation,
                entity.Rotations.ZAxisRotation,
                entity.LearningSpaceId);
        }
        catch (InteractiveComponentException exception)
        {
            return TypedResults.BadRequest(
                new InteractiveComponentValidationErorrResponse(exception.Message));
        }
        catch (DuplicateValueInEntityException exception)
        {
            string userFriendlyMessage;

            var plateIdFromRequest = createBoardRequest.Board.PlateId ?? "unknown";
  
            userFriendlyMessage =
                $"An Interactive Component with Plate ID '{plateIdFromRequest}' already exists.";

            return TypedResults.Conflict(new InteractiveComponentConflictErrorResponse(userFriendlyMessage));
        }
        catch (ForeignKeyException exception)
        {
            string userFriendlyMessage;

            if (exception.Message.Contains("FK_InteractiveComponent_LearningSpace", StringComparison.OrdinalIgnoreCase))
            {
                var idText = createBoardRequest.Board.LearningSpaceId.ToString() ?? "unknown";
                userFriendlyMessage = $"The specified building with ID '{idText}' does not exist. Please verify the Building ID.";

                return TypedResults.BadRequest(
                    new InteractiveComponentValidationErorrResponse(userFriendlyMessage));
            }

            // Fallback
            return TypedResults.BadRequest(
                new InteractiveComponentValidationErorrResponse(
                "A foreign key constraint failed. Please verify the provided references."));
        }

        return TypedResults.Ok(
            new CreateBoardResponse(entity.ToDto()));
    }
}
