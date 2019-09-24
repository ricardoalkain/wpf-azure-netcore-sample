FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/TTMS.Web.Api.Core/TTMS.Web.Api.Core.csproj", "src/TTMS.Web.Api.Core/"]
RUN dotnet restore "src/TTMS.Web.Api.Core/TTMS.Web.Api.Core.csproj"
COPY . .
WORKDIR "/src/src/TTMS.Web.Api.Core"
RUN dotnet build "TTMS.Web.Api.Core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TTMS.Web.Api.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTMS.Web.Api.Core.dll"]