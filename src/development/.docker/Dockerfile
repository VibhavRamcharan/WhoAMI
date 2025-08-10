# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY ../.. .

# Restore, build, and publish
WORKDIR /src/src/development/.webapi
RUN dotnet restore "AccountAPI.sln"
RUN dotnet build "AccountAPI.csproj" -c Release -o /app/build
RUN dotnet publish "AccountAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET ASP.NET runtime image as the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Set the entry point for the application
ENTRYPOINT ["dotnet", "AccountAPI.dll"]
