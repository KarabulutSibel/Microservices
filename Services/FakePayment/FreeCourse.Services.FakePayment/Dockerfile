FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim as build
WORKDIR /app
EXPOSE 8080
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/FakePayment/FreeCourse.Services.FakePayment/*.csproj Services/FakePayment/FreeCourse.Services.FakePayment/
RUN dotnet restore Services/FakePayment/FreeCourse.Services.FakePayment/*.csproj
COPY . .
WORKDIR /app/Services/FakePayment/FreeCourse.Services.FakePayment
RUN dotnet publish -c Release -o /app/out
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FreeCourse.Services.FakePayment.dll"]