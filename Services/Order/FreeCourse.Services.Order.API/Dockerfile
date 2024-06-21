FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim as build
WORKDIR /app
EXPOSE 8080
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Order/FreeCourse.Services.Order.API/*.csproj Services/Order/FreeCourse.Services.Order.API/
COPY Services/Order/FreeCourse.Services.Order.Application/*.csproj Services/Order/FreeCourse.Services.Order.Application/
COPY Services/Order/FreeCourse.Services.Order.Domain/*.csproj Services/Order/FreeCourse.Services.Order.Domain/
COPY Services/Order/FreeCourse.Services.Order.Domain.Core/*.csproj Services/Order/FreeCourse.Services.Order.Domain.Core/
COPY Services/Order/FreeCourse.Services.Order.Infrastructure/*.csproj Services/Order/FreeCourse.Services.Order.Infrastructure/
RUN dotnet restore Services/Order/FreeCourse.Services.Order.API/*.csproj
COPY . .
WORKDIR /app/Services/Order/FreeCourse.Services.Order.API
RUN dotnet publish -c Release -o /app/out
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FreeCourse.Services.Order.API.dll"]