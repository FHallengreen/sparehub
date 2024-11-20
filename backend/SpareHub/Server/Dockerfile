FROM mcr.microsoft.com/dotnet/sdk:8.0.402

WORKDIR /app/Server

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet restore && dotnet watch run --urls http://*:8080"]
