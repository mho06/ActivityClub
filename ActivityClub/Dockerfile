# Use the official ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ActivityClub.csproj"
RUN dotnet publish "ActivityClub.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ActivityClub.dll"]
