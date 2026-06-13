# AGENTS.md â€” RestfullApi (Template)

## Solution layout

```
src/
  Template.Api/             # ASP.NET Core 10 Web API (entry point)
  Template.Core/            # Entities, DTOs, interfaces, services, query filters
  Template.Infrastructure/  # EF Core, repos, migrations, validators, mappers, email/Hangfire
  Template.IntegrationTests/ # Integration tests (xUnit, net10.0)
  Dockerfile
  Template.sln
```

## Key commands

```powershell
# Build
dotnet build .\src\Template.sln

# Run API
dotnet run --project .\src\Template.Api

# Unit tests
dotnet test .\src\Template.IntegrationTests\Template.IntegrationTests.csproj

# Single test (example)
dotnet test .\src\Template.IntegrationTests\Template.IntegrationTests.csproj --filter "FullyQualifiedName~YourTestName"

# EF Core migrations (run from repo root)
dotnet ef migrations add MigrationName --project .\src\Template.Infrastructure --startup-project .\src\Template.Api
dotnet ef database update --project .\src\Template.Infrastructure --startup-project .\src\Template.Api

# Docker (run from src/)
docker build -t template-api -f Dockerfile .
```

## Architecture & DI wiring

- **Clean Architecture**: Api â†’ Core + Infrastructure; Infrastructure â†’ Core only (no circular deps).
- DI registration lives in `Template.Infrastructure/Extentions/ServiceCollectionExtention.cs`.
- **Two DB options exist** but only `AddDbContextsPostgress` is called from `Startup.cs:72`. The `appsettings.json` `Database:Engine` key is ignored.
- `AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)` is required for PostgreSQL `DateTime` â€” set in `Program.cs:23`.
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

- `Template.IntegrationTests` targets **net10.0** (xUnit, references `Template.Api`).
- Real unit tests are in `TestProject/` (xUnit + Moq, references `Template.Api`).
- No integration test infrastructure (no test DB, no test server setup).

## CI/CD

- GitHub Actions on `main` branch builds + deploys to Azure Web App `PlaztiWebAppsNLogic`.
- Uses **MSBuild + nuget.exe** (not `dotnet publish`). This is an ASP.NET (non-core) style deploy pipeline despite the project being .NET 10.

## Notable gotchas

- FluentValidation validators auto-discovered via `AddValidatorsFromAssemblies` â€” place them in `Template.Infrastructure/Validators/`. Auto-validation registered via `services.AddFluentValidationAutoValidation()`.
- AutoMapper profiles auto-discovered â€” place in `Template.Infrastructure/Mappings/`. Registered via `services.AddAutoMapper(cfg => {}, assemblies)`.
- EF Core migrations are stored in `Template.Infrastructure/Migrations/`.
- Serilog writes to `logs/Template.txt` (rolling daily).
- Hangfire uses PostgreSQL storage (configured in `AddHangfire()`).
- The folder `Template.Infrastructure/Extentions/` is intentionally misspelled (keep the name).


## MCP Usage

### context7
Use Context7 MCP to fetch current documentation whenever the user asks about a library, framework, SDK, API, CLI tool, or cloud service -- even well-known ones like React, Next.js, Prisma, Express, Tailwind, Django, or Spring Boot. This includes API syntax, configuration, version migration, library-specific debugging, setup instructions, and CLI tool usage. Use even when you think you know the answer -- your training data may not reflect recent changes. Prefer this over web search for library docs.

Do not use for: refactoring, writing scripts from scratch, debugging business logic, code review, or general programming concepts.

### microsoft_learn
Use Microsoft Learn MCP to fetch official .NET, ASP.NET Core, EF Core, C#, Azure, and other Microsoft documentation. Use this when questions involve .NET APIs, SDKs, Azure services, or Microsoft-specific frameworks instead of general web search.

### angular-cli
Use Angular CLI MCP for Angular-specific tasks: generating components, services, guards, pipes, directives, and other artifacts. Also use it to query Angular CLI commands, schematics, and build configuration. Do NOT use for general Angular runtime concepts (use context7 for that).

### sequential-thinking
Use Sequential Thinking MCP for complex problem-solving, debugging, refactoring, or any task that requires breaking down into logical steps. Invoke it when reasoning through architecture decisions, troubleshooting bugs, or planning multi-step implementations.

---

## Code Quality Standards

- **Defensive coding:** Always validate inputs, handle null/undefined gracefully, use type guards, and never trust external data. Prefer early returns and fail-fast patterns over deep nesting.
- **Clean Code:** Write self-documenting code â€” meaningful names, small focused functions/methods, single responsibility, no magic numbers/strings, avoid duplication (DRY), and keep side-effects explicit.
- **Error handling:** Use Result patterns (FluentResults in C#, custom union types in TS) instead of throwing exceptions for expected business failures. Log unexpected exceptions with structured context.
- **No comments in generated code** unless the logic is unavoidably complex â€” prefer expressive code over explanatory comments.

