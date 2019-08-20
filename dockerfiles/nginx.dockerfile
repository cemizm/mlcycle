FROM node:12 as build-stage

WORKDIR /app

COPY frontend/package.json frontend/package-lock.json ./

RUN npm install
RUN npm install -g @angular/cli@8.1.0

COPY frontend/. .

RUN ng build --prod --build-optimizer

FROM nginx:1.15
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build-stage /app/dist/frontend /usr/share/nginx/html
COPY dockerfiles/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]