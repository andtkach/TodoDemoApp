docker build -t andreytkach/demo-todo-auth .
docker run --name todo-auth -p 3031:8080 -d andreytkach/demo-todo-auth
docker push andreytkach/demo-todo-auth