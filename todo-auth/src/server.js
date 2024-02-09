// backend/index.js
const express = require("express");
const mongoose = require("mongoose");
const authRoutes = require("./routes/auth");
const infoRoutes = require("./routes/info");
const cors = require("cors"); // Import the CORS middleware
require("dotenv").config();

const app = express();
const PORT = 8080;

mongoose
  .connect(process.env.MONGODB_URI, {
    dbName: 'todo-auth-db',
  })
  .then(() => {
    console.log("Connected to MongoDB");
  })
  .catch((err) => {
    console.error("Error connecting to MongoDB", err);
  });

  var corsOptions = {
    credentials: true,
    origin: '*',
    optionsSuccessStatus: 200,
  }

app.use(cors(corsOptions));

app.use(express.json());
app.use("/info", infoRoutes)
app.use("/auth", authRoutes); 

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
