FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/DigitalWalletAPI
RUN dotnet restore

# Install EF CLI tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# RUN dotnet ef migrations add InitialCreate
# RUN dotnet ef database update 

# \
#       --connection Server=sqlserver;Database=ZooDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=true;

RUN dotnet publish "DigitalWalletAPI.csproj" -c Release -r linux-x64 -o /app/publish --no-self-contained

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY .env /app/publish

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DigitalWalletAPI.dll"]
EXPOSE 80