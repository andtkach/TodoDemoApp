DemoTodoApp

A very basic video on how to start developing and using containers and how to host containers in the cloud. 
We will create two applications using the infrastructure in Docker Desktop and then publish the images in Docker Hub and deploy them to Azure AppServices for free. A nice and basic introduction to the world of containers and clouds.

Code: https://github.com/andtkach/TodoDemoApp
Demo: https://demo-todo-app-from-containers.azurewebsites.net/

## Show env: No containers, no databases
Docker Desktop
https://www.elephantsql.com/
https://redis.com/
https://portal.azure.com/
https://hub.docker.com/

Remote Postgres database 
Remote Redis database 

Visual Studio Code

## Build api on local pc with databases in the Docker Desktop
Show api code in Visual Studio
Show local containes created by Docker Compose

## Build app on local pc with local api
Show app code in Visual Studio Code
check local web api connection in env file
npm start

Kill infrastructure in Docker Desktop

## Build container for api
Show Dockerfile for API
cd \TodoDemoApp\todo-api\Todo.Demo
docker build -f Tasks.Api/Dockerfile -t andreytkach/demo-todo-api . 
docker run --name todo-api -p 8080:8080 -d andreytkach/demo-todo-api

## Build container for app
Show Dockerfile for App
cd \TodoDemoApp\todo-app
docker build -t andreytkach/demo-todo-app .
docker run --name todo-app -p 8081:80 -d andreytkach/demo-todo-app


## Push all containers
docker login --username andreytkach
docker push andreytkach/demo-todo-api
docker push andreytkach/demo-todo-app

Kill infrastructure in Docker Desktop

## Crate two Azure app servies from containers
Create resource group: demo-todo-app-rg
Create app service for API: demo-todo-api-from-containers
andreytkach/demo-todo-api
Show swagger from Azure: https://demo-todo-api-from-containers.azurewebsites.net/

If needed set ports in Congiguration of AppService
WEBSITES_PORT=8080
PORT=8080


Create app service for App: demo-todo-app-from-containers
andreytkach/demo-todo-app
Show ui from Azure
https://demo-todo-app-from-containers.azurewebsites.net/


Rebuild App UI container with new URL and republish to Docket hub and restart app service
REACT_APP_TODOAPIURL
https://demo-todo-api-from-containers.azurewebsites.net/tasks/



Demo:
https://demo-todo-app-from-containers.azurewebsites.net/

Youtube
https://youtu.be/ZBWEcqqzj_c