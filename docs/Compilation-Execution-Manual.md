# ThemePark Back-end/Front-end Compilation & Execution Manual

## Build & Run

### Back-end

To build and run the back-end with swagger disabled, open a terminal located at Backend.Api and run the following commands:

```shell
dotnet clean
dotnet build
dotnet run
```

To do the same with swagger enabled, from a terminal located at Backend.Api, run:

```shell
dotnet clean
dotnet build -p:SWAGGER=true
dotnet run -p:SWAGGER=true
```

To run with https:

```shell
dotnet run --launch-profile https -p:SWAGGER=true
```

### Front-end

To build and run the front-end, open a terminal located at Frontend.Blazor and run the following commands:

```shell
dotnet clean
dotnet build
dotnet run
```

## Notes

### For Windows users:

Currently, the back-end and front-end projects execute successfully when they are executed one at a time. However, simultaneous execution of both projects, only works when using Visual Studio's multiple startup projects function. Simultaneous execution via the terminal is currently not working on Windows.
