#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ToDoListAPI/ToDoListAPI.csproj", "ToDoListAPI/"]
RUN dotnet restore "ToDoListAPI/ToDoListAPI.csproj"
COPY . .
WORKDIR "/src/ToDoListAPI"
RUN dotnet build "ToDoListAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoListAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListAPI.dll"]