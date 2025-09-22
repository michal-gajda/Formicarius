# Formicarius

```powershell
git init
dotnet new gitignore
dotnet new sln --format slnx --name Formicarius
dotnet new webapi --framework net9.0 --no-https --use-program-main --use-controllers --output src/WebApi --name Formicarius.WebApi
dotnet sln add src/WebApi
```

## OpenTelemetry

```powershell
dotnet add src/WebApi package OpenTelemetry.Extensions.Hosting
dotnet add src/WebApi package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add src/WebApi package OpenTelemetry.Instrumentation.AspNetCore
```
