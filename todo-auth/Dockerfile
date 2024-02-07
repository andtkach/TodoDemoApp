FROM node:21-alpine3.18

RUN mkdir -p /opt/app

WORKDIR /opt/app

EXPOSE 8080

COPY src/package.json src/package-lock.json /opt/app/

RUN npm install

COPY src/ .

CMD [ "npm", "run", "pm2"]
