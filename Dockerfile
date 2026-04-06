# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore StudentManagement.sln

# Build the project
RUN dotnet build StudentManagement.sln -c Release -o /app/build

# Publish the project
RUN dotnet publish StudentManagement.API/StudentManagement.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 5000 5001

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001

# Run the application
ENTRYPOINT ["dotnet", "StudentManagement.API.dll"]
