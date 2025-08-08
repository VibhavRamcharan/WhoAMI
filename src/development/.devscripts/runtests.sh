#!/bin/bash

echo "Running tests for .webapi.tests.unit..."
dotnet test src/development/.webapi.tests.unit/AccountAPI.Tests.Unit.csproj --logger "console;verbosity=normal"

echo "\nRunning tests for .webapi.tests.t1..."
dotnet test src/development/.webapi.tests.t1/AccountAPI.Tests.csproj --logger "console;verbosity=normal"

echo "\nRunning tests for .webapi.tests.t2..."
dotnet test src/development/.webapi.tests.t2/AccountAPI.Tests.T2.csproj --logger "console;verbosity=normal"
