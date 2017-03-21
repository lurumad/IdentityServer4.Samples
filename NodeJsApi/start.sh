#!/usr/bin/env bash

export HOST_IP=$(node $(pwd)/src/tools/ip.js)
env | grep HOST_IP
docker-compose kill && docker-compose up -d