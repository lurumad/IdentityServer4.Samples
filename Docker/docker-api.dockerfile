FROM microsoft/dotnet:1.1.0-sdk-projectjson

COPY ./src/Api/project.json /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/Api/ /app/

RUN dotnet publish -c Debug -o out

EXPOSE 5001
ENTRYPOINT ["dotnet", "out/app.dll"]