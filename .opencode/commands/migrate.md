---
description: Agrega y aplica una migraciÃ³n de EF Core
---

Agrega una nueva migraciÃ³n de EF Core y aplica la base de datos.

1. Ejecuta: `dotnet ef migrations add $ARGUMENTS --project src/Template.Infrastructure --startup-project src/Template.Api`
2. Si el paso 1 funciona, ejecuta: `dotnet ef database update --project src/Template.Infrastructure --startup-project src/Template.Api`
3. MuÃ©strame el resultado de ambos pasos.
