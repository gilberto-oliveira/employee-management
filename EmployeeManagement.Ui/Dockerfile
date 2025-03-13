FROM node:23-alpine AS build

WORKDIR /app

COPY ./EmployeeManagement.Ui/app/package.json ./EmployeeManagement.Ui/app/package-lock.json ./

RUN npm install

COPY ./EmployeeManagement.Ui/app /app

RUN npm run build --prod --omit=dev

FROM nginx:stable-alpine

WORKDIR /usr/share/nginx/html

COPY --from=build /app/dist/app /usr/share/nginx/html

COPY ./EmployeeManagement.Ui/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]