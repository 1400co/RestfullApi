# Restful API

API RESTful construida con **ASP.NET Core 10** y **EF Core**, siguiendo **Clean Architecture** con patrón **Repository + Unit of Work**.

## Stack

- **Framework**: ASP.NET Core 10, C#
- **ORM**: Entity Framework Core 10 con PostgreSQL (principal) y SQL Server (alternativo)
- **Autenticación**: JWT Bearer + Refresh Tokens + OTP
- **Mapeo**: AutoMapper
- **Validación**: FluentValidation
- **Logs**: Serilog (archivo + consola)
- **Background Jobs**: Hangfire con PostgreSQL
- **Documentación**: Swagger / Swashbuckle
- **Rate Limiting**: Fixed window (5 req/min en auth)
- **Contenedor**: Docker
- **CI/CD**: GitHub Actions → Azure Web App

## Arquitectura

```
SocialMedia.sln
├── SocialMedia.Api              # ASP.NET Core Web API (entry point)
│   ├── Controllers/             #   User, Post, Token, Modules, Cervezas, RolModule
│   ├── Responses/               #   ApiResponse<T> wrapper
│   ├── Startup.cs               #   DI, middleware, auth config
│   └── Program.cs               #   Host builder, Serilog, Npgsql timestamp
│
├── SocialMedia.Core             # Capa de dominio y aplicación
│   ├── Entities/                #   Modelos de dominio (Post, User, Comment, Modules, etc.)
│   ├── Dtos/                    #   Objetos de transferencia
│   ├── Interfaces/              #   Contratos de servicios y repositorios
│   ├── Services/                #   Lógica de negocio (GenericService<T> + específicos)
│   ├── QueryFilters/            #   Filtros con paginación (heredan de BaseQueryFilter)
│   ├── CustomEntities/          #   PagedList, Metadata, LinkInfo, opciones de config
│   └── Enumerations/            #   Enums (RoleType, Subscription, etc.)
│
├── SocialMedia.Infrastructure   # Capa de infraestructura
│   ├── Data/                    #   DbContext + Fluent API configurations
│   ├── Repositories/            #   BaseRepository, PostRepository, UnitOfWork
│   ├── Services/                #   EmailService, PasswordService, UriService, SessionService
│   ├── Filters/                 #   GlobalExceptionFilter (ProblemDetails)
│   ├── Validators/              #   FluentValidation validators
│   ├── Mappings/                #   AutoMapper profiles
│   ├── Migrations/              #   EF Core migrations
│   └── Extentions/              #   DI registration extension methods
│
└── SocialMedia.IntegrationTests # Tests de integración (xUnit)
```

## Características RESTful

### API Versioning
- Rutas versionadas: **`/api/v1/[controller]`**
- Swagger multi-versión (v1 activa, v2 preparada para futuros cambios)
- Sin breaking changes: cada versión puede evolucionar independientemente

### HTTP Status Codes
| Método | Código | Descripción |
|--------|--------|-------------|
| `GET` | `200 OK` | Listado paginado o recurso individual |
| `POST` | `201 Created` | Recurso creado + `Location` header vía `CreatedAtAction` |
| `PUT` | `204 No Content` | Recurso actualizado (idempotente) |
| `PATCH` | `204 No Content` | Actualización parcial del recurso |
| `DELETE` | `204 No Content` | Recurso eliminado (idempotente) |
| `400` | `Bad Request` | Error de negocio (`BusinessException`) con `ProblemDetails` |
| `401` | `Unauthorized` | Autenticación requerida |
| `404` | `Not Found` | Recurso no encontrado |

### HATEOAS (Hypermedia as the Engine of Application State)
Cada respuesta incluye enlaces navegables en el `Links` de `ApiResponse<T>`:

```json
{
  "data": { ... },
  "meta": {
    "nextPageUrl": "/api/v1/users?PageNumber=2&PageSize=10",
    "previousPageUrl": null
  },
  "links": [
    { "rel": "self",     "href": "/api/v1/users",         "method": "GET" },
    { "rel": "create",   "href": "/api/v1/users",         "method": "POST" },
    { "rel": "update",   "href": "/api/v1/users/{id}",    "method": "PUT" },
    { "rel": "delete",   "href": "/api/v1/users/{id}",    "method": "DELETE" },
    { "rel": "collection","href": "/api/v1/users",        "method": "GET" }
  ]
}
```

- **Recursos individuales**: `self`, `update`, `delete`, `collection`
- **Colecciones**: `self`, `create`, enlaces de paginación (`next`, `previous`)

### Content Negotiation
- Todos los controllers decorados con `[Consumes("application/json")]` y `[Produces("application/json")]`
- Respuestas de error en formato `application/problem+json` (RFC 7807)

### Partial Updates (PATCH)
- Endpoints `PATCH /api/v1/{resource}/{id}` con `JsonPatchDocument<T>`
- Permite actualizar campos específicos sin reemplazar el recurso completo
- Disponible en: `User`, `Post`

### Error Handling (RFC 7807 Problem Details)
Errores de validación y negocio retornan:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "User doesn't exist",
  "instance": "/api/v1/users"
}
```

- `BusinessException` → `400 Bad Request` con detalle del error
- Errores no controlados → `500 Internal Server Error`
- Errores de validación → validación estándar de ASP.NET Core

### Paginación Consistente
- Todos los filtros de paginación heredan de `BaseQueryFilter`:
  - `PageNumber` — número de página
  - `PageSize` — elementos por página
  - `Filter` — texto de búsqueda general
- `Metadata` incluido en todas las respuestas paginadas con `TotalCount`, `TotalPages`, `HasNextPage`, `HasPreviousPage`, `NextPageUrl`, `PreviousPageUrl`
- Header `X-Pagination` con el mismo metadata serializado

## Endpoints

### `api/v1/User`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Lista paginada de usuarios |
| `GET` | `/{id}` | JWT | Obtener usuario por ID |
| `POST` | `/` | Anónimo | Crear usuario (asigna rol Consumer) |
| `PUT` | `/{id}` | JWT | Actualizar usuario completo |
| `PATCH` | `/{id}` | JWT | Actualizar campos específicos |
| `DELETE` | `/{id}` | JWT | Eliminar usuario |

### `api/v1/Post`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Lista paginada de posts |
| `GET` | `/{id}` | JWT | Obtener post por ID |
| `POST` | `/` | JWT | Crear post |
| `PUT` | `/{id}` | JWT | Actualizar post completo |
| `PATCH` | `/{id}` | JWT | Actualizar campos específicos |
| `DELETE` | `/{id}` | JWT | Eliminar post |

### `api/v1/Token`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `POST` | `/otp` | Anónimo (rate limited) | Solicitar código OTP por email |
| `POST` | `/login` | Anónimo (rate limited) | Canjear OTP por JWT + Refresh Token |
| `POST` | `/renew` | JWT | Rotar Refresh Token |
| `DELETE` | `/` | JWT | Revocar Refresh Token |
| `GET` | `/me` | JWT | Obtener usuario actual desde JWT |

### `api/v1/Modules`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `GET` | `/all` | JWT | Todos los módulos (sin paginación) |
| `GET` | `/` | JWT | Módulos paginados |
| `GET` | `/{id}` | JWT | Obtener módulo por ID |
| `POST` | `/` | JWT | Crear módulo |
| `PUT` | `/{id}` | JWT | Actualizar módulo |
| `DELETE` | `/{id}` | JWT | Eliminar módulo |

### `api/v1/RolModule`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Permisos paginados |
| `GET` | `/{id}` | JWT | Obtener permiso por ID |
| `POST` | `/` | JWT | Crear permiso |
| `PUT` | `/{id}` | JWT | Actualizar permiso |
| `DELETE` | `/{id}` | JWT | Eliminar permiso |

### `api/v1/Cervezas`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Cervezas paginadas con filtros |
| `GET` | `/{id}` | JWT | Obtener cerveza por ID |
| `POST` | `/` | JWT | Crear cerveza |
| `PUT` | `/{id}` | JWT | Actualizar cerveza |
| `DELETE` | `/{id}` | JWT | Eliminar cerveza |

## Autenticación y Seguridad

- **Login OTP**: El usuario solicita un código de un solo uso por email y lo canjea por un JWT.
- **OTP criptográfico**: Generado con `RandomNumberGenerator.GetInt32()`, no con `System.Random`.
- **OTP de un solo uso**: Se elimina de la BD tras validación exitosa, previniendo ataques de reuso.
- **Rate limiting**: 5 requests/minuto en `otp` y `login` (retorna 429).
- **JWT SecretKey**: Leída desde env var `JWT_SECRET_KEY` con fallback a `appsettings.json`.
- **Refresh tokens**: Rotación de tokens con fecha de expiración.
- **Hashing de contraseñas**: PBKDF2 con salt configurable.
- **JWT Claims**: `ClaimTypes.Name` (email), `ClaimTypes.NameIdentifier` (userId), `ClaimTypes.Role` (roles).
- **SessionService**: Helper que lee usuario/roles desde los claims JWT vía `IHttpContextAccessor` — sin roundtrip a BD.

## Roles y Permisos

- **Roles predefinidos vía enum `RoleType`** (`Administrator`, `Consumer`) — sin tablas dinámicas.
- Cada `User` tiene una `List<RoleType> Roles` almacenada como columna separada por comas en la BD.
- Autorización granular vía `RolModule` (entidad) que asigna permisos CRUD por `RoleType` y `Module`.
- El rol `Administrator` obtiene permisos totales en todos los módulos automáticamente.
- **Estructura de permisos**: Módulos con permisos CRUD combinados cuando un usuario tiene múltiples roles.

## Datos

- **EF Core** con Fluent API para configuración de modelos.
- **PostgreSQL** como motor principal (vía Npgsql).
- **SQL Server** disponible como alternativa.
- **Auditoría** en todas las entidades (`CreatedAt`, `Responsable`).
- **Soporte para relaciones** en consultas (`Include` vía expression functions).

## Repositorio (`BaseRepository<T>`)

- Operaciones CRUD genéricas con `IQueryable<T>`.
- **SaveChanges unificado**: `UnitOfWork` es el único orquestador.
- **Delete por entidad**: Overload `Delete(T entity)` disponible (evita roundtrips).
- **AsNoTracking**: Métodos `GetAsNoTracking()` y `GetByIdAsNoTracking()` para lecturas sin tracking.
- **Helpers**: `AnyAsync()`, `CountAsync()`, `FirstOrDefaultAsync()` con filtro opcional.
- **Inserción/actualización masiva**: `Insert(List<T>)`, `Update(List<T>)`, `AddRangeAsync()`, `UpdateRangeAsync()`.
- **Operaciones sin commit**: `AddAsync()`, `AddRangeAsync()`, `UpdateRangeAsync()` para uso con UnitOfWork.

## Servicios Genéricos

- `GenericService<T>` para operaciones CRUD comunes (Insert, Update, Delete, Get, GetAll, Get paginado con filtro dinámico).
- Servicios específicos heredan de `GenericService<T>`: `UserService`, `PostService`, `CommentService`, `ModuleService`, `RolModuleService`, `CervezasService`, `SecurityService`.
- Métodos base reutilizados y delegados con `base.Method()` para reducir código duplicado.

## Servicios y Background

- **Hangfire** para envío de emails asíncronos (OTP, bienvenida).
- **Serilog** con rolling file en `logs/SocialMedia.txt`.
- **AutoMapper** con perfiles auto-descubiertos desde `Mappings/`.

## Comandos

```powershell
# Build
dotnet build .\SocialMedia\SocialMedia.sln

# Run API
dotnet run --project .\SocialMedia\SocialMedia.Api

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
