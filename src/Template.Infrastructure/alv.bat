del /F /S /Q Migrations\
rmdir Migrations
pause
dotnet ef migrations add InitialCreate
dotnet ef database update
