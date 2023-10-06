del /F /S /Q Migrations\
rmdir Migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
