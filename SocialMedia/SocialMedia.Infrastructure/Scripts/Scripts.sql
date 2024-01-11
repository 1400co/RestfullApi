--Scaffold-DbContext "Server=.;Database=SocialMedia;Integrated Security = true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data

--Instalar EF
--dotnet tool install --global dotnet-ef
--dotnet tool update --global dotnet-ef


--Instalar EF design
--dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.5

--Crear migracion
--dotnet ef migrations add InitialCreate

--Ejecutar migraciones
--dotnet ef database update


--Docker scripts
