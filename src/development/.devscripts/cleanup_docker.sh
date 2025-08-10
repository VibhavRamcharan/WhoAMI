#!/bin/bash
echo "Stopping all running Docker containers..."
docker stop $(docker ps -aq)

echo "Removing all Docker containers..."
docker rm $(docker ps -aq)

echo "Removing all Docker images..."
docker rmi $(docker images -aq)

echo "Docker images cleanup complete."
