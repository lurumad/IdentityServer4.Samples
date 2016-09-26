FROM microsoft/dotnet:1.0.0-preview2-sdk

COPY ./src/AspNetCoreAuthentication/project.json /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/AspNetCoreAuthentication/ /app/

EXPOSE 3308
ENTRYPOINT ["dotnet", "run"]
