# # Stage 1: Build the Angular app
# FROM node:21-alpine as build
# WORKDIR /app
# COPY . .
# RUN npm install
# RUN npm run build --prod

# # Stage 2: Serve the Angular app using NGINX
# FROM nginx:1.25.5-alpine
# COPY --from=build /app/dist/cheesy-mart /usr/share/nginx/html
# #copying nginx config from local to image
# COPY /nginx.conf /etc/nginx/conf.d/default.con
# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]

# STEP-1 BUILD
# Defining node image and giving alias as node-helper
# It's better to define version otherwise me might face issue in future build

FROM node:21-alpine as node-helper

#Accepting build-arg to create environment specific build
#it is useful when we have multiple environment (e.g: dev, tst, staging, prod)
#default value is development
ARG build_env=development

#Creating virtual directory inside docker image
WORKDIR /app

RUN npm cache clean --force

#Copying file from local machine to virtual docker image directory
COPY . .

#installing deps for project
RUN npm install

#creating angular build
RUN npm run build --configuration=$build_env

#STEP-2 RUN
#Defining nginx img 
FROM nginx:1.25.5-alpine as ngx
#exposing internal port
EXPOSE 80

#copying nginx config from local to image
COPY /nginx.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*  

#copying compiled code from dist to nginx folder for serving
COPY --from=node-helper /app/dist/cheesy-mart/browser /usr/share/nginx/html

