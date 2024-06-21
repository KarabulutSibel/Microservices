FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim as build
WORKDIR /app
EXPOSE 8080
COPY Gateways/FreeCourse.Gateway/*.csproj Gateways/FreeCourse.Gateway/
RUN dotnet restore Gateways/FreeCourse.Gateway/*.csproj
COPY . .
WORKDIR /app/Gateways/FreeCourse.Gateway
RUN dotnet publish -c Release -o /app/out
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FreeCourse.Gateway.dll"]