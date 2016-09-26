FROM microsoft/dotnet:1.0.0-preview2-sdk

COPY ./src/Api/project.json /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/Api/ /app/

EXPOSE 1773
ENTRYPOINT ["dotnet", "run"]
