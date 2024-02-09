Build image
### `docker build -t andreytkach/demo-todo-bff .`

docker run --name todo-bff -p 8091:80 -d andreytkach/demo-todo-bff

docker login --username andreytkach
docker push andreytkach/demo-todo-bff

