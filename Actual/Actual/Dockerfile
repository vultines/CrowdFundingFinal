# Build stage for your ASP.NET Core application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Actual/Actual.csproj", "Actual/"]
RUN dotnet restore "Actual/Actual.csproj"
COPY . .
WORKDIR "/src/Actual"
RUN dotnet build "Actual.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Actual.csproj" -c Release -o /app/publish /p:UseAppHost=false

# SQL Server setup stage
FROM mcr.microsoft.com/mssql/server AS sqlserver
WORKDIR /usr/src/app
COPY init.sql .
ENV SA_PASSWORD=
ENV ACCEPT_EULA=Passwordhereyo1!
ENV DATABASE_NAME=CrowdFunding
RUN chmod +x ./init.sql
CMD /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i init.sql

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=sqlserver /usr/src/app /usr/src/app
ENTRYPOINT ["dotnet", "Actual.dll"]
