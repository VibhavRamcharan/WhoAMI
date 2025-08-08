# variables.tf - Windows VM

variable "location" {
  type        = string
  description = "The Azure region where the resources will be created."
  default     = "East US"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group."
  default     = "rg-vm-terraform-windows"
}

variable "vm_name" {
  type        = string
  description = "The name of the virtual machine."
  default     = "win-demo-vm"
}

variable "vm_size" {
  type        = string
  description = "The size of the virtual machine."
  default     = "Standard_B2ms"
}

variable "admin_username" {
  type        = string
  description = "The admin username for the virtual machine."
  default     = "azureuser"
}

variable "admin_password" {
  type        = string
  description = "The admin password for the virtual machine."
  sensitive   = true
}

variable "tags" {
  type        = map(string)
  description = "A map of tags to apply to the resources."
  default = {
    environment = "development"
    project     = "WhoAMI"
  }
}