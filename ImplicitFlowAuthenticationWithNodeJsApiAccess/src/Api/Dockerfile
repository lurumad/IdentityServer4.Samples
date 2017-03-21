FROM node:4

COPY ./package.json /app/
COPY ./app.js /app/
WORKDIR /app

RUN cd /app; npm install -g npm; npm install
CMD ["node","--debug=5858","app.js"]