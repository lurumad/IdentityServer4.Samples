FROM microsoft/dotnet:1.0.0-preview2-sdk

COPY ./src/IdentityServer/project.json /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/IdentityServer/ /app/

EXPOSE 5000
ENTRYPOINT ["dotnet", "run"]
