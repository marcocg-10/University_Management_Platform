function New-FrontendClassLibraryProject {
    param (
        [string]$ProjectName
    )

    if (-not(Test-Path "Frontend.$ProjectName"))
    {
        dotnet new classlib -o "Frontend.$ProjectName" -n "UCR.ECCI.PI.ThemePark.Frontend.$ProjectName"
        dotnet sln add "Frontend.$ProjectName" --solution-folder Frontend
        dotnet add "Frontend.$ProjectName" package Microsoft.Extensions.DependencyInjection.Abstractions
        dotnet add "Frontend.$ProjectName" package Microsoft.Extensions.Configuration.Abstractions
        dotnet add "Frontend.$ProjectName" package SonarAnalyzer.CSharp
        dotnet add "Frontend.$ProjectName" package SonarAnalyzer.CSharp.Styling
    }
}

function New-FrontendUnitTestProject {
    param (
        [string]$ProjectName
    )

    if (-not(Test-Path "Frontend.$ProjectName.Tests.Unit"))
    {
        # Xunit test project.
        dotnet new xunit -o "Frontend.$ProjectName.Tests.Unit" -n "UCR.ECCI.PI.ThemePark.Frontend.$ProjectName.Tests.Unit"
        dotnet sln add "Frontend.$ProjectName.Tests.Unit" --solution-folder Frontend
        dotnet add "Frontend.$ProjectName.Tests.Unit" package Moq
        dotnet add "Frontend.$ProjectName.Tests.Unit" package FluentAssertions
        dotnet add "Frontend.$ProjectName.Tests.Unit" package SonarAnalyzer.CSharp
        dotnet add "Frontend.$ProjectName.Tests.Unit" package SonarAnalyzer.CSharp.Styling
    }
}

$projects = @("Application", "Domain", "Infrastructure")
$projects | ForEach-Object {
    New-FrontendClassLibraryProject -ProjectName $_
    New-FrontendUnitTestProject -ProjectName $_
}

New-FrontendClassLibraryProject -ProjectName "DependencyInjection"

dotnet new razorclasslib -o Frontend.Presentation.Blazor -n UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor
dotnet sln add "Frontend.Presentation.Blazor" --solution-folder Frontend

dotnet new install bunit.template
dotnet new bunit --framework xunit -o Frontend.Presentation.Blazor.Tests.Unit -n UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Tests.Unit
dotnet sln add "Frontend.Presentation.Blazor.Tests.Unit" --solution-folder Frontend

dotnet new blazor -o Frontend.Blazor -n UCR.ECCI.PI.ThemePark.Frontend.Blazor
dotnet sln add "Frontend.Blazor" --solution-folder Frontend