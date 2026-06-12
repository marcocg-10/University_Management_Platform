using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Repositories;

/// <summary>
/// Repository implementation that retrieves building data using the Kiota-generated API client.
/// </summary>
internal class KiotaBuildingRepository : IBuildingRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaBuildingRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota API client used to communicate with the backend service.</param>
    public KiotaBuildingRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of buildings from the backend API.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable list of <see cref="Building"/> entities.
    /// </returns>
    public async Task<IEnumerable<Building>> GetBuildingsAsync()
    {
        try
        {
            var response = await _apiClient.Buildings.GetAsync();

            var buildings = response?.Buildings?.Select(BuildingDtoMapper.toEntity)
                            ?? Enumerable.Empty<Building>();
            return buildings;
            
        } catch (ValidationErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        } catch (ConflictErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        }
       

    }

    public async Task<Building> CreateBuildingAsync(Building building)
    {

        CreateBuildingRequest req = new CreateBuildingRequest
        {
            Name = building.Name,
            OfficialID = building.OfficialId,
            FloorCount = building.FloorCount,
            Color = building.RenderInfo.Color,
            Height = (double)building.RenderInfo.Height,
            Width = (double)building.RenderInfo.Width,
            Depth = (double)building.RenderInfo.Depth,
            X = (double)building.RenderInfo.X,
            Y = (double)building.RenderInfo.Y,
            Z = (double)building.RenderInfo.Z,
            Texture = building.RenderInfo.Texture
        };

        try
        {
            var response = await _apiClient.Buildings.PostAsync(req);
            return BuildingDtoMapper.toEntity(response.Building!);
        } catch (ValidationErrorResponse ex )
        {
            throw new ValidationException(ex.ErrorMessage);
        } catch (ConflictErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        }
    }

    public async Task DeleteBuildingAsync(string officialId)
    {
        try
        {
            await _apiClient.Buildings[officialId].DeleteAsync();
        }
        catch (ValidationErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        }
        catch (ConflictErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        }
    }

    /// <summary>
    /// Asynchronously updates an existing building in the backend API.
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public async Task UpdateBuildingAsync(Building building)
    {
        var updateRequest = new UpdateBuildingRequest
        {
            OfficialID = building.OfficialId,
            Name = building.Name,
            FloorCount = building.FloorCount,
            Color = building.RenderInfo.Color,
            Height = (double)building.RenderInfo.Height,
            Width = (double)building.RenderInfo.Width,
            Depth = (double)building.RenderInfo.Depth,
            X = (double)building.RenderInfo.X,
            Y = (double)building.RenderInfo.Y,
            Z = (double)building.RenderInfo.Z,
            Texture = building.RenderInfo.Texture
        };
        
        try
        {
            await _apiClient.Buildings[building.OfficialId].PutAsync(updateRequest);
        } catch (ValidationErrorResponse ex )
        {
            throw new ValidationException(ex.ErrorMessage);
        } catch (ConflictErrorResponse ex)
        {
            throw new ValidationException(ex.ErrorMessage);
        }
    }
}

