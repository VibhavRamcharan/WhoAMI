## Repo cleanup
- **Cleanup repoisitory:** src/development/.devscripts/cleanup.sh
- **Cleanup docker:** src/development/.devscripts/cleanup_docker.sh

## Docker
- **Build:** docker build -t webapi-image -f src/development/.docker/Dockerfile .
- **Run** docker run -d -p 80:80 --name webapi-container webapi-image