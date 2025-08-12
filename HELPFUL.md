## Repo cleanup
- **Cleanup repoisitory:** src/development/.devscripts/cleanup.sh
- **Cleanup docker:** src/development/.devscripts/cleanup_docker.sh

## Docker
- **Build:** docker build -t webapi-image -f src/development/.docker/Dockerfile .
- **Run Container:** docker run -d -p 80:80 --name webapi-container webapi-image

## Build
- **Build WebAPI:** dotnet build src/development/.webapi/AccountAPI.sln

## Testing | Run
- **Unit:** dotnet test src/development/.webapi.tests.unit/AccountAPI.Tests.Unit.csproj
- **T1:** dotnet test src/development/.webapi.tests.T1/AccountAPI.Tests.T1.csproj
- **T2:** dotnet test src/development/.webapi.tests.T2/AccountAPI.Tests.T2.csproj
- **T3:** dotnet run --project src/development/.webapi.tests.t3/AccountAPI.Tests.T3.csproj