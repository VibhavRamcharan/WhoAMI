#!/bin/bash

echo "Running tests for .webapi.tests.unit..."
dotnet test src/development/.webapi.tests.unit/AccountAPI.Tests.Unit.csproj --logger "console;verbosity=normal"

echo "\nRunning tests for .webapi.tests.t1..."
dotnet test src/development/.webapi.tests.t1/AccountAPI.Tests.csproj --logger "console;verbosity=normal"

echo "\nRunning tests for .webapi.tests.t2..."
dotnet test src/development/.webapi.tests.t2/AccountAPI.Tests.T2.csproj --logger "console;verbosity=normal"

echo "\nRunning performance tests for .webapi.tests.t3..."

echo "Starting AccountAPI for performance tests..."
dotnet run --project src/development/.webapi/AccountAPI.csproj &
API_PID=$!

# Wait for the API to start (you might need a more robust check here)
sleep 10

dotnet run --project src/development/.webapi.tests.t3/AccountAPI.Tests.T3.csproj

echo "Stopping AccountAPI..."
kill $API_PID
wait $API_PID 2>/dev/null

echo "Performance tests completed."