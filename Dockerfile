FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORT=8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Employee/src/Employee.Host/Employee.Host.csproj"
RUN dotnet tool install --global dotnet-ef 
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build "./Employee/src/Employee.Host/Employee.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Employee/src/Employee.Host/Employee.Host.csproj" -c Release -o /app/out /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out .

ENTRYPOINT ["dotnet", "Employee.Host.dll"]