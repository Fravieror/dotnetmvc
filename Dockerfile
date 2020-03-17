FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["efecty.web.csproj", "./"]
RUN dotnet restore "./efecty.web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "efecty.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "efecty.web.csproj" -c Release -o /app/publish
# RUN docker run -d --name redis -p 6379:6379 redis

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "efecty.web.dll"]
