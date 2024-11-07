# Restful API

Este proyecto es una API RESTful que utiliza .NET Core y EF Core. Incluye configuraciones avanzadas y mejoras para facilitar la implementación de prácticas sólidas en desarrollo web.

## Características

1. **Entity Framework (EF)**: Añadido para la gestión de datos.
2. **Fluent API Configuration**: Configuración detallada de modelos mediante Fluent API.
3. **Métodos GET**: Implementación de métodos para obtener todos los posts y un solo post.
4. **Inserción de Posts**: Método para insertar posts en la base de datos.
5. **Technical Debt en API**: La API utiliza la misma clase del repositorio.
6. **Refactorización para Eliminar Debt**: Se sustituyen objetos de repositorio por DTOs y se usa AutoMapper para mapear objetos.
7. **Filtrado**: Uso de decoradores en DTOs y middleware para aplicar filtros.
8. **Configuración de la Web API**: Incluye validaciones con Fluent Validation.
9. **Operaciones CRUD con EF Core**: Implementación de operaciones básicas CRUD.
10. **Lógica de Negocio y Patrón de Repositorio**: Añadidos a la solución para una estructura limpia.
11. **Unit of Work**: Implementación del patrón Unit of Work.
12. **Manejo de Excepciones**: Añadido un manejador de excepciones y algunos refactores.
13. **Filtrado y Tipos de Retorno**: Mejora en los datos filtrados y en los tipos de retorno de la API.
14. **Paginación en Consultas**: Añadida paginación para mejorar la eficiencia en consultas.
15. **Configuración Adicional**: Mejoras en la configuración general de la API.
16. **Documentación con Swagger**: Documentación detallada de la API usando Swagger.
17. **Seguridad con JWT**: Integración de JSON Web Tokens (JWT) para asegurar la API.
18. **Registro e Inicio de Sesión de Usuario**: Funcionalidad de registro e inicio de sesión.
19. **Hashing de Contraseñas**: Almacenamiento seguro de contraseñas con hashing.
20. **Refactor Final**: Limpieza de código y ajustes finales.
21. **JWT en Swagger**: Integración de JWT en Swagger para pruebas autenticadas.
22. **Inserciones y Actualizaciones en Listas**: Añadido soporte para operaciones masivas en el repositorio base.
23. **Incluye Relaciones en Get y GetById**: Soporte para relaciones en las operaciones de obtención.
24. **Cambio de Id de int a Guid**: Uso de Guid para identificadores únicos.
25. **Soporte para PostgreSQL**: Añadido soporte para PostgreSQL en EF Core.
26. **Token de Refresco**: Implementación de refresh tokens para sesiones extendidas.
27. **Serilog**: Añadido Serilog para el manejo avanzado de logs.
28. **Actualización a .NET Core 8**: Migración y actualización a la última versión de .NET Core.
29. **Historial de Cambios en Modelos**: Auditoría para todos los modelos.
30. **Autenticación en Swagger**: Autenticación habilitada en Swagger.
31. **Entidades de Autorización y Permisos**: Estructura de permisos y autenticación en la base de datos.
32. **CORS**: Configuración de CORS para mejorar la seguridad de acceso.
33. **Servicio Genérico**: Añadido un servicio genérico para reutilización.
34. **Repositorio Base Actualizado**: Mejoras en la implementación del repositorio base.

## Agradecimientos Especiales

Gracias a @force4Win - Alvaro Andres Obregon por su colaboración y apoyo en este proyecto.
