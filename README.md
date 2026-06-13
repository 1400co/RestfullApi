# Restful API

API RESTful construida con **ASP.NET Core 10** y **EF Core**, siguiendo **Clean Architecture** con patrÃ³n **Repository + Unit of Work**.

## Stack

- **Framework**: ASP.NET Core 10, C#
- **ORM**: Entity Framework Core 10 con PostgreSQL (principal) y SQL Server (alternativo)
- **AutenticaciÃ³n**: JWT Bearer + Refresh Tokens + OTP + Password Recovery
- **Mapeo**: AutoMapper (auto-descubrimiento desde ensamblados)
- **ValidaciÃ³n**: FluentValidation con auto-validaciÃ³n
- **Logs**: Serilog (archivo + consola + detalles de excepciÃ³n via `Serilog.Exceptions`)
- **Background Jobs**: Hangfire con PostgreSQL (`WorkerCount = 1`)
- **DocumentaciÃ³n**: Swagger / Swashbuckle multi-versiÃ³n (v1 activa, v2 preparada)
- **Rate Limiting**: Fixed window (5 req/min en auth)
- **JSON Serialization**: Newtonsoft.Json (ReferenceLoopHandling.Ignore, NullValueHandling.Ignore)
- **Email**: MailKit + MimeKit (SMTP)
- **Dynamic Queries**: System.Linq.Dynamic.Core
- **Contenedor**: Docker
- **CI/CD**: GitHub Actions â†’ Build automÃ¡tico en push a `main`
- **IA / Asistencia**: OpenCode con agentes y MCPs configurados (`.opencode/`)

## Arquitectura

```
Template.sln (raíz del repo)
├── src/
│   ├── Template.Api/             # ASP.NET Core Web API (entry point)
│   │   ├── Controllers/          #   User, Post, Token, Modules, Cervezas, RolModule, Roles (placeholder)
│   │   ├── Responses/            #   ApiResponse<T> wrapper
│   │   ├── Startup.cs            #   DI, middleware, auth, CORS, rate limiting, Swagger
│   │   └── Program.cs            #   Host builder, Serilog, Npgsql timestamp
│   │
│   ├── Template.Core/            # Capa de dominio y aplicación
│   │   ├── Entities/             #   BaseEntity, User, Post, Comment, Modules, RolModule, Cervezas, Otp
│   │   ├── Dtos/                 #   17 DTOs (User, Post, Comment, Login, Auth, Token, etc.)
│   │   ├── Interfaces/           #   Contratos: repositorio genérico, UnitOfWork, servicios
│   │   ├── Services/             #   GenericService<T> + específicos (User, Post, Comment, Token, Security, etc.)
│   │   ├── QueryFilters/         #   BaseQueryFilter + filtros concretos con paginación
│   │   ├── CustomEntities/       #   PagedList, Metadata, LinkInfo, opciones de config, AuditLog
│   │   └── Enumerations/         #   RoleType, Subscription, PostType, FriendlyNameAttribute
│   │
│   ├── Template.Infrastructure/  # Capa de infraestructura
│   │   ├── Data/                 #   TemplateContext + Fluent API + Auditoría automática
│   │   ├── Repositories/         #   BaseRepository<T>, SecurityRepository, UnitOfWork
│   │   ├── Services/             #   EmailService (MailKit), PasswordService (PBKDF2), UriService, SessionService
│   │   ├── Filters/              #   GlobalExceptionFilter (ProblemDetails RFC 7807)
│   │   ├── Validators/           #   PostValidators, CervezasValidators (FluentValidation)
│   │   ├── Mappings/             #   AutoMapper profiles
│   │   ├── Migrations/           #   EF Core migrations
│   │   └── Extentions/           #   DI registration extension methods
│   │
│   ├── Template.IntegrationTests # Tests de integración (xUnit)
│   ├── Template.Angular/         # Proyecto frontend Angular (pendiente)
│   ├── Dockerfile
│   └── Template.sln
│
├── .opencode/                    # Configuración de IA (OpenCode)
├── knowledge-base/               # Base de conocimiento del proyecto
└── README.md
```

## CaracterÃ­sticas RESTful

### API Versioning
- Rutas versionadas: **`/api/v1/[controller]`**
- Swagger multi-versiÃ³n (v1 activa, v2 preparada para futuros cambios)
- Sin breaking changes: cada versiÃ³n puede evolucionar independientemente

### HTTP Status Codes
| MÃ©todo | CÃ³digo | DescripciÃ³n |
|--------|--------|-------------|
| `GET` | `200 OK` | Listado paginado o recurso individual |
| `POST` | `201 Created` | Recurso creado + `Location` header vÃ­a `CreatedAtAction` |
| `PUT` | `204 No Content` | Recurso actualizado (idempotente) |
| `PATCH` | `204 No Content` | ActualizaciÃ³n parcial del recurso |
| `DELETE` | `204 No Content` | Recurso eliminado (idempotente) |
| `400` | `Bad Request` | Error de negocio (`BusinessException`) con `ProblemDetails` |
| `401` | `Unauthorized` | AutenticaciÃ³n requerida |
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
- **Colecciones**: `self`, `create`, enlaces de paginaciÃ³n (`next`, `previous`)

### Content Negotiation
- Todos los controllers decorados con `[Consumes("application/json")]` y `[Produces("application/json")]`
- Respuestas de error en formato `application/problem+json` (RFC 7807)

### Partial Updates (PATCH)
- Endpoints `PATCH /api/v1/{resource}/{id}` con `JsonPatchDocument<T>`
- Permite actualizar campos especÃ­ficos sin reemplazar el recurso completo
- Disponible en: `User`, `Post`

### Error Handling (RFC 7807 Problem Details)
Errores de validaciÃ³n y negocio retornan:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "User doesn't exist",
  "instance": "/api/v1/users"
}
```

- `BusinessException` â†’ `400 Bad Request` con detalle del error
- Errores no controlados â†’ `500 Internal Server Error`
- Errores de validaciÃ³n â†’ validaciÃ³n estÃ¡ndar de ASP.NET Core

### PaginaciÃ³n Consistente
- Todos los filtros de paginaciÃ³n heredan de `BaseQueryFilter`:
  - `PageNumber` â€” nÃºmero de pÃ¡gina
  - `PageSize` â€” elementos por pÃ¡gina
  - `Filter` â€” texto de bÃºsqueda general
- `Metadata` incluido en todas las respuestas paginadas con `TotalCount`, `TotalPages`, `HasNextPage`, `HasPreviousPage`, `NextPageUrl`, `PreviousPageUrl`
- Header `X-Pagination` con el mismo metadata serializado

## Endpoints

### `api/v1/User`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Lista paginada de usuarios |
| `GET` | `/{id}` | JWT | Obtener usuario por ID |
| `POST` | `/` | AnÃ³nimo | Crear usuario (asigna rol Consumer) |
| `PUT` | `/{id}` | JWT | Actualizar usuario completo |
| `PATCH` | `/{id}` | JWT | Actualizar campos especÃ­ficos |
| `DELETE` | `/{id}` | JWT | Eliminar usuario |

### `api/v1/Post`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Lista paginada de posts |
| `GET` | `/{id}` | JWT | Obtener post por ID |
| `POST` | `/` | JWT | Crear post |
| `PUT` | `/{id}` | JWT | Actualizar post completo |
| `PATCH` | `/{id}` | JWT | Actualizar campos especÃ­ficos |
| `DELETE` | `/{id}` | JWT | Eliminar post |

### `api/v1/Token`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `POST` | `/otp` | AnÃ³nimo (rate limited) | Solicitar cÃ³digo OTP por email |
| `POST` | `/login` | AnÃ³nimo (rate limited) | Canjear OTP por JWT + Refresh Token + permisos combinados |
| `POST` | `/renew` | JWT | Rotar Refresh Token |
| `DELETE` | `/` | JWT | Revocar Refresh Token |
| `GET` | `/me` | JWT | Obtener claims del usuario actual desde JWT |

### `api/v1/Modules`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `GET` | `/all` | JWT | Todos los mÃ³dulos (sin paginaciÃ³n) |
| `GET` | `/` | JWT | MÃ³dulos paginados |
| `GET` | `/{id}` | JWT | Obtener mÃ³dulo por ID |
| `POST` | `/` | JWT | Crear mÃ³dulo |
| `PUT` | `/{id}` | JWT | Actualizar mÃ³dulo |
| `DELETE` | `/{id}` | JWT | Eliminar mÃ³dulo |

### `api/v1/RolModule`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Permisos paginados |
| `GET` | `/{id}` | JWT | Obtener permiso por ID |
| `POST` | `/` | JWT | Crear permiso |
| `PUT` | `/{id}` | JWT | Actualizar permiso |
| `DELETE` | `/{id}` | JWT | Eliminar permiso |

### `api/v1/Cervezas`
| MÃ©todo | Ruta | Auth | DescripciÃ³n |
|--------|------|------|-------------|
| `GET` | `/` | JWT | Cervezas paginadas con filtros (Nombre, GradosAlcohol, Precio) |
| `GET` | `/{id}` | JWT | Obtener cerveza por ID |
| `POST` | `/` | JWT | Crear cerveza |
| `PUT` | `/{id}` | JWT | Actualizar cerveza |
| `DELETE` | `/{id}` | JWT | Eliminar cerveza |

> **Nota**: Los endpoints para `Roles`, `UserInRoles`, `Comment` y `PasswordRecovery` fueron removidos o estÃ¡n pendientes de reimplementaciÃ³n. Los roles se manejan vÃ­a `RoleType` enum en el `User`. Los servicios de `CommentService` y `PasswordRecoveryService` existen en la capa Core pero no tienen controller asociado.

## AutenticaciÃ³n y Seguridad

- **Login OTP**: El usuario solicita un cÃ³digo de un solo uso por email y lo canjea por un JWT.
- **OTP criptogrÃ¡fico**: Generado con `RandomNumberGenerator.GetInt32()`, no con `System.Random`.
- **OTP de un solo uso**: Se elimina de la BD tras validaciÃ³n exitosa, previniendo ataques de reuso.
- **Rate limiting**: 5 requests/minuto en `otp` y `login` (retorna 429) vÃ­a `System.Threading.RateLimiting`.
- **JWT SecretKey**: LeÃ­da desde env var `JWT_SECRET_KEY` con fallback a `appsettings.json`.
- **Refresh tokens**: RotaciÃ³n de tokens (32 bytes criptogrÃ¡ficos) con expiraciÃ³n a 2 dÃ­as.
- **TokenService**: GeneraciÃ³n y renovaciÃ³n de JWT + Refresh Tokens.
- **Hashing de contraseÃ±as**: PBKDF2 con salt configurable (`PasswordService`).
- **JWT Claims**: `ClaimTypes.Name` (email), `ClaimTypes.NameIdentifier` (userId), `ClaimTypes.Role` (roles).
- **SessionService**: Helper que lee usuario/roles desde los claims JWT vÃ­a `IHttpContextAccessor` â€” sin roundtrip a BD.
- **AuthenticatedResponse**: Devuelve `AuthToken`, `RefreshToken`, `UserId`, `UserName` y `Permisos` (permisos combinados por mÃ³dulo).
- **RenovaciÃ³n**: Endpoint `POST /api/v1/Token/renew` que extrae el principal del token expirado, valida el refresh token y emite uno nuevo.
- **RevocaciÃ³n**: `DELETE /api/v1/Token` invalida el refresh token del usuario.

## Roles y Permisos

- **Roles predefinidos vÃ­a enum `RoleType`** (`Administrator`, `Consumer`) â€” sin tablas dinÃ¡micas.
- Cada `User` tiene una `List<RoleType> Roles` almacenada como columna separada por comas en la BD.
- AutorizaciÃ³n granular vÃ­a `RolModule` (entidad) que asigna permisos CRUD (`Created`, `Edited`, `Listed`, `Deleted`, `Printed`) por `RoleType` y `Module`.
- El rol `Administrator` obtiene permisos totales en todos los mÃ³dulos automÃ¡ticamente.
- **Permisos combinados**: `RolModuleService.ObtenerModulosUsuario()` combina permisos cuando un usuario tiene mÃºltiples roles, retornando `RolModuleCombinadoDto`.
- **Permisos en login**: Al autenticarse, la respuesta incluye `Permisos` con la lista de mÃ³dulos y permisos CRUD del usuario.
- **Roles removidos**: `RolesController`, `RolesService`, `RolesDto`, `UserInRoles*` fueron eliminados â€” la funcionalidad se migrÃ³ al enum `RoleType`.

## Datos

- **EF Core** con Fluent API para configuraciÃ³n de modelos (`ApplyConfigurationsFromAssembly`).
- **PostgreSQL** como motor principal (vÃ­a Npgsql) â€” activo en `Startup.cs`.
- **SQL Server** disponible como alternativa (configurado en `ServiceCollectionExtention.AddDbContextsSqlServer()`).
- **DbSets**: `Users`, `Posts`, `Comments`, `RolModule`, `Otp`, `Cervezas`, `AuditLogs`.
- **AuditorÃ­a** automÃ¡tica vÃ­a `AuditLog` (ver secciÃ³n de AuditorÃ­a).
- **AuditorÃ­a de entidades**: `BaseEntity` provee `Id` (Guid), `CreatedAt` y `Responsable` en todas las entidades.
- **Soporte para relaciones** en consultas (`Include` vÃ­a expression functions).
- **Lazy loading** habilitado via `Microsoft.EntityFrameworkCore.Proxies`.
- **Hardcoded fallback**: `TemplateContext.OnConfiguring()` contiene credenciales PostgreSQL de fallback (solo desarrollo local).

## Repositorio (`BaseRepository<T>`)

- Operaciones CRUD genÃ©ricas con `IQueryable<T>` y `Expression<Func<T, object>>[] includes`.
- **SaveChanges unificado**: `UnitOfWork` es el Ãºnico orquestador.
- **Delete por entidad**: Overload `Delete(T entity)` disponible (evita roundtrips).
- **AsNoTracking**: MÃ©todos `GetAsNoTracking()` y `GetByIdAsNoTracking()` para lecturas sin tracking.
- **Helpers**: `AnyAsync()`, `CountAsync()`, `FirstOrDefaultAsync()` con filtro opcional.
- **InserciÃ³n/actualizaciÃ³n masiva**: `Insert(List<T>)`, `Update(List<T>)`, `AddRangeAsync()`, `UpdateRangeAsync()`.
- **Operaciones sin commit**: `AddAsync()`, `AddRangeAsync()`, `UpdateRangeAsync()` para uso con UnitOfWork.
- **Detach**: `Detach(T entity)` para desvincular entidades del contexto.
- **PostRepository**: Extiende `IRepository<Post>` con `GetPostsByUser()`.

## Servicios GenÃ©ricos

- `GenericService<T>` para operaciones CRUD comunes (Insert, Update, Delete, Get, GetAll, Get paginado con **filtro dinÃ¡mico** via `System.Linq.Dynamic.Core` que busca en todas las propiedades string de la entidad).
- Servicios especÃ­ficos heredan de `GenericService<T>`: `UserService`, `PostService`, `CommentService`, `ModuleService`, `RolModuleService`, `CervezasService`, `SecurityService`, `TokenService`.
- MÃ©todos base reutilizados y delegados con `base.Method()` para reducir cÃ³digo duplicado.
- **Reglas de negocio** en servicios especÃ­ficos: filtro de contenido (`PostService` bloquea "sexo"), rate limiting de posts (mÃ¡x. 1 cada 7 dÃ­as si tiene < 10 posts), validaciÃ³n de existencia de usuario, etc.

## Servicios de Infraestructura

| Servicio | DescripciÃ³n |
|----------|-------------|
| `EmailService` | EnvÃ­o de emails SMTP via **MailKit** + **MimeKit** (usado por Hangfire para OTP, bienvenida) |
| `PasswordService` | Hashing PBKDF2 con salt configurable (`Hash()`, `Check()`) |
| `SessionService` | Lee `UserId`, `UserName`, `Roles` y `CurrentUser` desde claims JWT vÃ­a `IHttpContextAccessor` |
| `UriService` | Construye URLs de paginaciÃ³n para HATEOAS |
| `TokenService` | Genera JWT, genera/rota Refresh Tokens (32 bytes criptogrÃ¡ficos), extrae principal de token expirado |

## Background Jobs

- **Hangfire** con **PostgreSQL** como storage, configurado con `WorkerCount = 1`.
- EnvÃ­o de emails asÃ­ncronos: OTP, bienvenida de nuevo usuario.
- Dashboard accesible vÃ­a `/hangfire`.

## Logging

- **Serilog** con rolling file en `logs/Template.txt` + consola.
- `Serilog.Exceptions` para enriquecer logs con detalles de excepciÃ³n.
- `UseSerilogRequestLogging()` en el pipeline para log de requests HTTP.

## AutoMapper

- Perfiles auto-descubiertos desde todos los ensamblados cargados (`AppDomain.CurrentDomain.GetAssemblies()`).
- Mappings definidos en `Template.Infrastructure/Mappings/AutoMapperProfile.cs`.

## Comandos

```powershell
# Build
dotnet build .\src\Template.sln

# Run API
dotnet run --project .\src\Template.Api

# Tests de integraciÃ³n
dotnet test .\src\Template.IntegrationTests\Template.IntegrationTests.csproj

# Tests unitarios
dotnet test .\src\Template.UnitTests\Template.UnitTests.csproj

# EF Core migrations (ejecutar desde Template/)
dotnet ef migrations add MigrationName --project .\src\Template.Infrastructure --startup-project .\src\Template.Api
dotnet ef database update --project .\src\Template.Infrastructure --startup-project .\src\Template.Api

# Docker
docker build -t Template-api -f Template/Dockerfile .
```

## Variables de Entorno

| Variable | Obligatorio | DescripciÃ³n |
|----------|-------------|-------------|
| `JWT_SECRET_KEY` | Recomendado | Clave secreta para firmar JWT (mÃ­n. 32 caracteres). Si no se define, usa el valor en `appsettings.json` |

## Enumeraciones

| Enum | Valores | DescripciÃ³n |
|------|---------|-------------|
| `RoleType` | `Administrator`, `Consumer` | Roles del sistema |
| `Subscription` | `Free`, `Basic` | Tipo de suscripciÃ³n del usuario |
| `PostType` | `Image`, `Video`, `Party` | Tipo de contenido del post |
| `FriendlyNameAttribute` | â€” | Atributo personalizado para nombres amigables en enums |

## AuditorÃ­a AutomÃ¡tica

El sistema registra automÃ¡ticamente todos los cambios (Insert, Update, Delete) en la tabla `AuditLogs` mediante hooks en `SaveChanges()` / `SaveChangesAsync()`:

- **Captura**: `OnBeforeSaveChanges()` recorre el `ChangeTracker` y captura valores antiguos/nuevos, tipo de acciÃ³n y nombre de tabla.
- **Persistencia**: `OnAfterSaveChanges()` / `OnAfterSaveChangesAsync()` completa las entradas con propiedades temporales (ej. IDs generados) y las guarda.
- **Estructura de `AuditLog`**: `AuditLogId`, `TableName`, `ActionType`, `Timestamp`, `KeyValues` (JSON), `OldValues` (JSON), `NewValues` (JSON).
- **Clase helper `AuditEntry`**: Convierte `EntityEntry` de EF Core a `AuditLog`.

## Knowledge Base

El proyecto incluye una base de conocimiento estructurada y versionable en `knowledge-base/` que sigue una metodologÃ­a de trazabilidad completa:

```
knowledge-base/
â”œâ”€â”€ 01-discovery/        # Documentos, entrevistas, procesos, preguntas abiertas
â”œâ”€â”€ 02-domain/           # Actores, entidades, estados, eventos, glosario, reglas de negocio
â”œâ”€â”€ 03-requirements/     # Casos de uso, criterios de aceptaciÃ³n, historias de usuario
â”œâ”€â”€ 04-backlog/          # Ã‰picas, features, roadmap
â”œâ”€â”€ 05-architecture/     # ADR, arquitectura, integraciones, modelo de dominio
â”œâ”€â”€ 06-decisions/        # Decisiones arquitectÃ³nicas y tÃ©cnicas
â””â”€â”€ 07-ai-context/       # Contexto de producto, prompts para asistentes IA
```

## ConfiguraciÃ³n de IA (OpenCode)

El directorio `.opencode/` contiene la configuraciÃ³n del asistente de desarrollo:

| Archivo | DescripciÃ³n |
|---------|-------------|
| `opencode.json` | Configura MCPs: `microsoft_learn` (docs Microsoft), `sequential-thinking`, `context7` (docs de librerÃ­as) |
| `agents/analista.md` | Agente especializado en anÃ¡lisis de requerimientos |
| `rules/AGENTS.md` | Metadata del proyecto, convenciones, DI wiring, testing |
| `rules/AI_PROJECT_RULES.md` | MetodologÃ­a de gestiÃ³n de proyecto con trazabilidad completa |

### Subagente Analista

El proyecto incluye un **subagente analista** que transforma actas de entrevista en requerimientos estructurados con trazabilidad completa.

**Â¿CÃ³mo usarlo?**

1. Coloca el acta de entrevista en `knowledge-base/01-discovery/entrevistas/` (formato Markdown).
2. En la conversaciÃ³n con OpenCode, escribe:

   ```
   @analista Procesa el acta de entrevista en knowledge-base/01-discovery/entrevistas/[archivo].md y genera todos los artefactos de requerimientos siguiendo AI_PROJECT_RULES.md
   ```
3. El agente leerÃ¡ el acta, extraerÃ¡ hallazgos (`H-XXX`) y generarÃ¡ automÃ¡ticamente:
   - `02-domain/actores.md` â€” Actores del sistema (`ACT-XXX`)
   - `02-domain/entidades.md` â€” Entidades del dominio (`ENT-XXX`)
   - `02-domain/reglas-negocio.md` â€” Reglas de negocio (`RN-XXX`)
   - `02-domain/estados.md` â€” MÃ¡quina de estados por entidad
   - `02-domain/eventos-negocio.md` â€” Eventos de negocio (`EVN-XXX`)
   - `03-requirements/casos-uso/` â€” Casos de uso (`CU-XXX.md`)
   - `03-requirements/historias/` â€” Historias de usuario (`HU-XXX.md`) en formato Gherkin
   - `03-requirements/criterios/` â€” Criterios de aceptaciÃ³n (`CA-XXX.md`)
   - `04-backlog/epicas.md` â€” Ã‰picas (`EP-XXX`)
   - `04-backlog/features.md` â€” Features (`FEAT-XXX`)

**Reglas del agente:**

- Solo trabaja sobre `knowledge-base/` â€” no puede leer ni modificar cÃ³digo fuente.
- Usa **Sequential Thinking** para descomponer cada entrevista en pasos lÃ³gicos.
- Exige trazabilidad completa: ningÃºn artefacto se crea sin poder rastrearse hasta un hallazgo validado.
- Cada artefacto sigue la cadena: `Hallazgo â†’ Regla de Negocio â†’ Entidad â†’ Ã‰pica â†’ Feature â†’ Historia de Usuario â†’ Criterio de AceptaciÃ³n`.
- Identificadores correlativos globales por tipo (`H-001`, `RN-001`, `ACT-001`, etc.).
- Los criterios de aceptaciÃ³n siempre en **formato Gherkin** (`Dado/Cuando/Entonces`).

## Middleware Pipeline

Orden de ejecuciÃ³n en el pipeline de ASP.NET Core:

1. `UseDeveloperExceptionPage()` (solo desarrollo)
2. `UseSerilogRequestLogging()` â€” logging de requests
3. `UseHttpsRedirection()`
4. `UseSwagger()` + `UseSwaggerUI()` (en `/`)
5. `UseCors("AllowAllOrigins")` â€” permitir cualquier origen/header/mÃ©todo
6. `UseRateLimiter()` â€” AuthPolicy (5 req/min)
7. `UseHangfireDashboard()` â€” dashboard en `/hangfire`
8. `UseRouting()`
9. `UseAuthentication()` â€” JWT Bearer
10. `UseAuthorization()`
11. `UseEndpoints()` â†’ `MapControllers()`

## DI Registration (Scopes)

Registros en `ServiceCollectionExtention.AddServices()`:

| Interfaz | ImplementaciÃ³n | Scope |
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

- `PaginationOptions` â€” pÃ¡gina/tamaÃ±o por defecto
- `PasswordOptions` â€” configuraciÃ³n PBKDF2
- `AuthenticationOptions` â€” Issuer, Audience, SecretKey
- `SmtpSettings` â€” Server, Port, Username, Password, UseSSL, Url

## Entities

Todas las entidades heredan de `BaseEntity` (abstract): `Id` (Guid auto-generado), `CreatedAt`, `Responsable`.

| Entidad | DescripciÃ³n | Relaciones |
|---------|-------------|------------|
| `User` | Usuario del sistema | FK a `Comment`, `Post` |
| `Post` | PublicaciÃ³n con contenido multimedia | FK a `User`; `PostType` enum |
| `Comment` | Comentario en un post | FK a `Post`, `User` |
| `Modules` | MÃ³dulo funcional del sistema | FK a `RolModule` |
| `RolModule` | Permiso CRUD por rol y mÃ³dulo | FK a `Modules` |
| `Otp` | CÃ³digo de un solo uso para autenticaciÃ³n | FK a `User` |
| `Cervezas` | CatÃ¡logo de cervezas (ejemplo CRUD) | â€” |

## DTOs

| DTO | Namespace | DescripciÃ³n |
|-----|-----------|-------------|
| `BaseDto` | `Template.Core.Dtos` | Base abstract: Id, CreatedAt, Responsable |
| `UserDto` | `Template.Core.Dtos` | FullName, Email, BornDate, Phone, IsActive, RefreshToken, Subscription, Roles |
| `UserModelDto` | `Template.Core.Dtos` | Simplified: FullName, Email, Subscription, UserName, Roles |
| `PostDto` | `Template.Core.Dtos` | UserId, Date, Description, Image |
| `CommentDto` | `Template.Core.Dtos` | Description, IsActive, PostId, UserId |
| `ModulesDto` | `Template.Core.Dtos` | ModuleName |
| `RolModuleDto` | `Template.Core.Dtos` | Module, Created, Edited, Listed, Deleted, Printed, Role |
| `RolModuleCombinadoDto` | `Template.Api.Core.Dtos` | Permisos combinados de todos los roles del usuario |
| `CervezasDto` | `Template.Core.Dtos` | Nombre, GradosAlcohol, Precio |
| `Login` | `Template.Core.Dtos` | Email, Otp |
| `TokenDto` | `Template.Api.Core.Dtos` | AccessToken, RefreshToken |
| `AuthenticatedResponse` | `Template.Api.Core.Dtos` | AuthToken, RefreshToken, ExpiresIn, UserId, UserName, Permisos |
| `OtpDto` | `Template.Core.Dtos` | UserId, Password, ExpireDate |
| `PasswordRecoveryDto` | `Template.Core.Dtos` | UserName, PasswordRecoveryToken, ExpiryDate, UserId |
| `PasswordUpdate` | `Template.Core.Dtos` | PasswordRecoveryToken, Password |

## Reglas de Negocio en Servicios

| Servicio | Regla |
|----------|-------|
| `PostService.InsertPost()` | Valida que el usuario exista; bloquea contenido con "sexo"; rate limit: 1 post cada 7 dÃ­as si tiene < 10 posts |
| `PostService.UpdatePost()` | Lanza `BusinessException` si el post no existe |
| `CommentService.Insert()` | Valida que el usuario y post existan; bloquea contenido con "sexo" |
| `UserService.InsertUser()` | Valida que el email no estÃ© vacÃ­o; lanza `BusinessException` si falta |
| `CervezasService` | Valida Nombre requerido, Precio > 0, GradosAlcohol entre 0 y 100 |
| `ModuleService.Insert()` | Valida ModuleName requerido |
| `SecurityService` | OTP se elimina post-uso (single-use); validaciÃ³n de credenciales |

## Validadores FluentValidation

| Validador | Reglas |
|-----------|--------|
| `PostValidators` | Description: not null, 10â€“500 chars; Date: not null, debe ser pasada |
| `CervezasValidators` | Nombre: not null, 1â€“100 chars; GradosAlcohol: 0â€“100; Precio: > 0 |

## Query Filters

| Filtro | Propiedades adicionales |
|--------|------------------------|
| `BaseQueryFilter` | `PageNumber`, `PageSize`, `Filter` (texto de bÃºsqueda general) |
| `UserQueryFilter` | `UserId`, `FullName`, `Email`, `BornDate`, `Phone`, `IsActive`, `Subscription` |
| `PostQueryFilter` | `UserId`, `Date`, `Description` |
| `CommentQueryFilter` | `UserId`, `Description` |
| `ModulesQueryFilter` | (solo paginaciÃ³n) |
| `RolModuleQueryFilter` | (solo paginaciÃ³n) |
| `CervezasQueryFilter` | `Nombre`, `GradosAlcoholMin`, `GradosAlcoholMax`, `PrecioMin`, `PrecioMax` |
| `GeneralQueryFilter` | Filtro dinÃ¡mico usado por `GenericService<T>.Get()` |

> `GeneralQueryFilter` es usado por `GenericService<T>` para aplicar bÃºsqueda dinÃ¡mica sobre todas las propiedades string de la entidad via `System.Linq.Dynamic.Core`.

## Agradecimientos

Gracias a @force4Win - Alvaro Andres Obregon por su colaboraciÃ³n y apoyo en este proyecto.
