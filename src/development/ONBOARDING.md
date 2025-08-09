# Development Folder Onboarding Guide

This `development` folder serves as the central hub for all core application code, testing suites, infrastructure definitions, and development-related scripts for the WhoAMI project (Yes, I am trying to showcase skills). This guide provides a quick overview of each subfolder and key files to help new developers quickly understand the project's structure and purpose without needing to dive into each individual project.

## Folder Structure Overview

*   ### `.devscripts/`
    This folder contains various shell scripts designed to assist developers with common tasks.
    *   `cleanup.sh`: A utility script for removing build artifacts and temporary files (like `bin`, `obj`, `.vs`, `node_modules`) to ensure a clean development environment.
    *   `runtests.sh`: A comprehensive script that orchestrates the execution of all test suites (Unit, T1, T2, T3). It handles the lifecycle of the Docker container for the T2 and T3 tests, including building, running, and tearing down the container.

*   ### `.devtools/`
    This folder is currently empty. A placeholder, potentially for future developer tools, configurations.

*   ### `.docker/`
    Contains the `Dockerfile` which defines how the `AccountAPI` application is containerized into a Docker image. This image is fundamental for running the application in isolated environments and is utilized by the T2 and T3 integration/performance tests, as well as for deployment.

*   ### `.terraform/`
    This folder holds `main.tf`, a Terraform configuration file. It's used for defining and provisioning the necessary Azure cloud infrastructure (such as a Resource Group, App Service Plan, and App Service) required to deploy and host the `AccountAPI`.

*   ### `.webapi/`
    This is the core project directory for the `AccountAPI` itself.
    *   It contains the main application source code, including:
        *   `Controllers/`: Defines the API endpoints and handles incoming HTTP requests.
        *   `Datastore/`: Manages data access logic and interactions with the underlying data storage.
        *   `Models/`: Defines the data structures and entities used throughout the application.
        *   `Services/`: Encapsulates the business logic and core functionalities of the API.
    *   Also includes the C# project file (`AccountAPI.csproj`), solution file (`AccountAPI.sln`), application entry point (`Program.cs`), and configuration files (`appsettings.json`).

*   ### `.webapi.tests.unit/`
    This directory is dedicated to isolated unit tests. These tests focus on verifying the correctness of individual code units (e.g., a single method or class) by mocking out all external dependencies.

*   ### `.webapi.tests.t1/`
    This directory contains "in-memory" integration tests. These tests validate the interactions between different components of the application (e.g., controller to service, service to data store) but use mocked or in-memory implementations for external dependencies, meaning they do not hit a live database or external API.

*   ### `.webapi.tests.t2/`
    This folder houses functional integration/regressiontests. These tests interact directly with the live `AccountAPI` running within a Docker container, verifying end-to-end functionality.

*   ### `.webapi.tests.t3/`
    Contains performance tests, specifically using the NBomber framework. These tests also interact with the live `AccountAPI` running in a Docker container to measure performance metrics like requests per second and response times.

## Key Files at the Root of `development/`

*   ### `azure-pipelines.yml`
    This file defines the Continuous Integration/Continuous Delivery (CI/CD) pipeline for the project using Azure DevOps. It automates the entire build, test, and static code analysis (SonarQube) process, triggered by code changes.

*   ### `./.github/workflows/dotnet-ci.yml`
    This file defines a GitHub Actions CI workflow. It automates the build and test process for the project, specifically:
    *   **Triggers**: Runs on pushes and pull requests to the `master` branch.
    *   **Jobs**: Contains a `test-in-container` job that runs on `ubuntu-latest`.
    *   **Steps**:
        *   Checks out the repository.
        *   Sets up .NET 8.0.
        *   Builds and runs the Docker image for the `AccountAPI`.
        *   Waits for the API to be ready.
        *   Runs Unit Tests, T1 In-Memory Integration Tests, T2 Integration Tests, and T3 Performance Tests.
        *   Stops and removes the Docker container, ensuring cleanup even if tests fail.
    This workflow provides continuous integration for the project within the GitHub ecosystem.

## Conclusion

This `development` folder is structured to provide a clear separation of concerns for application code, various testing methodologies, infrastructure as code, and CI/CD automation. Understanding this layout will significantly aid in navigating and contributing to the project.
