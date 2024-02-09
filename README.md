# TodoDemoApp

docker compose up

# In controller
docker exec -it todo-ctrl /bin/bash

curl http://todo-auth:8080/info
curl http://todo-api:8080/info

curl http://todo-bff:80/iam/info
curl http://todo-bff:80/data/info


curl -X 'GET' \
  'https://localhost:5001/info' \
  -H 'accept: */*'