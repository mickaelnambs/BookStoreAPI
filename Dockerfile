FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "API/API.csproj"

COPY . .

RUN dotnet build "API/API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /Infrastructure/Data/SeedData

COPY --from=build /src/Infrastructure/Data/SeedData/books.json /Infrastructure/Data/SeedData/
COPY --from=build /src/Infrastructure/Data/SeedData/delivery.json /Infrastructure/Data/SeedData/

ENTRYPOINT ["dotnet", "API.dll"]