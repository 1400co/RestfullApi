# Restful API

API RESTful construida con **ASP.NET Core 10** y **EF Core**, siguiendo **Clean Architecture** con patrón **Repository + Unit of Work**.

## Stack

- **Framework**: ASP.NET Core 10, C#
- **ORM**: Entity Framework Core 10 con PostgreSQL (principal) y SQL Server (alternativo)
- **Autenticación**: JWT Bearer + Refresh Tokens
- **Mapeo**: AutoMapper
- **Validación**: FluentValidation
- **Logs**: Serilog (archivo + consola)
- **Background Jobs**: Hangfire con PostgreSQL
- **Documentación**: Swagger / Swashbuckle
- **Contenedor**: Docker
- **CI/CD**: GitHub Actions → Azure Web App

## Arquitectura

```
SocialMedia.sln
├── SocialMedia.Api              # ASP.NET Core Web API (entry point)
│   ├── Controllers/             #   Post, User, Token, Roles, Modules, etc.
│   ├── Startup.cs               #   DI, middleware, auth config
│   └── Program.cs               #   Host builder, Serilog, Npgsql timestamp
│
├── SocialMedia.Core             # Capa de dominio y aplicación
│   ├── Entities/                #   Modelos de dominio (Post, User, Comment, etc.)
│   ├── Dtos/                    #   Objetos de transferencia
│   ├── Interfaces/              #   Contratos de servicios y repositorios
│   ├── Services/                #   Lógica de negocio
│   ├── QueryFilters/            #   Filtros con paginación
│   ├── CustomEntities/          #   PagedList, Metadata, opciones de config
│   └── Enumerations/            #   Enums (RoleType, Subscription, etc.)
│
├── SocialMedia.Infrastructure   # Capa de infraestructura
│   ├── Data/                    #   DbContext + Fluent API configurations
│   ├── Repositories/            #   BaseRepository, PostRepository, UnitOfWork
│   ├── Services/                #   EmailService, PasswordService, UriService
│   ├── Filters/                 #   GlobalExceptionFilter, ValidationFilter
│   ├── Validators/              #   FluentValidation validators
│   ├── Mappings/                #   AutoMapper profiles
│   ├── Migrations/              #   EF Core migrations
│   └── Extentions/              #   DI registration extension methods
│
├── TestProject                  # Tests unitarios (xUnit + Moq)
└── SocialMedia.IntegrationTests # Tests de integración (xUnit)
```

## Características

### Autenticación y Seguridad
- **Login OTP**: El usuario solicita un código de un solo uso por email y lo canjea por un JWT.
- **OTP criptográfico**: Generado con `RandomNumberGenerator.GetInt32()`, no con `System.Random`.
- **OTP de un solo uso**: Se elimina de la BD tras validación exitosa, previniendo ataques de reuso.
- **Rate limiting**: 5 requests/minuto en `Login` y `RequestToken` (retorna 429).
- **JWT SecretKey**: Leída desde env var `JWT_SECRET_KEY` con fallback a `appsettings.json`.
- **Refresh tokens**: Rotación de tokens con fecha de expiración.
- **Autorización por endpoint**: `Revoke` y `Me` requieren JWT. Solo `Login` y `RequestToken` son anónimos.
- **Hashing de contraseñas**: PBKDF2 con salt configurable.
- **Estructura de permisos**: Roles por usuario + módulos con permisos CRUD por rol.

### API
- Endpoints RESTful bajo `api/[Controller]`.
- Respuestas envueltas en `ApiResponse<T>` con metadatos de paginación.
- Paginación vía `PagedList<T>` con filtros por query string.
- IDs tipo `Guid` (desde `BaseEntity`).
- Swagger UI en ruta raíz (`/`) con autenticación JWT Bearer.
- Manejo global de excepciones (`GlobalExceptionFilter`).
- Validación automática con FluentValidation.
- CORS configurado (`AllowAllOrigins`).

### Datos
- **EF Core** con Fluent API para configuración de modelos.
- **PostgreSQL** como motor principal (vía Npgsql).
- **SQL Server** disponible como alternativa.
- **Auditoría** en todas las entidades (`CreatedAt`, `Responsable`).
- **Soporte para relaciones** en consultas (`Include` vía expression functions).

### Repositorio (`BaseRepository<T>`)
- Operaciones CRUD genéricas con `IQueryable<T>`.
- **SaveChanges unificado**: Ningún método guarda automáticamente — `UnitOfWork` es el único orquestador.
- **Delete por entidad**: Overload `Delete(T entity)` disponible (evita roundtrips).
- **AsNoTracking**: Métodos `GetAsNoTracking()` y `GetByIdAsNoTracking()` para lecturas sin tracking.
- **Helpers**: `AnyAsync()`, `CountAsync()`, `FirstOrDefaultAsync()` con filtro opcional.
- **Inserción/actualización masiva**: `Insert(List<T>)`, `Update(List<T>)`, `AddRangeAsync()`, `UpdateRangeAsync()`.
- **Operaciones sin commit**: `AddAsync()`, `AddRangeAsync()`, `UpdateRangeAsync()` para uso con UnitOfWork.

### Servicios y Background
- **Servicio Genérico** (`GenericService<T>`) para operaciones CRUD comunes.
- **Hangfire** para envío de emails asíncronos (OTP, bienvenida).
- **Serilog** con rolling file en `logs/SocialMedia.txt`.
- **AutoMapper** con perfiles auto-descubiertos desde `Mappings/`.

## Comandos

```powershell
# Build
dotnet build .\SocialMedia\SocialMedia.sln

# Run API
dotnet run --project .\SocialMedia\SocialMedia.Api

# Tests unitarios
dotnet test .\SocialMedia\TestProject\TestProject.csproj

# Tests de integración
dotnet test .\SocialMedia\SocialMedia.IntegrationTests\SocialMedia.IntegrationTests.csproj

# EF Core migrations (ejecutar desde SocialMedia/)
dotnet ef migrations add MigrationName --project .\SocialMedia.Infrastructure --startup-project .\SocialMedia.Api
dotnet ef database update --project .\SocialMedia.Infrastructure --startup-project .\SocialMedia.Api

# Docker
docker build -t socialmedia-api -f SocialMedia/Dockerfile .
```

## Variables de Entorno

| Variable | Obligatorio | Descripción |
|----------|-------------|-------------|
| `JWT_SECRET_KEY` | Recomendado | Clave secreta para firmar JWT (mín. 32 caracteres). Si no se define, usa el valor en `appsettings.json` |

## Agradecimientos

Gracias a @force4Win - Alvaro Andres Obregon por su colaboración y apoyo en este proyecto.
