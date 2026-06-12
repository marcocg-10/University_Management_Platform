using Microsoft.AspNetCore.Components;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Core.Components.Tables;

public sealed class ColumnDefinition<TItem>
{
    public required string Title { get; init; }
    public Func<TItem, object?>? ValueSelector { get; init; }
    public RenderFragment<TItem>? Template { get; init; }
}