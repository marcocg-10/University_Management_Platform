using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api;

/// <summary>
/// Provides methods for registering learning space endpoints. 
/// </summary>
internal static class LearningSpaceEndpoints
{
    /// <summary>
    /// Maps the learning space endpoints on the specified application route.
    /// </summary>
    /// <param name="routes">The route builder used to register the learning space routes.</param>
    /// <returns>The route builder instance.</returns>
    internal static IEndpointRouteBuilder MapLearningSpaceEndpoints(this IEndpointRouteBuilder routes)
    {
        // Map learning space endpoints using the appropriate handler.
        // Create laboratory.
        routes
            .MapPost("/laboratories", CreateLaboratoryHandler.HandleAsync)
            .WithName("CreateLaboratory").RequireAuthorization("ManageLearningSpaces");

        // List LearningSpaces by BuildingId
        routes
            .MapGet("/buildings/{buildingId:int}/learningspaces", ListLearningSpaceByBuildingIdHandler.HandleAsync)
            .WithName("ListLearningSpacesByBuildingId").RequireAuthorization("ListLearningSpaces");

        // List laboratories.
        routes
            .MapGet("/laboratories", ListLaboratoriesHandler.HandleAsync)
            .WithName("ListLaboratories").RequireAuthorization("ListLearningSpaces");

        // Read laboratory by ID.
        routes
            .MapGet("/laboratories/{laboratoryId:int}", GetLaboratoryHandler.HandleAsync)
            .WithName("ReadLaboratory").RequireAuthorization("ManageLearningSpaces");

        // Update laboratory.
        routes
            .MapPut("/laboratories/{laboratoryId:int}", UpdateLaboratoryHandler.HandleAsync)
            .WithName("UpdateLaboratory").RequireAuthorization("ManageLearningSpaces");

        // Get classroom by id.
        routes
            .MapGet("/classrooms/{classroomId:int}", GetClassroomHandler.HandleAsync)
            .WithName("GetClassroom").RequireAuthorization("ManageLearningSpaces");

        // Delete classroom.
        routes
            .MapDelete("/classrooms/{classroomId:int}", DeleteClassroomHandler.HandleAsync)
            .WithName("DeleteClassroom").RequireAuthorization("ManageLearningSpaces");

        // Delete laboratory.
        routes
            .MapDelete("/laboratories/{laboratoryId:int}", DeleteLaboratoryHandler.HandleAsync)
            .WithName("DeleteLaboratory").RequireAuthorization("ManageLearningSpaces");

        // Map learning space endpoints using the appropriate handler.
        // Create classroom.
        routes
            .MapPost("/classrooms", CreateClassroomHandler.HandleAsync)
            .WithName("CreateClassroom").RequireAuthorization("ManageLearningSpaces");

        // List classrooms.
        routes
            .MapGet("/classrooms", ListClassroomsHandler.HandleAsync)
            .WithName("ListClassrooms").RequireAuthorization("ListLearningSpaces");

        // Update classroom.
        routes
            .MapPut("/classrooms/{classroomId:int}", UpdateClassroomHandler.HandleAsync)
            .WithName("UpdateClassroom").RequireAuthorization("ManageLearningSpaces");

        // Get any learning space by ID (global endpoint).
        routes
            .MapGet("/learningspaces/{learningSpaceId:int}", GetLearningSpaceHandler.HandleAsync)
            .WithName("GetLearningSpace").RequireAuthorization("ManageLearningSpaces");

        // Get laboratories paged.
        routes
            .MapGet("/laboratories/paged", ListLaboratoriesPagedHandler.HandleAsync)
            .WithName("ListLaboratoriesPaginated").RequireAuthorization("ListLearningSpaces");

        // Get classrooms paged.
        routes
            .MapGet("/classrooms/paged", ListClassroomsPagedHandler.HandleAsync)
            .WithName("ListClassroomsPaginated").RequireAuthorization("ListLearningSpaces");

        return routes;
    }
}
