FROM node:6

COPY ./ /app
WORKDIR /app

RUN cd /app; npm install
CMD ["node","server.js"]