FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY backend.csproj .
RUN dotnet restore "backend.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "backend.csproj" -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5215
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5215

ENTRYPOINT ["dotnet", "backend.dll"] 