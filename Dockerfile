# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 8080
ENV DOTNET_RUNNING_IN_CONTAINER=true  \
    DOTNET_USE_POLLING_FILE_WATCHER=true  \
    ASPNETCORE_ENVIRONMENT="Development" \
    ASPNETCORE_URLS=http://+:8080  \
    ASPNETCORE_VERSION=8.1.0  \
    DOTNET_VERSION=8.0  \
    APP_USER=app  \
    APP_DIR=/app
 
# TLSv1.2 TO TLSv1
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf    

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . .
RUN dotnet restore "CadastroPessoas.Api/CadastroPessoas.Api.csproj"
RUN dotnet build   "CadastroPessoas.Api/CadastroPessoas.Api.csproj" --configuration Release  --no-restore -o /app/build
RUN dotnet publish "CadastroPessoas.Api/CadastroPessoas.Api.csproj" --configuration Release  --no-restore -o /app/publish
 
# final
FROM base AS final
WORKDIR /app
COPY --from=build    /app/publish .
ENTRYPOINT ["dotnet", "CadastroPessoas.Api.dll"]