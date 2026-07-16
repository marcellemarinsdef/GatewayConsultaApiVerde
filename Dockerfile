# Esta fase é usada durante a execução
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER $APP_UID
WORKDIR /app

EXPOSE 3000


# Esta fase é usada para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["ApiConsultaProcesso.csproj", "./"]

RUN dotnet restore "./ApiConsultaProcesso.csproj"

COPY . .

RUN dotnet build "./ApiConsultaProcesso.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


# Esta fase publica a aplicação
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./ApiConsultaProcesso.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# Esta fase gera a imagem final
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ApiConsultaProcesso.dll"]