FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 305

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
ADD . /src
#COPY UploaderParserApi.csproj UploaderParserApi/
#RUN dotnet restore UploaderParserApi.csproj

#COPY UploaderParserApi.csproj UploaderParserApi/
#COPY *.sln ./
#COPY ./RabbitMQ/*.csproj ./RabbitMQ/
#COPY ./UploaderParserApi/*.csproj ./UploaderParserApi/
#RUN dotnet restore UploaderParserApi.csproj
COPY . .
#WORKDIR /src/XmlToCSV/UploaderParserApi/UploaderParserApi.csproj
#COPY . .
#RUN dotnet build UploaderParserApi.csproj -c Release -o /app
#RUN dotnet restore /src/XmlToCSV/UploaderParserApi/UploaderParserApi.csproj
COPY UploaderParserApi.csproj UploaderParserApi/
RUN dotnet restore UploaderParserApi.csproj
RUN dotnet build UploaderParserApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish UploaderParserApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UploaderParserApi.dll"]
