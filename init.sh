#!/bin/sh

# Install dotnet-ef CLI tool
dotnet tool install --global dotnet-ef

# Ensure the tool path is in the PATH
export PATH="$PATH:/root/.dotnet/tools"

# Navigate to the project directory
cd /app

# Apply EF Core migrations
dotnet ef database update