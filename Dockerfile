FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Установка шрифтов для кириллицы
RUN apt-get update && apt-get install -y fonts-dejavu-core
ENV LANG ru_RU.UTF-8

WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 10000
ENTRYPOINT ["dotnet", "wt-lab8-8razor-postgresql-savostikova.dll"]