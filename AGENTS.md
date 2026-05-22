# AGENTS.md — RestfullApi (SocialMedia)

## Solution layout

```
SocialMedia/
  SocialMedia.Api/             # ASP.NET Core 10 Web API (entry point)
  SocialMedia.Core/            # Entities, DTOs, interfaces, services, query filters
  SocialMedia.Infrastructure/  # EF Core, repos, migrations, validators, mappers, email/Hangfire
  TestProject/                 # Unit tests (xUnit + Moq, net10.0)
  SocialMedia.IntegrationTests/ # Integration tests (xUnit, net10.0)
  Dockerfile
  SocialMedia.sln
```

## Key commands

```powershell
# Build
dotnet build .\SocialMedia\SocialMedia.sln

# Run API
dotnet run --project .\SocialMedia\SocialMedia.Api

# Unit tests
dotnet test .\SocialMedia\TestProject\TestProject.csproj

# Single test (example)
dotnet test .\SocialMedia\TestProject\TestProject.csproj --filter "FullyQualifiedName~YourTestName"

# EF Core migrations (run from SocialMedia\)
dotnet ef migrations add MigrationName --project .\SocialMedia.Infrastructure --startup-project .\SocialMedia.Api
dotnet ef database update --project .\SocialMedia.Infrastructure --startup-project .\SocialMedia.Api

# Docker (run from SocialMedia\)
docker build -t socialmedia-api -f Dockerfile .
```

## Architecture & DI wiring

- **Clean Architecture**: Api → Core + Infrastructure; Infrastructure → Core only (no circular deps).
- DI registration lives in `SocialMedia.Infrastructure/Extentions/ServiceCollectionExtention.cs`.
- **Two DB options exist** but only `AddDbContextsPostgress` is called from `Startup.cs:72`. The `appsettings.json` `Database:Engine` key is ignored.
- `AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)` is required for PostgreSQL `DateTime` — set in `Program.cs:23`.
- Duplicate DI registrations exist (`IRepository<>` and `IUnitOfWork` are registered twice in `AddServices`). Do not add a third.
- `AddServices()` registers: `IGenericService<>` (scoped), services (transient), `IPasswordService` (singleton), `IUriService` (singleton via factory).

## API conventions

- All controllers inherit `ControllerBase`, use `[Route("api/[Controller]")]`, and require `[Authorize]`.
- Endpoints return `ApiResponse<T>` (wraps data + pagination metadata).
- Pagination metadata sent in both response body (`Meta`) and header (`X-Pagination`).
- IDs are `Guid` (from `BaseEntity`).
- Swagger UI at root path (`/`), JWT Bearer auth configured.
- CORS: `AllowAllOrigins` (open).

## Testing quirks

- `SocialMedia.IntegrationTests` targets **net10.0** (xUnit, references `SocialMedia.Api`).
- Real unit tests are in `TestProject/` (xUnit + Moq, references `SocialMedia.Api`).
- No integration test infrastructure (no test DB, no test server setup).

## CI/CD

- GitHub Actions on `main` branch builds + deploys to Azure Web App `PlaztiWebAppsNLogic`.
- Uses **MSBuild + nuget.exe** (not `dotnet publish`). This is an ASP.NET (non-core) style deploy pipeline despite the project being .NET 10.

## Notable gotchas

- FluentValidation validators auto-discovered via `AddValidatorsFromAssemblies` — place them in `SocialMedia.Infrastructure/Validators/`. Auto-validation registered via `services.AddFluentValidationAutoValidation()`.
- AutoMapper profiles auto-discovered — place in `SocialMedia.Infrastructure/Mappings/`. Registered via `services.AddAutoMapper(cfg => {}, assemblies)`.
- EF Core migrations are stored in `SocialMedia.Infrastructure/Migrations/`.
- Serilog writes to `logs/SocialMedia.txt` (rolling daily).
- Hangfire uses PostgreSQL storage (configured in `AddHangfire()`).
- The folder `SocialMedia.Infrastructure/Extentions/` is intentionally misspelled (keep the name).
