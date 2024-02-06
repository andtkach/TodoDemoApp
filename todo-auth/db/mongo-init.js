db = db.getSiblingDB('todo-auth-db');

db.createUser(
  {
      username: "admin",
      password: "password",
      tokens: [
          {
              token: "token"
          }
      ]
  }
)

db.createCollection('TestUser');
