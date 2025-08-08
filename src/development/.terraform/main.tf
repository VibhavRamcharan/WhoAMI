provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-account-webapp"
  location = "East US"
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name                = "plan-account-webapp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_app_service" "app_service" {
  name                = "app-account-webapp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id

  site_config {
    dotnet_framework_version = "v8.0"
    scm_type                 = "LocalGit"
  }

  app_settings = {
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "FUNCTIONS_WORKER_RUNTIME"          = "dotnet"
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "Custom"
    value = "Server=tcp:{your_db_server}.database.windows.net,1433;Initial Catalog={your_db_name};Persist Security Info=False;User ID={your_db_user};Password={your_db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
