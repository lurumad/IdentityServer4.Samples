FROM microsoft/dotnet:1.1.1-sdk

COPY ./src/Api/Api.csproj /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/Api/ /app/

RUN dotnet publish -c Debug -o out

EXPOSE 5001
ENTRYPOINT ["dotnet", "out/Api.dll"]