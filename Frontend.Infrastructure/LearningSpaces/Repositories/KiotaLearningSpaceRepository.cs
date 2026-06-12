using System.Globalization;
using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Exceptions.Classroom;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Exceptions.Laboratory;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.LearningSpaces.Mappers;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.LearningSpaces.Repositories;

/// <summary>
/// Repository implementation for managing Learning Spaces.
/// </summary>
/// <remarks>
/// Uses Kiota-generated API client to perform operations.
/// </remarks>
internal class KiotaLearningSpaceRepository : ILearningSpaceRepository
{
    /// <summary>
    /// Represents the API client used to interact with external services.
    /// </summary>
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="apiClient">The API client used to interact with the remote service.</param>
    public KiotaLearningSpaceRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously adds a new laboratory to the repository through the API.
    /// </summary>
    /// <remarks>
    /// This method converts the laboratory domain entity to a CreateLaboratoryRequest,
    /// sends it to the backend API, and handles the response. All numeric values
    /// are converted using InvariantCulture to ensure consistent formatting
    /// regardless of the system's locale settings.
    /// </remarks>
    /// <param name="laboratory">
    /// The laboratory entity to add to the repository. Must not be null and must
    /// contain valid dimensions and coordinates.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous add operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="laboratory"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API call fails or returns an unexpected response.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there are network connectivity issues or HTTP-level errors.
    /// </exception>
    public async Task AddLaboratoryAsync(Laboratory laboratory)
    {
        if (laboratory is null)
            throw new ArgumentNullException(nameof(laboratory), "laboratory cannot be null.");

        var request = new CreateLaboratoryRequest
        {
            BuildingId = laboratory.BuildingId is null ? null : laboratory.BuildingId.Value.ToString(),
            FloorLevel = laboratory.FloorLevel is null ? null : laboratory.FloorLevel.Value.ToString(),
            RoomId = laboratory.RoomId,
            Color = laboratory.Color.Value.ToString(),
            Texture = laboratory.Texture.Value.ToString(),
            Width = laboratory.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
            Length = laboratory.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
            Height = laboratory.Dimensions.Height.ToString(CultureInfo.InvariantCulture),
            XCoordinate = laboratory.Coordinates.XCoordinate.ToString(CultureInfo.InvariantCulture),
            YCoordinate = laboratory.Coordinates.YCoordinate.ToString(CultureInfo.InvariantCulture),
            ZCoordinate = laboratory.Coordinates.ZCoordinate.ToString(CultureInfo.InvariantCulture),
        };

        try
        {
            await _apiClient.Laboratories.PostAsync(request);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while adding the laboratory.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while adding the laboratory.");
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while adding the laboratory.");
        }
    }

    public async Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
    {
        var response = await _apiClient.Laboratories.GetAsync();

        var laboratories = response?.Laboratories?.Select(LaboratoryDtoMapper.ToEntity)
            ?? Enumerable.Empty<Laboratory>();

        return laboratories;
    }

    /// <summary>
    /// Deletes a <see cref="Laboratory"/> from the database by its <paramref name="Id"/>.
    /// </summary>
    /// <param name="Id">The unique identifier of the laboratory to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="Id"/> is invalid (less than or equal to 0).
    /// </exception>
    /// <exception cref="LaboratoryNotFoundException">
    /// Thrown if the laboratory does not exist in the database.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when validation errors, conflicts, or other API errors occur.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API call fails or returns an unexpected response.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there are network connectivity issues or HTTP-level errors.
    /// </exception>
    public async Task DeleteLaboratoryAsync(int Id)
    {
        if (Id <= 0)
            throw new ArgumentException("A valid laboratory ID is required.", nameof(Id));

        try
        {
            await _apiClient
                .Laboratories[Id]
                .DeleteAsync()
                .ConfigureAwait(false);
        }
        catch (LearningSpaceNotFoundErrorResponse notFoundError)
        {
            throw new LaboratoryNotFoundException(Id);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while deleting the laboratory.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while deleting the laboratory. The laboratory may be in use by other components.");
        }
        catch (ApiException ex) when ((int?)ex.ResponseStatusCode == 404)
        {
            throw new LaboratoryNotFoundException(Id);
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while deleting the laboratory.");
        }
    }

    /// <summary>
    /// Asynchronously updates an existing laboratory in the repository through the API.
    /// </summary>
    /// <remarks>
    /// This method converts the laboratory domain entity to an UpdateLaboratoryRequest,
    /// sends it to the backend API, and handles the response. All numeric values
    /// are converted using InvariantCulture to ensure consistent formatting
    /// regardless of the system's locale settings. The laboratory ID is used to
    /// target the specific resource for updating.
    /// </remarks>
    /// <param name="laboratory">
    /// The laboratory entity to update in the repository. Must not be null and must
    /// contain a valid ID along with updated dimensions and coordinates.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous update operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="laboratory"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API call fails or returns an unexpected response.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there are network connectivity issues or HTTP-level errors.
    /// </exception>
    public async Task UpdateLaboratoryAsync(Laboratory laboratory)
    {
        if (laboratory is null)
            throw new ArgumentNullException(nameof(laboratory), "Laboratory cannot be null.");

        var request = new UpdateLaboratoryRequest
        {
            BuildingId = laboratory.BuildingId is null ? null : laboratory.BuildingId.Value.ToString(),
            FloorLevel = laboratory.FloorLevel is null ? null : laboratory.FloorLevel.Value.ToString(),
            RoomId = laboratory.RoomId,
            Color = laboratory.Color.Value.ToString(),
            Texture = laboratory.Texture.Value.ToString(),
            Width = laboratory.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
            Length = laboratory.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
            Height = laboratory.Dimensions.Height.ToString(CultureInfo.InvariantCulture),
            XCoordinate = laboratory.Coordinates.XCoordinate.ToString(CultureInfo.InvariantCulture),
            YCoordinate = laboratory.Coordinates.YCoordinate.ToString(CultureInfo.InvariantCulture),
            ZCoordinate = laboratory.Coordinates.ZCoordinate.ToString(CultureInfo.InvariantCulture),
        };

        try
        {
            await _apiClient.Laboratories[laboratory.Id].PutAsync(request);
        }
        catch (LearningSpaceNotFoundErrorResponse notFoundError)
        {
            throw new LaboratoryNotFoundException(laboratory.Id);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while updating the laboratory.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while updating the laboratory.");
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while updating the laboratory.");
        }
    }

    public async Task AddClassroomAsync(Classroom classroom)
    {
        if (classroom is null)
            throw new ArgumentNullException(nameof(classroom), "Classroom cannot be null.");

        var request = new CreateClassroomRequest
        {
            BuildingId = classroom.BuildingId is null ? null : classroom.BuildingId.Value.ToString(),
            FloorLevel = classroom.FloorLevel is null ? null : classroom.FloorLevel.Value.ToString(),
            RoomId = classroom.RoomId,
            Color = classroom.Color.Value.ToString(),
            Texture = classroom.Texture.Value.ToString(),
            Width = classroom.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
            Length = classroom.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
            Height = classroom.Dimensions.Height.ToString(CultureInfo.InvariantCulture),
            XCoordinate = classroom.Coordinates.XCoordinate.ToString(CultureInfo.InvariantCulture),
            YCoordinate = classroom.Coordinates.YCoordinate.ToString(CultureInfo.InvariantCulture),
            ZCoordinate = classroom.Coordinates.ZCoordinate.ToString(CultureInfo.InvariantCulture),
        };

        try
        {
            await _apiClient.Classrooms.PostAsync(request);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while adding the classroom.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while adding the classroom.");
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while adding the classroom.");
        }
    }

    public async Task<IEnumerable<Classroom>> ListClassroomsAsync()
    {
        var response = await _apiClient.Classrooms.GetAsync();

        var classrooms = response?.Classrooms?.Select(ClassroomDtoMapper.ToEntity)
            ?? Enumerable.Empty<Classroom>();

        return classrooms;
    }

    /// <summary>
    /// Deletes a <see cref="Classroom"/> from the database by its <paramref name="Id"/>.
    /// </summary>
    /// <param name="Id">The unique identifier of the classroom to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="Id"/> is invalid (less than or equal to 0).
    /// </exception>
    /// <exception cref="ClassroomNotFoundException">
    /// Thrown if the classroom does not exist in the database.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when validation errors, conflicts, or other API errors occur.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API call fails or returns an unexpected response.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there are network connectivity issues or HTTP-level errors.
    /// </exception>
    public async Task DeleteClassroomAsync(int Id)
    {
        if (Id <= 0)
            throw new ArgumentException("A valid classroom ID is required.", nameof(Id));

        try
        {
            await _apiClient
                .Classrooms[Id]
                .DeleteAsync()
                .ConfigureAwait(false);
        }
        catch (LearningSpaceNotFoundErrorResponse notFoundError)
        {
            throw new ClassroomNotFoundException(Id);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while deleting the classroom.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while deleting the classroom. The classroom may be in use by other components.");
        }
        catch (ApiException ex) when ((int?)ex.ResponseStatusCode == 404)
        {
            throw new ClassroomNotFoundException(Id);
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while deleting the classroom.");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of classrooms along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the classrooms for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// classrooms.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of classrooms to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter classrooms by name or other attributes.</param>
    /// <returns>A tuple containing the classrooms in the requested page and pagination metadata.</returns>
    public async Task<(IEnumerable<Classroom> Classrooms, PaginationMetadata Metadata)> ListClassroomsPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? keyword = null)
    {
        var response = await _apiClient.Classrooms.Paged.GetAsync(c =>
            {
                c.QueryParameters.PageNumber = pageNumber;
                c.QueryParameters.PageSize = pageSize;

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    c.QueryParameters.Keyword = keyword;
                }

            }).ConfigureAwait(false);

        var classrooms = response?.Classrooms?.Select(ClassroomDtoMapper.ToEntity) ?? Enumerable.Empty<Classroom>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (classrooms, metadata);
    }

    /// Asynchronously updates an existing classroom in the repository through the API.
    /// </summary>
    /// <remarks>
    /// This method converts the classroom domain entity to an UpdateClassroomRequest,
    /// sends it to the backend API, and handles the response. All numeric values
    /// are converted using InvariantCulture to ensure consistent formatting
    /// regardless of the system's locale settings. The classroom ID is used to
    /// target the specific resource for updating.
    /// </remarks>
    /// <param name="classroom">
    /// The classroom entity to update in the repository. Must not be null and must
    /// contain a valid ID along with updated dimensions and coordinates.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous update operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="classroom"/> is null.
    /// </exception>
    /// <exception cref="ClassroomNotFoundException">
    /// Thrown when the classroom with the specified ID does not exist.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when validation errors, conflicts, or other API errors occur.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API call fails or returns an unexpected response.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there are network connectivity issues or HTTP-level errors.
    /// </exception>
    public async Task UpdateClassroomAsync(Classroom classroom)
    {
        if (classroom is null)
            throw new ArgumentNullException(nameof(classroom), "Classroom cannot be null.");

        var request = new UpdateClassroomRequest
        {
            BuildingId = classroom.BuildingId is null ? null : classroom.BuildingId.Value.ToString(),
            FloorLevel = classroom.FloorLevel is null ? null : classroom.FloorLevel.Value.ToString(),
            RoomId = classroom.RoomId,
            Color = classroom.Color.Value.ToString(),
            Texture = classroom.Texture.Value.ToString(),
            Width = classroom.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
            Length = classroom.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
            Height = classroom.Dimensions.Height.ToString(CultureInfo.InvariantCulture),
            XCoordinate = classroom.Coordinates.XCoordinate.ToString(CultureInfo.InvariantCulture),
            YCoordinate = classroom.Coordinates.YCoordinate.ToString(CultureInfo.InvariantCulture),
            ZCoordinate = classroom.Coordinates.ZCoordinate.ToString(CultureInfo.InvariantCulture),
        };

        try
        {
            await _apiClient.Classrooms[classroom.Id].PutAsync(request);
        }
        catch (LearningSpaceNotFoundErrorResponse notFoundError)
        {
            throw new ClassroomNotFoundException(classroom.Id);
        }
        catch (LearningSpaceValidationErrorResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while updating the classroom.");
        }
        catch (LearningSpaceConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while updating the classroom.");
        }
        catch (ApiException)
        {
            throw new DomainException("An unexpected error occurred while updating the classroom.");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of laboratories along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the laboratories for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// laboratories.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of laboratories to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter laboratories by name or other attributes.</param>
    /// <returns>A tuple containing the laboratories in the requested page and pagination metadata.</returns>
    public async Task<(IEnumerable<Laboratory> Laboratories, PaginationMetadata Metadata)> ListLaboratoriesPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? keyword = null)
    {
        var response = await _apiClient.Laboratories.Paged.GetAsync(c =>
            {
                c.QueryParameters.PageNumber = pageNumber;
                c.QueryParameters.PageSize = pageSize;

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    c.QueryParameters.Keyword = keyword;
                }
            }).ConfigureAwait(false);

        var laboratories = response?.Laboratories?.Select(LaboratoryDtoMapper.ToEntity) ?? Enumerable.Empty<Laboratory>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (laboratories, metadata);
    }
}