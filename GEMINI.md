# Project Overview

This project is a .NET Core Web API for managing user accounts. It includes a comprehensive test suite, containerization with Docker, and CI/CD pipelines for Azure DevOps. The repository also contains Terraform scripts for provisioning the necessary infrastructure on Azure.

## Key Technologies

*   **.NET 8:** The core framework for the Web API.
*   **xUnit:** The testing framework for unit and integration tests.
*   **Docker:** For containerizing the Web API.
*   **Azure DevOps:** For CI/CD pipelines.
*   **Terraform:** For infrastructure as code.
*   **SonarQube:** For static code analysis.

# Building and Running

## Building the Project

To build the project, you can use the .NET CLI:

```bash
dotnet build src/development/.webapi/AccountAPI.sln
```

## Running the Project

To run the project locally, you can use the .NET CLI:

```bash
dotnet run --project src/development/.webapi/AccountAPI.csproj
```

The API will be available at `http://localhost:8080`.

## Running the Tests

To run the tests, you can use the .NET CLI:

```bash
dotnet test src/development/.webapi.tests.unit/AccountAPI.Tests.Unit.csproj
dotnet test src/development/.webapi.tests.t1/AccountAPI.Tests.csproj
dotnet test src/development/.webapi.tests.t2/AccountAPI.Tests.T2.csproj
```

# Development Conventions

## Coding Style

The project follows the standard C# coding conventions.

## Testing Practices

The project has multiple test projects:

*   `AccountAPI.Tests.Unit`: Contains unit tests for the API (located in `./.webapi.tests.unit`).
*   `AccountAPI.Tests.t1`: Contains in-memory integration tests for the API (located in `./.webapi.tests.t1`).
*   `AccountAPI.Tests.t2`: Contains virtualized integration tests for the API (located in `./.webapi.tests.t2`).

All new features should be accompanied by corresponding unit and integration tests.

## CI/CD

The project uses Azure DevOps for CI/CD. The pipeline is defined in the `azure-pipelines.yml` file. The pipeline builds the project, runs the tests, and pushes a Docker image to a container registry.

## Containerization

Docker integration for the `AccountAPI` is located in `./.docker`, enabling consistent deployment.

## Infrastructure as Code

Related Terraform configurations are located in `./.terraform` and demonstrate provisioning Azure VMs for hosting the WebAPI.

# Demonstration Projects

## Terraform Azure VM Modules

This section describes the Terraform modules for creating both Windows and Linux virtual machines on Microsoft Azure, located in `src/demos/terraform`.

### Project Structure

*   **`linux/terraform-azure-vm/`**: Contains the Terraform module for creating a Linux (Ubuntu 18.04-LTS) VM.
*   **`windows/terraform-azure-vm/`**: Contains the Terraform module for creating a Windows Server 2019 Datacenter VM.

### Key Files

*   **`main.tf`**: Defines all the Azure resources (Resource Group, VNet, Subnet, Public IP, NIC, NSG, Virtual Machine).
*   **`variables.tf`**: Defines input variables for customization (e.g., VM size, location).
*   **`terraform.tfvars`**: Provides actual values for variables (not checked into source control).
*   **`versions.tf`**: Specifies required versions of Terraform and the Azure provider.

### Features

These modules demonstrate:

*   Resource Tagging
*   Static Public IP Addresses
*   Network Security Groups (NSGs) with RDP/SSH rules
*   Outputs for Public IP and VM ID
*   SSH Key Authentication (Linux)
