echo ---------------------------------------------------
echo Build ctrl

cd todo-ctrl
docker build -t andreytkach/demo-todo-ctrl .
cd ..

echo End build ctrl
echo ---------------------------------------------------


echo ---------------------------------------------------
echo Build auth

cd todo-auth
docker build -t andreytkach/demo-todo-auth .
cd ..

echo End build auth
echo ---------------------------------------------------

echo ---------------------------------------------------
echo Build api

cd todo-api
cd Todo.Demo

docker build -f Tasks.Api/Dockerfile -t andreytkach/demo-todo-api .
cd ..
cd ..

echo End build api
echo ---------------------------------------------------

echo ---------------------------------------------------
echo Build bff

cd todo-bff
docker build -t andreytkach/demo-todo-bff .
cd ..

echo End build bff
echo ---------------------------------------------------

echo ---------------------------------------------------
echo Build app

cd todo-app
docker build -t andreytkach/demo-todo-app .
cd ..

echo End build app
echo ---------------------------------------------------


echo DONE