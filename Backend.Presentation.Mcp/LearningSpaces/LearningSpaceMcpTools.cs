using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;
using System.ComponentModel;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp.LearningSpaces;

/// <summary>
/// Tools used by a model context protocol (MCP) agent for learning space management.
/// </summary>
[McpServerToolType]
public static class LearningSpaceMcpTools
{
    /// <summary>
    /// Tool used by a MCP agent to list all laboratories in the university. 
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a LearningSpaceService interface.</param>
    /// <returns>List laboratories response as an asynchronous operation.</returns>
    [McpServerTool, Description("Lists all laboratories in the university.")]
    public static Task<ListLaboratoriesResponse> ListLaboratoriesAsync([FromServices] ILearningSpaceService learningSpaceService)
    {
        return ListLaboratoriesHandler.HandleAsync(learningSpaceService);
    }
}