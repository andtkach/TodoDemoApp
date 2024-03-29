const express = require("express");
const jwt = require("jsonwebtoken");
const User = require("../models/user");
const router = express.Router();
const bcrypt = require("bcrypt");

router.post("/login", async (req, res) => {
  console.log('login called');
  const { username, password } = req.body;
  const user = await User.findOne({ username });

  if (!user) return res.status(400).send("Invalid username or password.");

  const validPassword = await bcrypt.compare(password, user.password);

  if (!validPassword)
    return res.status(400).send("Invalid username or password.");

  const token = jwt.sign({ 
    sub: user.id, 
    userId: user.id,
    username: user.username,
    aud: "todo-app",
    iss: "https://todo-app-iam",
  }, process.env.JWT_SECRET, {expiresIn: '30d'});

  res.send({ userId: user.id, token });
});

router.post("/register", async (req, res) => {
  console.log('register called');
  try {
    const { username, password } = req.body;

    const existingUser = await User.findOne({ username });
    if (existingUser) {
      return res.status(400).json({ error: "Username already exists." });
    }

    const salt = await bcrypt.genSalt(10);
    const hashedPassword = await bcrypt.hash(password, salt);

    const user = new User({
      username,
      password: hashedPassword,
    });

    const savedUser = await user.save();
    res.json({
      message: "User registered successfully",
      userId: savedUser._id,
    });
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: "Internal server error" });
  }
});

module.exports = router;
