FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
ADD . /src
#
#COPY XmlToCSV/XmlToCSV.csproj XmlToCSV/
COPY XmlToCSV.csproj XmlToCSV/
#RUN dotnet restore XmlToCSV/XmlToCSV.csproj
RUN dotnet restore XmlToCSV.csproj
COPY . .
WORKDIR /src/XmlToCSV
COPY . .
RUN dotnet build XmlToCSV.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish XmlToCSV.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "XmlToCSV.dll"]
