namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Roles.Components;

/// <summary>
/// Represents a request to display a toast notification.
/// </summary>
/// <param name="Message">The text to display.</param>
/// <param name="Type">Bootstrap background type, e.g., success, warning, danger, info.</param>
public readonly record struct ToastRequest(string Message, string Type);
