const express = require("express");
const router = express.Router();

router.get("/", async (req, res) => {
  console.log('info called');
  res.send({ api: 'todo-auth', version: '0.1' });
});

module.exports = router;
