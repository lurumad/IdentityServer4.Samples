FROM microsoft/dotnet:1.1.0-sdk-projectjson

RUN apt-get update
RUN apt-get -qq update
RUN apt-get install -y nodejs npm
RUN update-alternatives --install /usr/bin/node node /usr/bin/nodejs 10
RUN npm install -g bower

COPY ./src/MvcClient/project.json /app/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/MvcClient/ /app/

RUN dotnet publish -c Debug -o out

EXPOSE 5002
ENTRYPOINT ["dotnet", "out/app.dll"]