using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handles the retrieval of a paginated list of classrooms.
/// </summary>
/// <remarks>This method retrieves a specific page of classrooms from the data source, along with metadata about the
/// pagination state. The classrooms are mapped to DTOs before being returned in the response.</remarks>
public static class ListClassroomsPagedHandler
{
    /// <summary>
    /// Retrieves a paginated list of classrooms and associated pagination metadata.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be 1 or greater.</param>
    /// <param name="pageSize">The number of items per page. Must be between 1 and 100, inclusive.</param>
    /// <param name="learningSpaceService">The service used to retrieve the classrooms. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="ListClassroomsPagedResponse"/> containing the list of classrooms for the specified page and the associated
    /// pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="learningSpaceService"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1, or if <paramref name="pageSize"/> is not between 1 and
    /// 100.</exception>
    public static async Task<ListClassroomsPagedResponse> HandleAsync(
        int pageNumber,
        int pageSize,
        string? keyword,
        ILearningSpaceService learningSpaceService)
    {
        if (learningSpaceService is null)
        {
            throw new ArgumentNullException(nameof(learningSpaceService));
        }

        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
        }

        if (pageSize < 1 || pageSize > 100) // Max 100 items per page
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
        }

        var (classrooms, totalCount) = await learningSpaceService
            .ListClassroomsPagedAsync(pageNumber, pageSize, keyword)
            .ConfigureAwait(false);

        var classroomDtos = classrooms
            .Select(LearningSpaceDtoMapper.ToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return new ListClassroomsPagedResponse(classroomDtos, paginationMetadata);
    }
}