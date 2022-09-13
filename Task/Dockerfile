FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5050
EXPOSE 5051

ENV ASPNETCORE_URLS=http://+:5050

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Task/TATask.csproj", "Task/"]
RUN dotnet restore "Task/TATask.csproj"
COPY . .
WORKDIR "/src/Task"
RUN dotnet build "TATask.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TATask.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TATask.dll"]
