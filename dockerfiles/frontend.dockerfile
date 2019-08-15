FROM node:12

WORKDIR /app

COPY package.json ./

RUN npm install
RUN npm install -g @angular/cli@8.1.0

EXPOSE 4200

CMD npm start

