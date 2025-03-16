# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only necessary files first for caching
COPY *.sln ./
COPY WealthSync/*.csproj WealthSync/
RUN dotnet restore WealthSync/WealthSync.csproj

# Copy the rest of the app
COPY . .
WORKDIR /src/WealthSync

# Build the application
RUN dotnet publish -c Release -o /app/out --no-restore

# Use the official runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables for .NET Core
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "WealthSync.dll"]
