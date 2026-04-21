FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["QuantityMeasurement.WebApi/QuantityMeasurement.WebApi.csproj", "QuantityMeasurement.WebApi/"]
COPY ["QuantityMeasurement.BusinessLayer/QuantityMeasurement.BusinessLayer.csproj", "QuantityMeasurement.BusinessLayer/"]
COPY ["QuantityMeasurement.Repository/QuantityMeasurement.Repository.csproj", "QuantityMeasurement.Repository/"]
COPY ["QuantityMeasurement.Model/QuantityMeasurement.Model.csproj", "QuantityMeasurement.Model/"]

RUN dotnet restore "QuantityMeasurement.WebApi/QuantityMeasurement.WebApi.csproj"

COPY . .

RUN dotnet publish "QuantityMeasurement.WebApi/QuantityMeasurement.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurement.WebApi.dll"]