FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["src/Esb.Core/Esb.Core.csproj", "src/Esb.Core/"]
COPY ["src/Esb.Router/Esb.Router.csproj", "src/Esb.Router/"]
RUN dotnet restore "src/Esb.Router/Esb.Router.csproj"

COPY src/Esb.Core/ src/Esb.Core/
COPY src/Esb.Router/ src/Esb.Router/
WORKDIR /src/src/Esb.Router
RUN dotnet publish "Esb.Router.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Esb.Router.dll"]
