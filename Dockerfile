# Stage 1: Build and publish inside Docker
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project and restore dependencies
COPY ./DigitalWalletAPI/*.csproj ./DigitalWalletAPI/
RUN dotnet restore ./DigitalWalletAPI/DigitalWalletAPI.csproj

# Copy everything and build
COPY . .
RUN dotnet publish ./DigitalWalletAPI/DigitalWalletAPI.csproj \
    -c Release -r linux-arm64 --self-contained false -o /app/publish

# Stage 2: Run in a minimal runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "DigitalWalletAPI.dll"]