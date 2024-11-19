# Use the official .NET 8 SDK image as the base image for build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Expose only the HTTP port
EXPOSE 5000

# Use the SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore any dependencies (via NuGet)
COPY ["DiceMiceAPI/DiceMiceAPI.csproj", "DiceMiceAPI/"]
RUN dotnet restore "DiceMiceAPI/DiceMiceAPI.csproj"

# Copy the rest of the app's files and build the app
COPY . .
WORKDIR "/src/DiceMiceAPI"
RUN dotnet build "DiceMiceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiceMiceAPI.csproj" -c Release -o /app/publish

# Final image setup to run the app
FROM base AS final
WORKDIR /app

# Copy the app from the publish step
COPY --from=publish /app/publish .

# Set the ASP.NET Core URL to match Render's internal port
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the app
ENTRYPOINT ["dotnet", "DiceMiceAPI.dll"]
