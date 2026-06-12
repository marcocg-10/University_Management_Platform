using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;
using System.ComponentModel;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp.InteractiveComponents;

/// <summary>
/// Boards MCP (Model Context Protocol) tools for managing board interactive components.
/// </summary>
[McpServerToolType]
public static class BoardMcpTools
{
    /// <summary>
    /// Gets all boards in the system.
    /// </summary>
    /// <param name="interactiveComponentsService"> interface for IC </param>
    /// <returns> The list of boards. </returns>
    [McpServerTool, Description("Gets all boards.")]
    public static Task<ListAllBoardsResponse> ListAllBoardsAsync([FromServices] IInteractiveComponentService interactiveComponentsService)
    {
        return ListAllBoardsHandler.HandleAsync(interactiveComponentsService);
    }
}
