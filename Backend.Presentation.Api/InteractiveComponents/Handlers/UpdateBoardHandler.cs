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
/// Handles requests to update an existing <see cref="Board"/> by its unique PlateId.
/// </summary>
public static class UpdateBoardHandler
{
    /// <summary>
    /// Processes a request to update a board identified by <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">
    /// The unique identifier of the board to update.
    /// </param>
    /// <param name="updateBoardRequest">
    /// The request object containing updated values for the board's properties:
    /// - <see cref="UpdateBoardRequest.Color"/>
    /// - <see cref="UpdateBoardRequest.MarkerColor"/>
    /// - <see cref="UpdateBoardRequest.Texture"/>
    /// - <see cref="UpdateBoardRequest.CoordinateX"/>
    /// - <see cref="UpdateBoardRequest.CoordinateY"/>
    /// - <see cref="UpdateBoardRequest.CoordinateZ"/>
    /// - <see cref="UpdateBoardRequest.Width"/>
    /// - <see cref="UpdateBoardRequest.Height"/>
    /// - <see cref="UpdateBoardRequest.Depth"/>
    /// - <see cref="UpdateBoardRequest.LearningSpaceId"/>
    /// </param>
    /// <param name="interactiveComponentService">
    /// Service layer responsible for performing the update operation on the board.
    /// Must not be null.
    /// </param>
    internal static async Task<Results<
    Ok<UpdateBoardResponse>,
    BadRequest<InteractiveComponentValidationErorrResponse>,
    Conflict<InteractiveComponentConflictErrorResponse>>> HandleAsync(
        string plateId,
        [FromServices] IInteractiveComponentService interactiveComponentService,
        [FromBody] UpdateBoardRequest updateBoardRequest)
    {
        Board entity;
        try
        {
            entity = updateBoardRequest.Board.ToEntity();
            entity = await interactiveComponentService.UpdateBoardAsync(
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

            var plateIdFromRequest = updateBoardRequest.Board.PlateId ?? "unknown";

            userFriendlyMessage =
                $"An Interactive Component with Plate ID '{plateIdFromRequest}' already exists.";

            return TypedResults.Conflict(new InteractiveComponentConflictErrorResponse(userFriendlyMessage));
        }
        catch (ForeignKeyException exception)
        {
            string userFriendlyMessage;

            if (exception.Message.Contains("FK_InteractiveComponent_LearningSpace", StringComparison.OrdinalIgnoreCase))
            {
                var idText = updateBoardRequest.Board.LearningSpaceId.ToString() ?? "unknown";
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
            new UpdateBoardResponse(entity.ToDto()));
    }
}
