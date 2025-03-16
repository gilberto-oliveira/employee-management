FROM node:23-alpine AS build

WORKDIR /app

COPY ./frontend/angular/package.json ./frontend/angular/package-lock.json ./

RUN npm install

COPY ./frontend/angular /app

RUN npm run build --prod --omit=dev

FROM nginx:stable-alpine

WORKDIR /usr/share/nginx/html

COPY --from=build /app/dist/angular /usr/share/nginx/html

COPY ./frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]