# CustomSelect Component Usage Guide

## Overview

The `CustomSelect` component is a reusable, generic dropdown selector with search functionality. It provides a consistent UI/UX across the application for selecting items from a list.

---

## Core Component: CustomSelect

### Location
`Frontend.Presentation.Blazor\Core\Components\CustomSelect.razor`

### Features
- Generic type support - works with any data type
- Searchable dropdown with real-time filtering
- Keyboard-friendly and accessible
- Customizable display text and item keys
- Optional search (can be disabled)
- Two-way binding support
- Disabled state support
- No results message customization

---

## Basic Usage

### Minimal Example

```razor
@using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Core.Components

<CustomSelect TItem="MyEntity"
              Items="myEntities"
              SelectedItem="selectedEntity"
              SelectedItemChanged="OnEntitySelected"
              GetItemText="@(entity => entity.Name)"
              GetItemKey="@(entity => entity.Id)" />

@code {
    private List<MyEntity> myEntities = new();
    private MyEntity? selectedEntity;

    private void OnEntitySelected(MyEntity? entity)
    {
        selectedEntity = entity;
        // Handle selection logic here
    }
}
```

---

## Parameters Reference

### Required Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `Items` | `IEnumerable<TItem>` | Collection of items to display in the dropdown |
| `GetItemText` | `Func<TItem, string>` | Function that returns the display text for each item |

### Optional Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `SelectedItem` | `TItem?` | `null` | Currently selected item (supports two-way binding) |
| `SelectedItemChanged` | `EventCallback<TItem?>` | - | Event fired when selection changes |
| `GetItemKey` | `Func<TItem, object>` | `item => item!` | Function to get unique key for item comparison |
| `Placeholder` | `string` | `"Select an option"` | Text shown when no item is selected |
| `SearchPlaceholder` | `string` | `"Search"` | Placeholder text in the search input |
| `NoResultsText` | `string` | `"No results found"` | Text shown when search returns no results |
| `EnableSearch` | `bool` | `true` | Whether to show the search input |
| `Disabled` | `bool` | `false` | Disables the dropdown |
| `CssClass` | `string` | `""` | Additional CSS classes |
| `AdditionalAttributes` | `Dictionary<string, object>?` | - | Any additional HTML attributes |

---

## Advanced Examples

### Example 1: Simple String List

```razor
<CustomSelect TItem="string"
              Items="countries"
              SelectedItem="selectedCountry"
              SelectedItemChanged="@(country => selectedCountry = country)"
              GetItemText="@(country => country)"
              GetItemKey="@(country => country)"
              Placeholder="Select Country" />

@code {
    private List<string> countries = new() { "Australia", "Colombia", "Denmark", "Germany", "Indonesia" };
    private string? selectedCountry;
}
```

### Example 2: Complex Object with Custom Display

```razor
<CustomSelect TItem="User"
              Items="users"
              SelectedItem="selectedUser"
              SelectedItemChanged="HandleUserSelection"
              GetItemText="@(user => $"{user.FirstName} {user.LastName} ({user.Email})")"
              GetItemKey="@(user => user.Id)"
              Placeholder="Select User"
              SearchPlaceholder="Search by name or email" />

@code {
    private List<User> users = new();
    private User? selectedUser;

    private async Task HandleUserSelection(User? user)
    {
        selectedUser = user;
        if (user != null)
        {
            await LoadUserDetails(user.Id);
        }
    }
}
```

### Example 3: Dropdown Without Search

```razor
<CustomSelect TItem="Status"
              Items="statuses"
              SelectedItem="currentStatus"
              SelectedItemChanged="@(status => currentStatus = status)"
              GetItemText="@(status => status.ToString())"
              EnableSearch="false"
              Placeholder="Select Status" />

@code {
    private List<Status> statuses = Enum.GetValues<Status>().ToList();
    private Status currentStatus;
}
```

### Example 4: Disabled State

```razor
<CustomSelect TItem="Building"
              Items="buildings"
              SelectedItem="selectedBuilding"
              Disabled="isProcessing"
              Placeholder="Select Building" />

@code {
    private bool isProcessing = false;
}
```

---

## BuildingSelect Implementation

### Location
`Frontend.Presentation.Blazor\Buildings\Components\BuildingSelect.razor`

### Purpose
A specialized component that uses `CustomSelect` specifically for building selection, with built-in service integration.

### Usage

```razor
@using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Buildings.Components

<BuildingSelect SelectedBuilding="selectedBuilding"
                SelectedBuildingChanged="OnBuildingSelected" />

@code {
    private Building? selectedBuilding;

    private void OnBuildingSelected(Building? building)
    {
        selectedBuilding = building;
        Console.WriteLine($"Selected: {building?.Name}");
    }
}
```

### What BuildingSelect Handles

- Automatically fetches buildings from `IBuildingService`
- Handles loading states and errors
- Pre-configured with building-specific settings
- Uses building name for display and ID for comparison

### Full BuildingSelect Example in a Page

```razor
@page "/buildings/assign"
@using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities
@using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Buildings.Components

<h3>Assign Building</h3>

<div class="form-group">
    <label>Choose a Building:</label>
    <BuildingSelect SelectedBuilding="selectedBuilding"
                    SelectedBuildingChanged="HandleBuildingChange" />
</div>

@if (selectedBuilding != null)
{
    <div class="alert alert-info mt-3">
        <h5>Selected Building</h5>
        <p><strong>Name:</strong> @selectedBuilding.Name</p>
        <p><strong>ID:</strong> @selectedBuilding.Id</p>
    </div>
}

<button class="btn btn-primary mt-3" 
        disabled="@(selectedBuilding == null)"
        @onclick="SaveAssignment">
    Save Assignment
</button>

@code {
    private Building? selectedBuilding;

    private void HandleBuildingChange(Building? building)
    {
        selectedBuilding = building;
        StateHasChanged();
    }

    private async Task SaveAssignment()
    {
        if (selectedBuilding != null)
        {
            // Save logic here
            Console.WriteLine($"Saving building: {selectedBuilding.Name}");
        }
    }
}
```

---

## Styling

The component comes with built-in CSS (`CustomSelect.razor.css`) that provides:
- Clean, modern design
- Hover and focus states
- Selected item highlighting
- Smooth transitions
- Responsive behavior
- Custom scrollbar styling

To override styles, you can:

1. **Add custom CSS class:**
```razor
<CustomSelect TItem="Building"
              CssClass="my-custom-select"
              ... />
```

2. **Use global CSS override:**
```css
::deep .custom-select-container {
    max-width: 500px;
}

::deep .select-header {
    background-color: #f0f0f0;
}
```

---

## Creating Your Own Select Components

Follow the `BuildingSelect` pattern to create specialized select components:

### Step 1: Create the Component

```razor
@* UserSelect.razor *@
@using YourNamespace.Domain.Users.Entities
@using YourNamespace.Application.Users.Services
@using YourNamespace.Blazor.Core.Components
@inject IUserService UserService

<CustomSelect TItem="User"
              Items="_users"
              SelectedItem="SelectedUser"
              SelectedItemChanged="SelectedUserChanged"
              GetItemText="@(user => $"{user.FirstName} {user.LastName}")"
              GetItemKey="@(user => user.Id)"
              Placeholder="Select User"
              SearchPlaceholder="Search users"
              NoResultsText="No users found" />

@code {
    [Parameter]
    public User? SelectedUser { get; set; }

    [Parameter]
    public EventCallback<User?> SelectedUserChanged { get; set; }

    private IEnumerable<User> _users = Array.Empty<User>();

    protected override async Task OnInitializedAsync()
    {
        _users = await UserService.GetUsersAsync();
    }
}
```

### Step 2: Use the Component

```razor
<UserSelect SelectedUser="currentUser"
            SelectedUserChanged="@(user => currentUser = user)" />
```

---

## Common Pitfalls

### 1. Missing GetItemText
```razor
Bad:
<CustomSelect TItem="Building" Items="buildings" />

Good:
<CustomSelect TItem="Building" 
              Items="buildings"
              GetItemText="@(b => b.Name)" />
```

### 2. Item Comparison Issues
If your items aren't comparing correctly, provide a proper `GetItemKey`:

```razor
<CustomSelect TItem="MyEntity"
              Items="entities"
              GetItemText="@(e => e.Name)"
              GetItemKey="@(e => e.Id)" />  <!-- Use unique identifier -->
```

### 3. Two-Way Binding
For two-way binding to work, both parameters must be provided:

```razor
<CustomSelect @bind-SelectedItem="myItem"
              Items="items"
              GetItemText="@(i => i.Name)" />
```

Or manually:
```razor
<CustomSelect SelectedItem="myItem"
              SelectedItemChanged="@(item => myItem = item)"
              Items="items"
              GetItemText="@(i => i.Name)" />
```

---

## Summary

| Use Case | Component to Use |
|----------|------------------|
| Need a building selector | Use `BuildingSelect` |
| Need any other entity selector | Create a wrapper like `BuildingSelect` using `CustomSelect` |
| One-off custom dropdown | Use `CustomSelect` directly in your page |

Both components provide a consistent, user-friendly selection experience while maintaining flexibility for different use cases.
