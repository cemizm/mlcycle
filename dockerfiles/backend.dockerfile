FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS staging

WORKDIR /src

COPY backend/. .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS final
WORKDIR /app
COPY --from=staging /app .
CMD dotnet Backend.WebApi.dll