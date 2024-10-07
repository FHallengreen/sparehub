# Use the SDK image directly in the development stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /app

# Copy only the Server project file to restore dependencies
COPY Server/Server.csproj Server/
RUN dotnet restore Server/Server.csproj

# Copy the rest of the source code
COPY Server/ ./Server/

# Change to the Server directory
WORKDIR /app/Server

# Build the application
RUN dotnet build -c Release

# Expose the application port
EXPOSE 8080

# Use dotnet watch to run the application in development
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://*:8080"]
