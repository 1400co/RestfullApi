# Restful API

API RESTful construida con **ASP.NET Core 10** y **EF Core**, siguiendo **Clean Architecture** con patrón **Repository + Unit of Work**.

## Stack

- **Framework**: ASP.NET Core 10, C#
- **ORM**: Entity Framework Core 10 con PostgreSQL (principal) y SQL Server (alternativo)
- **Autenticación**: JWT Bearer + Refresh Tokens + OTP + Password Recovery
- **Mapeo**: AutoMapper (auto-descubrimiento desde ensamblados)
- **Validación**: FluentValidation con auto-validación
- **Logs**: Serilog (archivo + consola + detalles de excepción via `Serilog.Exceptions`)
- **Background Jobs**: Hangfire con PostgreSQL (`WorkerCount = 1`)
- **Documentación**: Swagger / Swashbuckle multi-versión (v1 activa, v2 preparada)
- **Rate Limiting**: Fixed window (5 req/min en auth)
- **JSON Serialization**: Newtonsoft.Json (ReferenceLoopHandling.Ignore, NullValueHandling.Ignore)
- **Email**: MailKit + MimeKit (SMTP)
- **Dynamic Queries**: System.Linq.Dynamic.Core
- **Contenedor**: Docker
- **CI/CD**: GitHub Actions → Build automático en push a `main`
- **IA / Asistencia**: OpenCode con agentes y MCPs configurados (`.opencode/`)

## Arquitectura

```
SocialMedia.sln
├── SocialMedia.Api              # ASP.NET Core Web API (entry point)
│   ├── Controllers/             #   User, Post, Token, Modules, Cervezas, RolModule, Roles (placeholder)
│   ├── Responses/               #   ApiResponse<T> wrapper
│   ├── Startup.cs               #   DI, middleware, auth, CORS, rate limiting, Swagger
│   └── Program.cs               #   Host builder, Serilog, Npgsql timestamp
│
├── SocialMedia.Core             # Capa de dominio y aplicación
│   ├── Entities/                #   BaseEntity, User, Post, Comment, Modules, RolModule, Cervezas, Otp
│   ├── Dtos/                    #   17 DTOs (User, Post, Comment, Login, Auth, Token, etc.)
│   ├── Interfaces/              #   Contratos: repositorio genérico, UnitOfWork, servicios
│   ├── Services/                #   GenericService<T> + específicos (User, Post, Comment, Token, Security, etc.)
│   ├── QueryFilters/            #   BaseQueryFilter + filtros concretos con paginación
│   ├── CustomEntities/          #   PagedList, Metadata, LinkInfo, opciones de config, AuditLog
│   └── Enumerations/            #   RoleType, Subscription, PostType, FriendlyNameAttribute
│
├── SocialMedia.Infrastructure   # Capa de infraestructura
│   ├── Data/                    #   SocialMediaContext + Fluent API + Auditoría automática
│   ├── Repositories/            #   BaseRepository<T>, SecurityRepository, UnitOfWork
│   ├── Services/                #   EmailService (MailKit), PasswordService (PBKDF2), UriService, SessionService
│   ├── Filters/                 #   GlobalExceptionFilter (ProblemDetails RFC 7807)
│   ├── Validators/              #   PostValidators, CervezasValidators (FluentValidation)
│   ├── Mappings/                #   AutoMapper profiles
│   ├── Migrations/              #   EF Core migrations
│   └── Extentions/              #   DI registration extension methods
│
├── SocialMedia.IntegrationTests # Tests de integración (xUnit)
├── SocialMedia.Angular/         # Proyecto frontend Angular (pendiente)
└── SocialMedia.UnitTests/       # Tests unitarios (xUnit + Moq, net10.0)
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
| `POST` | `/login` | Anónimo (rate limited) | Canjear OTP por JWT + Refresh Token + permisos combinados |
| `POST` | `/renew` | JWT | Rotar Refresh Token |
| `DELETE` | `/` | JWT | Revocar Refresh Token |
| `GET` | `/me` | JWT | Obtener claims del usuario actual desde JWT |

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
| `GET` | `/` | JWT | Cervezas paginadas con filtros (Nombre, GradosAlcohol, Precio) |
| `GET` | `/{id}` | JWT | Obtener cerveza por ID |
| `POST` | `/` | JWT | Crear cerveza |
| `PUT` | `/{id}` | JWT | Actualizar cerveza |
| `DELETE` | `/{id}` | JWT | Eliminar cerveza |

> **Nota**: Los endpoints para `Roles`, `UserInRoles`, `Comment` y `PasswordRecovery` fueron removidos o están pendientes de reimplementación. Los roles se manejan vía `RoleType` enum en el `User`. Los servicios de `CommentService` y `PasswordRecoveryService` existen en la capa Core pero no tienen controller asociado.

## Autenticación y Seguridad

- **Login OTP**: El usuario solicita un código de un solo uso por email y lo canjea por un JWT.
- **OTP criptográfico**: Generado con `RandomNumberGenerator.GetInt32()`, no con `System.Random`.
- **OTP de un solo uso**: Se elimina de la BD tras validación exitosa, previniendo ataques de reuso.
- **Rate limiting**: 5 requests/minuto en `otp` y `login` (retorna 429) vía `System.Threading.RateLimiting`.
- **JWT SecretKey**: Leída desde env var `JWT_SECRET_KEY` con fallback a `appsettings.json`.
- **Refresh tokens**: Rotación de tokens (32 bytes criptográficos) con expiración a 2 días.
- **TokenService**: Generación y renovación de JWT + Refresh Tokens.
- **Hashing de contraseñas**: PBKDF2 con salt configurable (`PasswordService`).
- **JWT Claims**: `ClaimTypes.Name` (email), `ClaimTypes.NameIdentifier` (userId), `ClaimTypes.Role` (roles).
- **SessionService**: Helper que lee usuario/roles desde los claims JWT vía `IHttpContextAccessor` — sin roundtrip a BD.
- **AuthenticatedResponse**: Devuelve `AuthToken`, `RefreshToken`, `UserId`, `UserName` y `Permisos` (permisos combinados por módulo).
- **Renovación**: Endpoint `POST /api/v1/Token/renew` que extrae el principal del token expirado, valida el refresh token y emite uno nuevo.
- **Revocación**: `DELETE /api/v1/Token` invalida el refresh token del usuario.

## Roles y Permisos

- **Roles predefinidos vía enum `RoleType`** (`Administrator`, `Consumer`) — sin tablas dinámicas.
- Cada `User` tiene una `List<RoleType> Roles` almacenada como columna separada por comas en la BD.
- Autorización granular vía `RolModule` (entidad) que asigna permisos CRUD (`Created`, `Edited`, `Listed`, `Deleted`, `Printed`) por `RoleType` y `Module`.
- El rol `Administrator` obtiene permisos totales en todos los módulos automáticamente.
- **Permisos combinados**: `RolModuleService.ObtenerModulosUsuario()` combina permisos cuando un usuario tiene múltiples roles, retornando `RolModuleCombinadoDto`.
- **Permisos en login**: Al autenticarse, la respuesta incluye `Permisos` con la lista de módulos y permisos CRUD del usuario.
- **Roles removidos**: `RolesController`, `RolesService`, `RolesDto`, `UserInRoles*` fueron eliminados — la funcionalidad se migró al enum `RoleType`.

## Datos

- **EF Core** con Fluent API para configuración de modelos (`ApplyConfigurationsFromAssembly`).
- **PostgreSQL** como motor principal (vía Npgsql) — activo en `Startup.cs`.
- **SQL Server** disponible como alternativa (configurado en `ServiceCollectionExtention.AddDbContextsSqlServer()`).
- **DbSets**: `Users`, `Posts`, `Comments`, `RolModule`, `Otp`, `Cervezas`, `AuditLogs`.
- **Auditoría** automática vía `AuditLog` (ver sección de Auditoría).
- **Auditoría de entidades**: `BaseEntity` provee `Id` (Guid), `CreatedAt` y `Responsable` en todas las entidades.
- **Soporte para relaciones** en consultas (`Include` vía expression functions).
- **Lazy loading** habilitado via `Microsoft.EntityFrameworkCore.Proxies`.
- **Hardcoded fallback**: `SocialMediaContext.OnConfiguring()` contiene credenciales PostgreSQL de fallback (solo desarrollo local).

## Repositorio (`BaseRepository<T>`)

- Operaciones CRUD genéricas con `IQueryable<T>` y `Expression<Func<T, object>>[] includes`.
- **SaveChanges unificado**: `UnitOfWork` es el único orquestador.
- **Delete por entidad**: Overload `Delete(T entity)` disponible (evita roundtrips).
- **AsNoTracking**: Métodos `GetAsNoTracking()` y `GetByIdAsNoTracking()` para lecturas sin tracking.
- **Helpers**: `AnyAsync()`, `CountAsync()`, `FirstOrDefaultAsync()` con filtro opcional.
- **Inserción/actualización masiva**: `Insert(List<T>)`, `Update(List<T>)`, `AddRangeAsync()`, `UpdateRangeAsync()`.
- **Operaciones sin commit**: `AddAsync()`, `AddRangeAsync()`, `UpdateRangeAsync()` para uso con UnitOfWork.
- **Detach**: `Detach(T entity)` para desvincular entidades del contexto.
- **PostRepository**: Extiende `IRepository<Post>` con `GetPostsByUser()`.

## Servicios Genéricos

- `GenericService<T>` para operaciones CRUD comunes (Insert, Update, Delete, Get, GetAll, Get paginado con **filtro dinámico** via `System.Linq.Dynamic.Core` que busca en todas las propiedades string de la entidad).
- Servicios específicos heredan de `GenericService<T>`: `UserService`, `PostService`, `CommentService`, `ModuleService`, `RolModuleService`, `CervezasService`, `SecurityService`, `TokenService`.
- Métodos base reutilizados y delegados con `base.Method()` para reducir código duplicado.
- **Reglas de negocio** en servicios específicos: filtro de contenido (`PostService` bloquea "sexo"), rate limiting de posts (máx. 1 cada 7 días si tiene < 10 posts), validación de existencia de usuario, etc.

## Servicios de Infraestructura

| Servicio | Descripción |
|----------|-------------|
| `EmailService` | Envío de emails SMTP via **MailKit** + **MimeKit** (usado por Hangfire para OTP, bienvenida) |
| `PasswordService` | Hashing PBKDF2 con salt configurable (`Hash()`, `Check()`) |
| `SessionService` | Lee `UserId`, `UserName`, `Roles` y `CurrentUser` desde claims JWT vía `IHttpContextAccessor` |
| `UriService` | Construye URLs de paginación para HATEOAS |
| `TokenService` | Genera JWT, genera/rota Refresh Tokens (32 bytes criptográficos), extrae principal de token expirado |

## Background Jobs

- **Hangfire** con **PostgreSQL** como storage, configurado con `WorkerCount = 1`.
- Envío de emails asíncronos: OTP, bienvenida de nuevo usuario.
- Dashboard accesible vía `/hangfire`.

## Logging

- **Serilog** con rolling file en `logs/SocialMedia.txt` + consola.
- `Serilog.Exceptions` para enriquecer logs con detalles de excepción.
- `UseSerilogRequestLogging()` en el pipeline para log de requests HTTP.

## AutoMapper

- Perfiles auto-descubiertos desde todos los ensamblados cargados (`AppDomain.CurrentDomain.GetAssemblies()`).
- Mappings definidos en `SocialMedia.Infrastructure/Mappings/AutoMapperProfile.cs`.

## Comandos

```powershell
# Build
dotnet build .\SocialMedia\SocialMedia.sln

# Run API
dotnet run --project .\SocialMedia\SocialMedia.Api

# Tests de integración
dotnet test .\SocialMedia\SocialMedia.IntegrationTests\SocialMedia.IntegrationTests.csproj

# Tests unitarios
dotnet test .\SocialMedia\SocialMedia.UnitTests\SocialMedia.UnitTests.csproj

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

## Enumeraciones

| Enum | Valores | Descripción |
|------|---------|-------------|
| `RoleType` | `Administrator`, `Consumer` | Roles del sistema |
| `Subscription` | `Free`, `Basic` | Tipo de suscripción del usuario |
| `PostType` | `Image`, `Video`, `Party` | Tipo de contenido del post |
| `FriendlyNameAttribute` | — | Atributo personalizado para nombres amigables en enums |

## Auditoría Automática

El sistema registra automáticamente todos los cambios (Insert, Update, Delete) en la tabla `AuditLogs` mediante hooks en `SaveChanges()` / `SaveChangesAsync()`:

- **Captura**: `OnBeforeSaveChanges()` recorre el `ChangeTracker` y captura valores antiguos/nuevos, tipo de acción y nombre de tabla.
- **Persistencia**: `OnAfterSaveChanges()` / `OnAfterSaveChangesAsync()` completa las entradas con propiedades temporales (ej. IDs generados) y las guarda.
- **Estructura de `AuditLog`**: `AuditLogId`, `TableName`, `ActionType`, `Timestamp`, `KeyValues` (JSON), `OldValues` (JSON), `NewValues` (JSON).
- **Clase helper `AuditEntry`**: Convierte `EntityEntry` de EF Core a `AuditLog`.

## Knowledge Base

El proyecto incluye una base de conocimiento estructurada y versionable en `knowledge-base/` que sigue una metodología de trazabilidad completa:

```
knowledge-base/
├── 01-discovery/        # Documentos, entrevistas, procesos, preguntas abiertas
├── 02-domain/           # Actores, entidades, estados, eventos, glosario, reglas de negocio
├── 03-requirements/     # Casos de uso, criterios de aceptación, historias de usuario
├── 04-backlog/          # Épicas, features, roadmap
├── 05-architecture/     # ADR, arquitectura, integraciones, modelo de dominio
├── 06-decisions/        # Decisiones arquitectónicas y técnicas
└── 07-ai-context/       # Contexto de producto, prompts para asistentes IA
```

## Configuración de IA (OpenCode)

El directorio `.opencode/` contiene la configuración del asistente de desarrollo:

| Archivo | Descripción |
|---------|-------------|
| `opencode.json` | Configura MCPs: `microsoft_learn` (docs Microsoft), `sequential-thinking`, `context7` (docs de librerías) |
| `agents/analista.md` | Agente especializado en análisis de requerimientos |
| `rules/AGENTS.md` | Metadata del proyecto, convenciones, DI wiring, testing |
| `rules/AI_PROJECT_RULES.md` | Metodología de gestión de proyecto con trazabilidad completa |

### Subagente Analista

El proyecto incluye un **subagente analista** que transforma actas de entrevista en requerimientos estructurados con trazabilidad completa.

**¿Cómo usarlo?**

1. Coloca el acta de entrevista en `knowledge-base/01-discovery/entrevistas/` (formato Markdown).
2. En la conversación con OpenCode, escribe:

   ```
   @analista Procesa el acta de entrevista en knowledge-base/01-discovery/entrevistas/[archivo].md y genera todos los artefactos de requerimientos siguiendo AI_PROJECT_RULES.md
   ```
3. El agente leerá el acta, extraerá hallazgos (`H-XXX`) y generará automáticamente:
   - `02-domain/actores.md` — Actores del sistema (`ACT-XXX`)
   - `02-domain/entidades.md` — Entidades del dominio (`ENT-XXX`)
   - `02-domain/reglas-negocio.md` — Reglas de negocio (`RN-XXX`)
   - `02-domain/estados.md` — Máquina de estados por entidad
   - `02-domain/eventos-negocio.md` — Eventos de negocio (`EVN-XXX`)
   - `03-requirements/casos-uso/` — Casos de uso (`CU-XXX.md`)
   - `03-requirements/historias/` — Historias de usuario (`HU-XXX.md`) en formato Gherkin
   - `03-requirements/criterios/` — Criterios de aceptación (`CA-XXX.md`)
   - `04-backlog/epicas.md` — Épicas (`EP-XXX`)
   - `04-backlog/features.md` — Features (`FEAT-XXX`)

**Reglas del agente:**

- Solo trabaja sobre `knowledge-base/` — no puede leer ni modificar código fuente.
- Usa **Sequential Thinking** para descomponer cada entrevista en pasos lógicos.
- Exige trazabilidad completa: ningún artefacto se crea sin poder rastrearse hasta un hallazgo validado.
- Cada artefacto sigue la cadena: `Hallazgo → Regla de Negocio → Entidad → Épica → Feature → Historia de Usuario → Criterio de Aceptación`.
- Identificadores correlativos globales por tipo (`H-001`, `RN-001`, `ACT-001`, etc.).
- Los criterios de aceptación siempre en **formato Gherkin** (`Dado/Cuando/Entonces`).

## Middleware Pipeline

Orden de ejecución en el pipeline de ASP.NET Core:

1. `UseDeveloperExceptionPage()` (solo desarrollo)
2. `UseSerilogRequestLogging()` — logging de requests
3. `UseHttpsRedirection()`
4. `UseSwagger()` + `UseSwaggerUI()` (en `/`)
5. `UseCors("AllowAllOrigins")` — permitir cualquier origen/header/método
6. `UseRateLimiter()` — AuthPolicy (5 req/min)
7. `UseHangfireDashboard()` — dashboard en `/hangfire`
8. `UseRouting()`
9. `UseAuthentication()` — JWT Bearer
10. `UseAuthorization()`
11. `UseEndpoints()` → `MapControllers()`

## DI Registration (Scopes)

Registros en `ServiceCollectionExtention.AddServices()`:

| Interfaz | Implementación | Scope |
|----------|---------------|-------|
| `IRepository<T>` | `BaseRepository<T>` | Scoped |
| `IGenericService<T>` | `GenericService<T>` | Scoped |
| `IUnitOfWork` | `UnitOfWork` | Transient |
| `IEmailService` | `EmailService` | Transient |
| `ISessionService` | `SessionService` | Transient |
| `ITokenService` | `TokenService` | Transient |
| `IUserService` | `UserService` | Transient |
| `IRolModuleService` | `RolModuleService` | Transient |
| `ICommentService` | `CommentService` | Transient |
| `IPostService` | `PostService` | Transient |
| `ISecurityService` | `SecurityService` | Transient |
| `IModuleService` | `ModuleService` | Transient |
| `ICervezasService` | `CervezasService` | Transient |
| `IPasswordService` | `PasswordService` | Singleton |
| `IUriService` | `UriService` | Singleton |

### Opciones registradas (desde `appsettings.json`)

- `PaginationOptions` — página/tamaño por defecto
- `PasswordOptions` — configuración PBKDF2
- `AuthenticationOptions` — Issuer, Audience, SecretKey
- `SmtpSettings` — Server, Port, Username, Password, UseSSL, Url

## Entities

Todas las entidades heredan de `BaseEntity` (abstract): `Id` (Guid auto-generado), `CreatedAt`, `Responsable`.

| Entidad | Descripción | Relaciones |
|---------|-------------|------------|
| `User` | Usuario del sistema | FK a `Comment`, `Post` |
| `Post` | Publicación con contenido multimedia | FK a `User`; `PostType` enum |
| `Comment` | Comentario en un post | FK a `Post`, `User` |
| `Modules` | Módulo funcional del sistema | FK a `RolModule` |
| `RolModule` | Permiso CRUD por rol y módulo | FK a `Modules` |
| `Otp` | Código de un solo uso para autenticación | FK a `User` |
| `Cervezas` | Catálogo de cervezas (ejemplo CRUD) | — |

## DTOs

| DTO | Namespace | Descripción |
|-----|-----------|-------------|
| `BaseDto` | `SocialMedia.Core.Dtos` | Base abstract: Id, CreatedAt, Responsable |
| `UserDto` | `SocialMedia.Core.Dtos` | FullName, Email, BornDate, Phone, IsActive, RefreshToken, Subscription, Roles |
| `UserModelDto` | `SocialMedia.Core.Dtos` | Simplified: FullName, Email, Subscription, UserName, Roles |
| `PostDto` | `SocialMedia.Core.Dtos` | UserId, Date, Description, Image |
| `CommentDto` | `SocialMedia.Core.Dtos` | Description, IsActive, PostId, UserId |
| `ModulesDto` | `SocialMedia.Core.Dtos` | ModuleName |
| `RolModuleDto` | `SocialMedia.Core.Dtos` | Module, Created, Edited, Listed, Deleted, Printed, Role |
| `RolModuleCombinadoDto` | `TransforSerPu.Core.Dtos` | Permisos combinados de todos los roles del usuario |
| `CervezasDto` | `SocialMedia.Core.Dtos` | Nombre, GradosAlcohol, Precio |
| `Login` | `SocialMedia.Core.Dtos` | Email, Otp |
| `TokenDto` | `TransforSerPu.Core.Dtos` | AccessToken, RefreshToken |
| `AuthenticatedResponse` | `TransforSerPu.Core.Dtos` | AuthToken, RefreshToken, ExpiresIn, UserId, UserName, Permisos |
| `OtpDto` | `SocialMedia.Core.Dtos` | UserId, Password, ExpireDate |
| `PasswordRecoveryDto` | `SocialMedia.Core.Dtos` | UserName, PasswordRecoveryToken, ExpiryDate, UserId |
| `PasswordUpdate` | `SocialMedia.Core.Dtos` | PasswordRecoveryToken, Password |

## Reglas de Negocio en Servicios

| Servicio | Regla |
|----------|-------|
| `PostService.InsertPost()` | Valida que el usuario exista; bloquea contenido con "sexo"; rate limit: 1 post cada 7 días si tiene < 10 posts |
| `PostService.UpdatePost()` | Lanza `BusinessException` si el post no existe |
| `CommentService.Insert()` | Valida que el usuario y post existan; bloquea contenido con "sexo" |
| `UserService.InsertUser()` | Valida que el email no esté vacío; lanza `BusinessException` si falta |
| `CervezasService` | Valida Nombre requerido, Precio > 0, GradosAlcohol entre 0 y 100 |
| `ModuleService.Insert()` | Valida ModuleName requerido |
| `SecurityService` | OTP se elimina post-uso (single-use); validación de credenciales |

## Validadores FluentValidation

| Validador | Reglas |
|-----------|--------|
| `PostValidators` | Description: not null, 10–500 chars; Date: not null, debe ser pasada |
| `CervezasValidators` | Nombre: not null, 1–100 chars; GradosAlcohol: 0–100; Precio: > 0 |

## Query Filters

| Filtro | Propiedades adicionales |
|--------|------------------------|
| `BaseQueryFilter` | `PageNumber`, `PageSize`, `Filter` (texto de búsqueda general) |
| `UserQueryFilter` | `UserId`, `FullName`, `Email`, `BornDate`, `Phone`, `IsActive`, `Subscription` |
| `PostQueryFilter` | `UserId`, `Date`, `Description` |
| `CommentQueryFilter` | `UserId`, `Description` |
| `ModulesQueryFilter` | (solo paginación) |
| `RolModuleQueryFilter` | (solo paginación) |
| `CervezasQueryFilter` | `Nombre`, `GradosAlcoholMin`, `GradosAlcoholMax`, `PrecioMin`, `PrecioMax` |
| `GeneralQueryFilter` | Filtro dinámico usado por `GenericService<T>.Get()` |

> `GeneralQueryFilter` es usado por `GenericService<T>` para aplicar búsqueda dinámica sobre todas las propiedades string de la entidad via `System.Linq.Dynamic.Core`.

## Agradecimientos

Gracias a @force4Win - Alvaro Andres Obregon por su colaboración y apoyo en este proyecto.
