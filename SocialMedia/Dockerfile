# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SocialMedia.Api/SocialMedia.Api.csproj", "SocialMedia.Api/"]
RUN dotnet restore "SocialMedia.Api/SocialMedia.Api.csproj"
COPY . .
WORKDIR "/src/SocialMedia.Api"
RUN dotnet build "SocialMedia.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialMedia.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialMedia.Api.dll"]