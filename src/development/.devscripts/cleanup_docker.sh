#!/bin/bash
echo "Removing all Docker containers..."
docker ps -aq | xargs -r docker rm

echo "Removing all Docker images..."
docker images -aq | xargs -r docker rmi --force

echo "Docker images cleanup complete."