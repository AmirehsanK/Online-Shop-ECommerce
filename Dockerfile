# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Project files first, so the restore layer is reused until a dependency changes.
COPY global.json ./
COPY Domain/Domain.csproj Domain/
COPY Infra.Data/Infra.Data.csproj Infra.Data/
COPY Application/Application.csproj Application/
COPY IOC/IOC.csproj IOC/
COPY Web/Web.csproj Web/
RUN dotnet restore Web/Web.csproj

COPY Domain/ Domain/
COPY Infra.Data/ Infra.Data/
COPY Application/ Application/
COPY IOC/ IOC/
COPY Web/ Web/
RUN dotnet publish Web/Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Admin uploads (products, banners, galleries) are written under wwwroot/images, so
# that folder - and only that folder - belongs to the non-root user the site runs as.
RUN chown -R $APP_UID /app/wwwroot/images
USER $APP_UID

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Web.dll"]
