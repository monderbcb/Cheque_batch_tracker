#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cheque_batch_tracker/Cheque_batch_tracker.csproj", "Cheque_batch_tracker/"]
RUN dotnet restore "Cheque_batch_tracker/Cheque_batch_tracker.csproj"
COPY . .
WORKDIR "/src/Cheque_batch_tracker"
RUN dotnet build "Cheque_batch_tracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cheque_batch_tracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cheque_batch_tracker.dll"]