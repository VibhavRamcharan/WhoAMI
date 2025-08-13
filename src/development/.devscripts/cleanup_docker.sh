#!/bin/bash
echo "Removing all Docker containers..."
docker stop $(docker ps -aq) && docker rm $(docker ps -aq)

echo "Removing all Docker images..."
docker rmi $(docker images -aq) --force

echo "Docker images cleanup complete."