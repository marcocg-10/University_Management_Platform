using ModelContextProtocol.Server;
using System.ComponentModel;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp.Buildings;

/// <summary>
/// MCP tools for building management.
/// </summary>
[McpServerToolType]

public static class BuildingsMcpTools
{
    /// <summary>
    /// Retrieves all buildings registered in the system.
    /// This method does not support querying buildings by name.
    /// </summary>
    /// <param name="buildingService">The service used to access building data.</param>
    /// <returns> 
    /// The <see cref="GetBuildingsResponse"/> containing a collection of all registered buildings.
    /// </returns>
    [McpServerTool, Description("Get all buildings registered or filter by name to retrieve specific building information")]
    public static Task<GetBuildingsResponse> GetBuildingsAsync(IBuildingService buildingService)
    {
        return GetBuildingsHandler.HandleAsync(buildingService);
    }
}
