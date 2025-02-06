#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
# Garantir que o usuário é root para instalar pacotes
USER root

# Instalar unzip e baixar o vsdbg para debug
RUN apt-get update && apt-get install -y unzip curl \
    && mkdir -p /vsdbg \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && chmod +x /vsdbg/vsdbg \
    && dotnet dev-certs https --clean 

# Voltar para o usuário original do contêiner (se necessário)
USER app
WORKDIR /src
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj", "src/Ambev.DeveloperEvaluation.WebApi/"]
COPY ["src/Ambev.DeveloperEvaluation.IoC/Ambev.DeveloperEvaluation.IoC.csproj", "src/Ambev.DeveloperEvaluation.IoC/"]
COPY ["src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj", "src/Ambev.DeveloperEvaluation.Domain/"]
COPY ["src/Ambev.DeveloperEvaluation.Common/Ambev.DeveloperEvaluation.Common.csproj", "src/Ambev.DeveloperEvaluation.Common/"]
COPY ["src/Ambev.DeveloperEvaluation.Application/Ambev.DeveloperEvaluation.Application.csproj", "src/Ambev.DeveloperEvaluation.Application/"]
COPY ["src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj", "src/Ambev.DeveloperEvaluation.ORM/"]
RUN dotnet restore "./src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Ambev.DeveloperEvaluation.WebApi"
RUN dotnet build "./Ambev.DeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /src/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Ambev.DeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /src/publish /p:UseAppHost=false

FROM base AS final
RUN dotnet dev-certs https 
WORKDIR /src
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.WebApi.dll", "--wait-for-debugger"]
