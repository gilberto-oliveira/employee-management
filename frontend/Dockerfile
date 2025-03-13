FROM node:23-alpine AS build

WORKDIR /app

COPY ./frontend/app/package.json ./frontend/app/package-lock.json ./

RUN npm install

COPY ./frontend/app /app

RUN npm run build --prod --omit=dev

FROM nginx:stable-alpine

WORKDIR /usr/share/nginx/html

COPY --from=build /app/dist/app /usr/share/nginx/html

COPY ./frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]