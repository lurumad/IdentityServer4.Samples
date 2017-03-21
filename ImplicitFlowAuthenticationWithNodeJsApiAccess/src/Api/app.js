'use strict';

const express = require('express');
const app = express();
const cors = require('cors');
const jwt = require('jsonwebtoken');
const oidcJwksVerify = require('express-oidc-jwks-verify');
const HOST_IP = process.env.HOST_IP;

app.use(cors());
app.use(oidcJwksVerify({ issuer: `http://${HOST_IP}:5000` }));
app.get('/identity', (req, res) => {
  const header = req.header('Authorization');
  const token = header.replace(/Bearer /, '');

  return res.status(200).send(jwt.decode(token));
});

app.listen(5004);
console.log('API listening on port 5004');
