
Build image
### `docker build -f Tasks.Api/Dockerfile -t andreytkach/demo-todo-api .`

docker run --name todo-api -p 8080:8080 -d andreytkach/demo-todo-api

docker login --username andreytkach
docker push andreytkach/demo-todo-api