DemoTodoApp

Add authentication and IAM service

Code: https://github.com/andtkach/TodoDemoApp
Demo: https://demo-todo-app-from-containers.azurewebsites.net/

## Build container for app
Show Dockerfile for App
cd \TodoDemoApp\todo-auth
docker build -t andreytkach/demo-todo-auth .
docker run --name todo-auth -p 3031:3031 -d andreytkach/demo-todo-auth
docker push andreytkach/demo-todo-auth

Kill infrastructure in Docker Desktop

## Crate two Azure app servies from containers
Create resource group: demo-todo-app-rg
Create app service for API: demo-todo-auth-from-containers
andreytkach/demo-todo-auth

If needed set ports in Congiguration of AppService
WEBSITES_PORT=8080
PORT=8080

