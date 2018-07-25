FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
ADD . /src
#COPY UploaderParserApi/UploaderParserApi.csproj UploaderParserApi/
#RUN dotnet restore UploaderParserApi/UploaderParserApi.csproj

#COPY UploaderParserApi.csproj UploaderParserApi/
COPY *.sln ./
COPY ./RabbitMQ/RabbitMQ.csproj ./RabbitMQ/
COPY ./UploaderParserApi/UploaderParserApi.csproj ./UploaderParserApi/
#RUN dotnet restore UploaderParserApi.csproj
RUN dotnet restore ./UploaderParserApi/UploaderParserApi.csproj
#COPY . .
#WORKDIR /src/UploaderParserApi
RUN dotnet build ./UploaderParserApi/UploaderParserApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ./UploaderParserApi/UploaderParserApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UploaderParserApi.dll"]
