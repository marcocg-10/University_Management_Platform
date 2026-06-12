# Error Handling & Toast Notifications Documentation

## Overview

This document explains how to use the toast notification system and the ErrorWrapper component in our Blazor application for effective error handling and user feedback.

## Table of Contents

1. [Toast Notifications](#toast-notifications)
2. [ErrorWrapper Component](#errorwrapper-component)
3. [Setup & Configuration](#setup--configuration)
4. [Usage Examples](#usage-examples)
5. [Best Practices](#best-practices)
6. [Troubleshooting](#troubleshooting)

---

## Toast Notifications

### What are Toast Notifications?

Toast notifications are temporary, non-intrusive messages that appear on screen to provide feedback to users about actions or events in the application.

### Available Toast Types

```csharp
// Error toasts (red)
ToastService.ShowError("Something went wrong!");

// Success toasts (green)
ToastService.ShowSuccess("Operation completed successfully!");

// Info toasts (blue)
ToastService.ShowInfo("Here's some useful information.");

// Warning toasts (yellow)
ToastService.ShowWarning("Please be aware of this warning.");
```

### Toast Configuration

Current toast settings (configured in `MainLayout.razor`):
- **Position**: Top-right corner
- **Timeout**: 10 seconds
- **Max Count**: 3 toasts at once
- **Progress Bar**: Enabled
- **Close Button**: Enabled
- **Icons**: Material Design icons

### Using Toasts in Components

1. **Inject the service** in your component:
```razor
@inject IToastService ToastService
```

2. **Show toasts** in your code:
```csharp
@code {
    private async Task SaveData()
    {
        try
        {
            await DataService.SaveAsync();
            ToastService.ShowSuccess("Data saved successfully!");
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to save: {ex.Message}");
        }
    }
}
```

---

## ErrorWrapper Component

### What is ErrorWrapper?

The `ErrorWrapper` is a Blazor error boundary component that catches unhandled exceptions in child components and provides a consistent error handling experience.

### How It Works

1. **Wraps child components** to catch exceptions
2. **Logs errors** for debugging
3. **Shows toast notifications** to inform users
4. **Redirects to error page** for serious issues

### Basic Usage

```razor
<ErrorWrapper>
    <YourComponent />
</ErrorWrapper>
```

### What Happens When an Error Occurs

1. Exception is thrown in wrapped component
2. `ErrorWrapper` catches the exception
3. Error is logged to console/logs
4. Toast notification appears with error message
5. User is redirected to `/error/500` page

### ErrorWrapper Features

- **Automatic Error Logging**: All errors are logged with full stack traces
- **Toast Notifications**: Users see friendly error messages
- **Graceful Degradation**: Prevents app crashes
- **Error Page Redirect**: Serious errors redirect to a user-friendly page

---

## Setup & Configuration

### Required Dependencies

The following packages are already configured:

```xml
<PackageReference Include="Blazored.Toast" Version="4.2.1" />
```

### Service Registration

In `Program.cs`:
```csharp
builder.Services.AddBlazoredToast();
```

### Component Setup

In `App.razor`:
```razor
<CascadingBlazoredToast />
```

In `MainLayout.razor`:
```razor
<BlazoredToasts Position="ToastPosition.TopRight"
                Timeout="10"
                MaxToastCount="3"
                ShowProgressBar="true"
                ShowCloseButton="true" />
```

---

## Usage Examples

### Example 1: Form with Error Handling

```razor
<!-- BuildingPage.razor -->
<ErrorWrapper>
    <NewBuildingForm OnValidSubmit="HandleSubmit" />
</ErrorWrapper>

@code {
    [Inject] private IToastService ToastService { get; set; }
    
    private async Task HandleSubmit()
    {
        try
        {
            await BuildingService.CreateAsync(building);
            ToastService.ShowSuccess("Building created successfully!");
        }
        catch (ValidationException ex)
        {
            ToastService.ShowWarning($"Validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            ToastService.ShowError("An unexpected error occurred.");
            // ErrorWrapper will catch if we rethrow
        }
    }
}
```

### Example 2: API Call with Toast Feedback

```razor
@inject IToastService ToastService
@inject IDataService DataService

<button @onclick="LoadData" disabled="@isLoading">
    @(isLoading ? "Loading..." : "Load Data")
</button>

@code {
    private bool isLoading = false;
    
    private async Task LoadData()
    {
        isLoading = true;
        try
        {
            var data = await DataService.GetDataAsync();
            ToastService.ShowSuccess($"Loaded {data.Count} items successfully!");
        }
        catch (UnauthorizedAccessException)
        {
            ToastService.ShowWarning("You don't have permission to access this data.");
        }
        catch (HttpRequestException ex)
        {
            ToastService.ShowError("Network error. Please check your connection.");
        }
        catch (Exception ex)
        {
            ToastService.ShowError("An unexpected error occurred.");
            // Log error for debugging
            Logger.LogError(ex, "Failed to load data");
        }
        finally
        {
            isLoading = false;
        }
    }
}
```

### Example 3: Multiple Components with Shared Error Handling

```razor
<!-- Page with multiple components -->
<ErrorWrapper>
    <div class="row">
        <div class="col-md-6">
            <UserList />
        </div>
        <div class="col-md-6">
            <UserDetails />
        </div>
    </div>
</ErrorWrapper>
```

---

## Best Practices

### When to Use Toasts

✅ **Good use cases:**
- Success confirmations ("Data saved!")
- Validation warnings ("Please fill required fields")
- Network errors ("Connection failed")
- Permission issues ("Access denied")

❌ **Avoid for:**
- Critical system errors (use ErrorWrapper redirect instead)
- Long error messages (use error page)
- Frequent notifications (users will ignore them)

### When to Use ErrorWrapper

✅ **Wrap these components:**
- Forms with complex validation
- Data grids and lists
- Components that make API calls
- Interactive widgets

❌ **Don't wrap:**
- Simple static content
- Individual buttons (wrap the parent instead)
- Components that already have error handling

### Error Message Guidelines

1. **Be User-Friendly**: Avoid technical jargon
   ```csharp
   // ❌ Bad
   ToastService.ShowError("NullReferenceException in UserService.GetById()");
   
   // ✅ Good
   ToastService.ShowError("Unable to load user information. Please try again.");
   ```

2. **Be Actionable**: Tell users what they can do
   ```csharp
   // ❌ Bad
   ToastService.ShowError("Validation failed");
   
   // ✅ Good
   ToastService.ShowError("Please fill in all required fields and try again.");
   ```

3. **Be Specific When Helpful**:
   ```csharp
   // ✅ Good
   ToastService.ShowWarning($"File size ({fileSize}MB) exceeds the 10MB limit.");
   ```

---

## Troubleshooting

### Toasts Not Appearing

1. **Check service registration**:
   ```csharp
   // In Program.cs
   builder.Services.AddBlazoredToast();
   ```

2. **Check component placement**:
   ```razor
   <!-- In App.razor -->
   <CascadingBlazoredToast />
   
   <!-- In MainLayout.razor -->
   <BlazoredToasts />
   ```

3. **Check injection**:
   ```razor
   @inject IToastService ToastService
   ```

### Toasts Appearing Behind Modals

Add high z-index CSS:
```css
.toast-high-z {
    z-index: 10000 !important;
}
```

### ErrorWrapper Not Catching Errors

1. **Ensure proper wrapping**:
   ```razor
   <!-- ✅ Correct -->
   <ErrorWrapper>
       <ComponentThatMightFail />
   </ErrorWrapper>
   
   <!-- ❌ Wrong -->
   <ComponentThatMightFail />
   <ErrorWrapper />
   ```

2. **Check if errors are being caught elsewhere**:
   ```csharp
   try 
   {
       // Don't catch and hide errors that should bubble up
   }
   catch (Exception)
   {
       // This prevents ErrorWrapper from seeing the error
   }
   ```

### Redirect Not Working

Check NavigationManager injection:
```csharp
@inject NavigationManager NavigationManager
```

---

## Error Page (500Error.razor)

The error page provides a user-friendly experience when serious errors occur:

- **Route**: `/error/500` or `/error`
- **Features**: 
  - Friendly error message
  - "Go to Home" button
  - "Go Back" button
  - Clean, professional design

### Customizing the Error Page

You can modify `500Error.razor` to:
- Add error reporting functionality
- Include contact information
- Add custom branding
- Provide additional help resources

---

## Summary

- **Use toasts** for user feedback and minor errors
- **Use ErrorWrapper** to catch and handle component errors gracefully  
- **Follow best practices** for user-friendly error messages
- **Configure properly** to ensure everything works together
- **Test error scenarios** to ensure good user experience

For questions or issues, contact the development team or check the troubleshooting section above.