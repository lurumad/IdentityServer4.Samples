'use strict'

const express = require('express');
const app = express();

app.use('/', express.static(__dirname + '/static'));

app.get('/constants', (req, res) => {
  console.log(process.env.HOST_IP)
  res.send(`var HOST_IP = '${process.env.HOST_IP}'`)
})

app.listen(5005, () => {
  console.log('listening on port 5005')
});