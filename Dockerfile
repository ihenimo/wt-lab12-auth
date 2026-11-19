# Используем SDK для сборки (10.0)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем все остальное и публикуем
COPY . ./
RUN dotnet publish -c Release -o out

# Используем runtime для запуска (10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Render требует, чтобы приложение слушало порт 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# ВАЖНО: Проверьте название вашего .dll файла! 
# У вас в логах мелькало "wt-lab12-12auth-validationl-savostikova.csproj" (с двумя l)
ENTRYPOINT ["dotnet", "wt-lab12-12auth-validationl-savostikova.dll"]