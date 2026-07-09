---
description: Reglas de arquitectura, convenciones y patrones para RestfullApi (ASP.NET Core 10, Clean Architecture)
alwaysApply: true
---

# RestfullApi — Sistema de Reglas C#

Este documento transforma la arquitectura y convenciones del proyecto (ver `README.md`) en reglas accionables para el agente de Cursor. **Siempre consultar modelos existentes antes de usarlos; nunca inventar propiedades en entidades o DTOs.**

---

## 1. Arquitectura (Clean Architecture)

### Capas y dependencias

```
Template.Api          → Template.Core + Template.Infrastructure
Template.Infrastructure → Template.Core (solo)
Template.Core         → sin dependencias de infraestructura
```

| Capa | Proyecto | Responsabilidad |
|------|----------|-----------------|
| Presentación | `Template.Api` | Controllers, `ApiResponse<T>`, `Startup.cs`, `Program.cs` |
| Dominio/Aplicación | `Template.Core` | Entities, DTOs, Interfaces, Services, QueryFilters, Enumerations |
| Infraestructura | `Template.Infrastructure` | EF Core, Repositories, Validators, Mappings, Filters, Email, Hangfire |

### Reglas de ubicación

- **Nuevo controller** → `Template.Api/Controllers/`
- **Nueva entidad** → `Template.Core/Entities/` (heredar `BaseEntity`)
- **Nuevo DTO** → `Template.Core/Dtos/` (heredar `BaseDto` si aplica)
- **Nueva interfaz de servicio** → `Template.Core/Interfaces/`
- **Implementación de servicio de negocio** → `Template.Core/Services/` (heredar `GenericService<T>` cuando sea CRUD)
- **Repositorio** → `Template.Infrastructure/Repositories/`
- **Validador FluentValidation** → `Template.Infrastructure/Validators/`
- **Perfil AutoMapper** → `Template.Infrastructure/Mappings/AutoMapperProfile.cs`
- **Configuración Fluent API** → `Template.Infrastructure/Data/`
- **Registro DI** → `Template.Infrastructure/Extentions/ServiceCollectionExtention.cs` (mantener el nombre con typo)

### Prohibido

- Referencias circulares entre capas
- Lógica de negocio en controllers (solo orquestación, mapeo y respuesta HTTP)
- Acceso directo a `DbContext` desde controllers o servicios de Core
- Crear tablas dinámicas de roles (`RolesController`, `UserInRoles` fueron eliminados)

---

## 2. Stack obligatorio

| Área | Tecnología | Regla |
|------|------------|-------|
| Framework | ASP.NET Core 10, C# | No degradar versión sin decisión explícita |
| ORM | EF Core 10 + PostgreSQL (principal) | `AddDbContextsPostgress` activo en `Startup.cs` |
| Auth | JWT Bearer + Refresh Tokens + OTP | Rate limit 5 req/min en `otp` y `login` |
| Mapeo | AutoMapper | Perfiles auto-descubiertos; agregar en `Mappings/` |
| Validación | FluentValidation + auto-validación | Validadores en `Infrastructure/Validators/` |
| Logs | Serilog + `Serilog.Exceptions` | Archivo `logs/Template.txt` + consola |
| Jobs | Hangfire (PostgreSQL, `WorkerCount = 1`) | Emails async (OTP, bienvenida) |
| JSON | Newtonsoft.Json | `ReferenceLoopHandling.Ignore`, `NullValueHandling.Ignore` |
| Docs | Swagger multi-versión | v1 activa, rutas `/api/v1/` |

---

## 3. Convenciones C#

### Enumeraciones

- **Usar enums con valores enteros explícitos** (requerido del proyecto):

```csharp
public enum RoleType
{
    Administrator = 0,
    Consumer = 1
}
```

- Roles del sistema: solo `RoleType` (`Administrator`, `Consumer`) — almacenados como lista separada por comas en `User.Roles`
- Otros enums existentes: `Subscription` (`Free`, `Basic`), `PostType` (`Image`, `Video`, `Party`)
- Usar `FriendlyNameAttribute` para nombres amigables cuando aplique

### Entidades

- Todas heredan `BaseEntity`: `Id` (Guid), `CreatedAt`, `Responsable`
- **Leer la entidad antes de modificarla** — no agregar propiedades no definidas en el modelo
- IDs siempre `Guid`

### DTOs

- Heredar `BaseDto` cuando corresponda (`Id`, `CreatedAt`, `Responsable`)
- DTOs de API específicos van en namespace `Template.Api.Core.Dtos` (`AuthenticatedResponse`, `TokenDto`, `RolModuleCombinadoDto`)
- DTOs de dominio en `Template.Core.Dtos`
- **No asumir ni agregar propiedades** — leer el DTO existente primero

### Nomenclatura

- Controllers: `{Entity}Controller` (singular, ej. `UserController`)
- Servicios: `{Entity}Service` implementando `I{Entity}Service`
- Filtros: `{Entity}QueryFilter` heredando `BaseQueryFilter`
- Excepciones de negocio: `BusinessException` (retorna 400)

---

## 4. API REST

### Versionado y rutas

```csharp
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
[Authorize] // excepto endpoints anónimos explícitos con [AllowAnonymous]
public class UserController : ControllerBase
```

### Códigos HTTP (obligatorio)

| Método | Código | Uso |
|--------|--------|-----|
| `GET` | `200 OK` | Recurso o listado paginado |
| `POST` | `201 Created` | Recurso creado + `CreatedAtAction` con header `Location` |
| `PUT` | `204 No Content` | Actualización completa (idempotente) |
| `PATCH` | `204 No Content` | Actualización parcial con `JsonPatchDocument<T>` |
| `DELETE` | `204 No Content` | Eliminación (idempotente) |
| Error negocio | `400 Bad Request` | `BusinessException` → `ProblemDetails` RFC 7807 |
| Sin auth | `401 Unauthorized` | |
| No existe | `404 Not Found` | |

### Respuestas

- Envolver datos en `ApiResponse<T>` con `Data`, `Meta` y `Links`
- Paginación: incluir `Metadata` en body y header `X-Pagination`
- HATEOAS: enlaces `self`, `create`, `update`, `delete`, `collection` (recursos individuales); `next`/`previous` en colecciones
- Usar `IUriService` para construir URLs de paginación
- Errores: `application/problem+json` vía `GlobalExceptionFilter`

### PATCH

- Disponible en `User` y `Post` como referencia
- Endpoint: `PATCH /api/v1/{resource}/{id}` con `JsonPatchDocument<TDto>`

---

## 5. Controllers — patrón a seguir

```csharp
// 1. Inyectar servicio + IMapper + IUriService (y otros según necesidad)
// 2. Llamar servicio de Core
// 3. Mapear entidad → DTO con AutoMapper
// 4. Construir ApiResponse<T> con Metadata y Links
// 5. Retornar IActionResult con código HTTP correcto
```

- Decorar con `[ProducesResponseType]` para Swagger
- `[HttpGet(Name = nameof(GetUsers))]` para `CreatedAtAction`
- Endpoints anónimos: `[AllowAnonymous]` + rate limiting en auth (`TokenController`)
- No reimplementar lógica que ya existe en servicios

### Controllers activos

`User`, `Post`, `Token`, `Modules`, `RolModule`, `Cervezas`

### Pendientes / removidos (no recrear sin requerimiento)

`Roles`, `UserInRoles`, `Comment`, `PasswordRecovery` — servicios `CommentService` y `PasswordRecoveryService` existen en Core sin controller

---

## 6. Servicios

### GenericService\<T\>

- CRUD base: `Insert`, `Update`, `Delete`, `Get`, `GetAll`, `Get` paginado
- Filtro dinámico en propiedades `string` vía `System.Linq.Dynamic.Core` + `GeneralQueryFilter`
- Servicios específicos heredan y delegan con `base.Method()`

### Servicios existentes

`UserService`, `PostService`, `CommentService`, `ModuleService`, `RolModuleService`, `CervezasService`, `SecurityService`, `TokenService`

### Reglas de negocio (implementar en servicio, no en controller)

| Servicio | Regla |
|----------|-------|
| `PostService.InsertPost()` | Usuario debe existir; bloquear "sexo"; máx. 1 post/7 días si tiene < 10 posts |
| `PostService.UpdatePost()` | `BusinessException` si post no existe |
| `CommentService.Insert()` | Usuario y post deben existir; bloquear "sexo" |
| `UserService.InsertUser()` | Email requerido |
| `CervezasService` | Nombre requerido; Precio > 0; GradosAlcohol 0–100 |
| `ModuleService.Insert()` | ModuleName requerido |
| `SecurityService` | OTP single-use (eliminar tras validación) |

### DI Scopes (no duplicar registros)

| Tipo | Scope |
|------|-------|
| `IRepository<T>`, `IGenericService<T>` | Scoped |
| `IUnitOfWork`, servicios de aplicación | Transient |
| `IPasswordService`, `IUriService` | Singleton |

> `IRepository<>` e `IUnitOfWork` ya tienen registros duplicados en `AddServices()` — no agregar un tercero.

---

## 7. Repositorio y Unit of Work

- **Solo `UnitOfWork` llama `SaveChanges`** — repositorios no persisten solos
- Usar `BaseRepository<T>` para CRUD genérico
- Lecturas sin tracking: `GetAsNoTracking()`, `GetByIdAsNoTracking()`
- Bulk: `Insert(List<T>)`, `Update(List<T>)`, `AddRangeAsync()`, `UpdateRangeAsync()`
- Sin commit: `AddAsync()`, `UpdateRangeAsync()` para batch con UnitOfWork
- `Detach(T entity)` para desvincular del contexto
- `PostRepository`: método extra `GetPostsByUser()`

---

## 8. Autenticación y seguridad

- Login OTP: solicitar código por email → canjear por JWT + Refresh Token
- OTP: `RandomNumberGenerator.GetInt32()` (nunca `System.Random`)
- OTP single-use: eliminar de BD tras validación
- JWT secret: env var `JWT_SECRET_KEY` (mín. 32 chars) con fallback a `appsettings.json`
- Refresh tokens: 32 bytes criptográficos, expiración 2 días, rotación en `/api/v1/Token/renew`
- Passwords: PBKDF2 via `PasswordService`
- Claims: `ClaimTypes.Name` (email), `ClaimTypes.NameIdentifier` (userId), `ClaimTypes.Role`
- `SessionService`: leer usuario/roles desde JWT claims (sin roundtrip a BD)
- `AuthenticatedResponse`: incluye `AuthToken`, `RefreshToken`, `UserId`, `UserName`, `Permisos`

---

## 9. Roles y permisos

- Roles via enum `RoleType` — **no crear tablas de roles**
- Permisos granulares: entidad `RolModule` (CRUD: `Created`, `Edited`, `Listed`, `Deleted`, `Printed`) por `RoleType` + `Module`
- `Administrator` obtiene permisos totales automáticamente
- `RolModuleService.ObtenerModulosUsuario()` combina permisos de múltiples roles → `RolModuleCombinadoDto`
- Login retorna `Permisos` con módulos y permisos CRUD del usuario

---

## 10. Datos y EF Core

- PostgreSQL principal (`Npgsql`); SQL Server disponible en `AddDbContextsSqlServer()` (no activo por defecto)
- `AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)` en `Program.cs` — **requerido**
- Fluent API con `ApplyConfigurationsFromAssembly`
- DbSets: `Users`, `Posts`, `Comments`, `RolModule`, `Otp`, `Cervezas`, `AuditLogs`
- Lazy loading habilitado (`Microsoft.EntityFrameworkCore.Proxies`)
- Auditoría automática en `SaveChanges` → tabla `AuditLogs` (`AuditEntry` helper)
- Migraciones: `Template.Infrastructure/Migrations/`

### Comandos EF

```powershell
dotnet ef migrations add MigrationName --project .\src\Template.Infrastructure --startup-project .\src\Template.Api
dotnet ef database update --project .\src\Template.Infrastructure --startup-project .\src\Template.Api
```

---

## 11. Validación

### FluentValidation (Infrastructure/Validators/)

| Validador | Reglas |
|-----------|--------|
| `PostValidators` | Description: not null, 10–500 chars; Date: not null, pasada |
| `CervezasValidators` | Nombre: not null, 1–100 chars; GradosAlcohol: 0–100; Precio: > 0 |

- Nuevos validadores: crear en `Validators/`, auto-descubiertos por `AddValidatorsFromAssemblies`
- Validación de entrada HTTP: FluentValidation; reglas de negocio complejas: servicios con `BusinessException`

---

## 12. Query Filters y paginación

- Todos los filtros heredan `BaseQueryFilter`: `PageNumber`, `PageSize`, `Filter`
- Filtros específicos: `UserQueryFilter`, `PostQueryFilter`, `CommentQueryFilter`, `ModulesQueryFilter`, `RolModuleQueryFilter`, `CervezasQueryFilter`
- `GeneralQueryFilter`: búsqueda dinámica en `GenericService<T>.Get()`
- `Metadata`: `TotalCount`, `TotalPages`, `HasNextPage`, `HasPreviousPage`, `NextPageUrl`, `PreviousPageUrl`

---

## 13. Infraestructura transversal

### Middleware (orden en `Startup.cs`)

1. `UseDeveloperExceptionPage` (dev)
2. `UseSerilogRequestLogging`
3. `UseHttpsRedirection`
4. `UseSwagger` + `UseSwaggerUI`
5. `UseCors("AllowAllOrigins")`
6. `UseRateLimiter`
7. `UseHangfireDashboard` (`/hangfire`)
8. `UseRouting` → `UseAuthentication` → `UseAuthorization` → `MapControllers`

### Servicios de infraestructura

| Servicio | Uso |
|----------|-----|
| `EmailService` | SMTP MailKit (OTP, bienvenida via Hangfire) |
| `PasswordService` | PBKDF2 hash/check |
| `SessionService` | Claims JWT |
| `UriService` | URLs HATEOAS |
| `TokenService` | JWT + Refresh Tokens |

---

## 14. Testing y comandos

```powershell
dotnet build .\src\Template.sln
dotnet run --project .\src\Template.Api
dotnet test .\src\Template.IntegrationTests\Template.IntegrationTests.csproj
dotnet test .\src\Template.UnitTests\Template.UnitTests.csproj
docker build -t Template-api -f Template/Dockerfile .
```

- Tests de integración: xUnit, net10.0
- Al agregar tests: seguir estructura existente en `Template.IntegrationTests` y `Template.UnitTests`

---

## 15. Checklist al agregar un recurso CRUD

1. Entidad en `Core/Entities/` (heredar `BaseEntity`)
2. DTO en `Core/Dtos/` (heredar `BaseDto`)
3. Interfaz `I{Entity}Service` en `Core/Interfaces/`
4. Servicio en `Core/Services/` (heredar `GenericService<T>` si aplica)
5. `{Entity}QueryFilter` en `Core/QueryFilters/`
6. Configuración Fluent API + DbSet en `Infrastructure/Data/`
7. Validador FluentValidation si hay reglas de entrada
8. Mapping en `AutoMapperProfile.cs`
9. Registro DI en `ServiceCollectionExtention.AddServices()`
10. Controller en `Api/Controllers/` con `ApiResponse<T>`, paginación, HATEOAS y códigos HTTP correctos
11. Migración EF si hay cambios de esquema
12. Swagger `[ProducesResponseType]` documentado

---

## 16. MCP y documentación externa

- **microsoft_learn**: documentación oficial .NET, ASP.NET Core, EF Core, Azure
- **context7**: docs actualizadas de librerías de terceros
- **sequential-thinking**: decisiones de arquitectura y debugging complejo

No usar MCP para lógica de negocio del proyecto — consultar `README.md`, `knowledge-base/` y código fuente.

---

## 17. Referencias del proyecto

| Recurso | Ubicación |
|---------|-----------|
| Documentación completa | `README.md` |
| Convenciones de agente | `.opencode/rules/AGENTS.md` |
| Metodología IA / trazabilidad | `.opencode/rules/AI_PROJECT_RULES.md` |
| Base de conocimiento | `knowledge-base/` |
| Config OpenCode | `.opencode/opencode.json` |
