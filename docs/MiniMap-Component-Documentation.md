# MiniMap Component Documentation

## Overview

The MiniMap component is a 2D visualization tool built with Blazor and JavaScript that provides an interactive canvas for displaying and manipulating objects. It features dynamic scaling, collision detection, drag-and-drop functionality, and a clean external UI.

## Features

- ✅ **Interactive Objects**: Drag and drop objects with customizable properties
- ✅ **Dynamic Grid System**: Automatically adjusting coordinate grid with multiple scale levels
- ✅ **Collision Detection**: Visual feedback with red borders for overlapping objects
- ✅ **External UI Controls**: Clean interface with scale indicator and grid toggle
- ✅ **Zoom & Pan**: Mouse wheel zoom and canvas panning
- ✅ **Customizable Objects**: Color, size, and draggable properties
- ✅ **Real-time Callbacks**: Position change notifications
- ✅ **Hover Tooltips**: Object information on mouse hover

## Component Structure

### File Locations
```
Frontend.Presentation.Blazor/Core/2D/MiniMap.razor         # Main Blazor component
Frontend.Blazor/wwwroot/_content/Minimap/minimap.js       # JavaScript engine
Frontend.Presentation.Blazor/Core/Pages/MinimapTest.razor # Example usage
```

## Basic Usage

### 1. Component Declaration

```csharp
@page "/minimap-example"

<MiniMap @ref="miniMap"
         Width="400"
         Height="300"
         OnObjectMoved="HandleObjectMove" />

@code {
    private MiniMap miniMap = null!;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await miniMap.AddObject("building1", 100, 100, "blue", 30, 20, true);
            await miniMap.AddObject("building2", 200, 150, "red", 25, 25, false);
        }
    }
    
    private Task HandleObjectMove(MiniMap.ObjectPosition position)
    {
        Console.WriteLine($"Object {position.Id} moved to ({position.X}, {position.Y})");
        return Task.CompletedTask;
    }
}
```

### 2. Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Width` | `int` | 300 | Canvas width in pixels |
| `Height` | `int` | 300 | Canvas height in pixels |
| `OnObjectMoved` | `EventCallback<ObjectPosition>` | - | Callback when object is dragged by user |

## API Methods

### Object Management

#### AddObject
Adds a new object to the minimap.

```csharp
public async Task AddObject(
    string id,              // Unique object identifier
    double x,              // X coordinate
    double y,              // Y coordinate  
    string color = "red",  // CSS color
    double width = 10,     // Object width
    double length = 10,    // Object height
    bool draggable = true  // Can be dragged by user
)
```

**Example:**
```csharp
await miniMap.AddObject("building1", 150, 100, "#4CAF50", 40, 30, true);
```

#### UpdateObjectPosition
Programmatically moves an object to a new position.

```csharp
public async Task<bool> UpdateObjectPosition(
    string id,    // Object ID
    double x,     // New X coordinate
    double y      // New Y coordinate
)
```

**Example:**
```csharp
bool success = await miniMap.UpdateObjectPosition("building1", 200, 150);
if (success) {
    Console.WriteLine("Object moved successfully");
}
```

#### MoveObject (Legacy)
Moves an object without return value.

```csharp
public async Task MoveObject(string id, double x, double y)
```

#### SetObjectDraggable
Changes whether an object can be dragged by the user.

```csharp
public async Task SetObjectDraggable(string id, bool draggable)
```

**Example:**
```csharp
// Make object non-draggable
await miniMap.SetObjectDraggable("building1", false);
```

### View Controls

#### SetZoom
Sets the zoom level of the minimap.

```csharp
public async Task SetZoom(double zoom) // Range: 0.3 to 4.0
```

#### SetOffset
Sets the pan offset of the view.

```csharp
public async Task SetOffset(double x, double y)
```

### Grid Configuration

#### SetGridVisibility
Shows or hides the coordinate grid.

```csharp
public async Task SetGridVisibility(bool visible)
```

#### SetGridSize
Sets the base grid spacing.

```csharp
public async Task SetGridSize(double size)
```

#### SetGridColors
Customizes grid appearance.

```csharp
public async Task SetGridColors(
    string gridColor = "#e0e0e0",  // Grid line color
    string axisColor = "#888888"   // Main axis color
)
```

### Collision Detection

#### GetCollisions
Returns array of object IDs that are currently colliding.

```csharp
public async Task<string[]> GetCollisions()
```

**Example:**
```csharp
string[] colliding = await miniMap.GetCollisions();
foreach (string id in colliding) {
    Console.WriteLine($"Object {id} is colliding");
}
```

## Advanced Features

### Dynamic Scaling System

The grid automatically adjusts based on zoom level:

| Zoom Level | Scale Ratio | Use Case |
|------------|-------------|----------|
| ≥ 4.0 | 1:1 | Precise positioning |
| ≥ 2.0 | 1:5 | Detail work |
| ≥ 1.0 | 1:10 | Normal view |
| ≥ 0.5 | 1:20 | Wider view |
| ≥ 0.3 | 1:50 | Layout overview |
| ≥ 0.1 | 1:100 | Large area view |
| < 0.1 | 1:200+ | Maximum overview |

### Object States and Visual Feedback

#### Cursor States
- **Draggable objects**: `pointer` cursor on hover
- **Non-draggable objects**: `default` cursor
- **Background**: `grab` cursor for panning
- **During drag**: `move` cursor

#### Visual Indicators
- **Collision Detection**: Red borders on overlapping objects
- **Hover Tooltips**: Show object ID and coordinates
- **Scale Indicator**: Current zoom ratio display
- **Grid Toggle**: Visual button state

## Integration Examples

### Example 1: Building Layout Tool

```csharp
@code {
    private readonly List<Building> _buildings = new();
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadBuildings();
        }
    }
    
    private async Task LoadBuildings()
    {
        var buildings = await BuildingService.GetBuildingsAsync();
        
        foreach (var building in buildings)
        {
            await miniMap.AddObject(
                building.Id,
                building.Position.X,
                building.Position.Y,
                building.Color,
                building.Width,
                building.Depth,
                building.IsMoveable
            );
        }
    }
    
    private async Task HandleObjectMove(MiniMap.ObjectPosition pos)
    {
        // Update building position in database
        var building = _buildings.FirstOrDefault(b => b.Id == pos.Id);
        if (building != null)
        {
            building.Position = new Point(pos.X, pos.Y);
            await BuildingService.UpdateBuildingAsync(building);
            
            // Check for collisions
            var collisions = await miniMap.GetCollisions();
            if (collisions.Contains(pos.Id))
            {
                await ShowCollisionWarning(pos.Id);
            }
        }
    }
}
```

### Example 2: Animation System

```csharp
private async Task AnimateObjectPath(string objectId, List<Point> path)
{
    foreach (var point in path)
    {
        await miniMap.UpdateObjectPosition(objectId, point.X, point.Y);
        await Task.Delay(100); // Animation speed
    }
}

private async Task AnimateCircularPath(string objectId, Point center, double radius)
{
    for (int angle = 0; angle <= 360; angle += 5)
    {
        double radians = angle * Math.PI / 180;
        double x = center.X + radius * Math.Cos(radians);
        double y = center.Y + radius * Math.Sin(radians);
        
        await miniMap.UpdateObjectPosition(objectId, x, y);
        await Task.Delay(50);
    }
}
```

### Example 3: Collision Monitoring

```csharp
private Timer _collisionTimer;

protected override void OnInitialized()
{
    // Check for collisions every second
    _collisionTimer = new Timer(CheckCollisions, null, 1000, 1000);
}

private async void CheckCollisions(object state)
{
    var collisions = await miniMap.GetCollisions();
    
    if (collisions.Length > 0)
    {
        await InvokeAsync(() =>
        {
            foreach (string id in collisions)
            {
                ShowCollisionAlert(id);
            }
            StateHasChanged();
        });
    }
}
```

## Event Handling

### ObjectPosition Record

```csharp
public record ObjectPosition(
    string Id,                    // Object identifier
    double X,                     // X coordinate
    double Y,                     // Y coordinate
    string Color = "red",         // Object color
    double Width = 10,            // Object width
    double Length = 10            // Object height
);
```

### OnObjectMoved Callback

Called when user drags an object (not when moved programmatically):

```csharp
private async Task HandleObjectMove(MiniMap.ObjectPosition position)
{
    // Update your data model
    await UpdateObjectInDatabase(position);
    
    // Validate new position
    if (IsValidPosition(position))
    {
        await SavePosition(position);
    }
    else
    {
        // Revert to previous position
        await miniMap.UpdateObjectPosition(position.Id, previousX, previousY);
    }
}
```

## Best Practices

### 1. Initialization
Always use `OnAfterRenderAsync` with `firstRender` check:

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        // Add objects here - component is fully initialized
        await AddObjects();
    }
}
```

### 2. Error Handling
Wrap API calls in try-catch blocks:

```csharp
try
{
    bool success = await miniMap.UpdateObjectPosition("obj1", x, y);
    if (!success)
    {
        Logger.LogWarning("Failed to move object obj1 - object not found");
    }
}
catch (Exception ex)
{
    Logger.LogError(ex, "Error moving object");
}
```

### 3. Performance
- Avoid rapid successive calls to UpdateObjectPosition
- Use appropriate animation delays (50-100ms)
- Check collision state only when needed

### 4. User Experience
- Provide visual feedback for operations
- Handle edge cases gracefully
- Use meaningful object IDs
- Validate coordinates before moving objects

## Troubleshooting

### Common Issues

#### Objects not appearing
- Ensure `OnAfterRenderAsync` with `firstRender` check is used
- Verify coordinates are within canvas bounds
- Check console for JavaScript errors

#### NullReferenceException
- Use `= null!` for component references
- Don't call methods in `OnInitializedAsync`
- Ensure component is properly referenced

#### Collisions not detecting
- Verify objects have non-zero dimensions
- Check that objects actually overlap
- Ensure collision checking is called after object movement

#### Animation not smooth
- Use appropriate delay (50-100ms)
- Avoid blocking the UI thread
- Consider using `Task.Yield()` for better responsiveness

### Debug Commands

```csharp
// Log current object positions
var collisions = await miniMap.GetCollisions();
Console.WriteLine($"Current collisions: {string.Join(", ", collisions)}");

// Test object movement
bool moved = await miniMap.UpdateObjectPosition("test", 100, 100);
Console.WriteLine($"Object moved: {moved}");
```

## Browser Compatibility

- **Chrome**: ✅ Full support
- **Firefox**: ✅ Full support  
- **Safari**: ✅ Full support
- **Edge**: ✅ Full support
- **Mobile**: ⚠️ Limited touch support

## Performance Notes

- Optimized for up to 100 objects
- Collision detection is O(n²) - consider spatial partitioning for many objects
- Canvas rendering is hardware accelerated
- JavaScript module lazy-loads on first use

---

## Version History

- **v1.0**: Initial release with basic functionality
- **v1.1**: Added collision detection and visual feedback
- **v1.2**: External UI controls and dynamic scaling
- **v1.3**: Improved initialization and error handling
- **v1.4**: Added animation support and comprehensive API

---

For additional support or feature requests, please refer to the project's GitHub repository.