FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Blazor.App/Blazor.App.csproj", "Blazor.App/"]
RUN dotnet restore "src/Blazor.App/Blazor.App.csproj"
COPY . .
WORKDIR "/src/Blazor.App"
RUN dotnet build "Blazor.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blazor.App.dll"]
