## Repo cleanup
- **Cleanup repoisitory:** src/development/.devscripts/cleanup.sh
- **Cleanup docker:** src/development/.devscripts/cleanup_docker.sh

## Docker
- **Build:** docker build -t webapi-image -f src/development/.docker/Dockerfile .
- **Run container:** docker run -d -p 80:80 --name webapi-container webapi-image
- **View running and stopped containers:**  docker ps -a
- **Stop container:** docker stop webapi-container
- **Remove container:** docker rm webapi-container
- **Call health endpoint** curl http://localhost:80/health

## Build
- **Build WebAPI:** dotnet build src/development/.webapi/AccountAPI.sln

## Testing & Run
- **Unit:** dotnet test src/development/.webapi.tests.unit/AccountAPI.Tests.Unit.csproj
- **T1:** dotnet test src/development/.webapi.tests.T1/AccountAPI.Tests.T1.csproj
- **T2:** dotnet test src/development/.webapi.tests.T2/AccountAPI.Tests.T2.csproj
- **T3:** dotnet run --project src/development/.webapi.tests.t3/AccountAPI.Tests.T3.csproj