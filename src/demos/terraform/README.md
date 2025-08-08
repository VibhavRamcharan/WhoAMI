# Terraform Azure VM Modules

This repository contains Terraform modules for creating both Windows and Linux virtual machines on Microsoft Azure. These modules are designed to be reusable, customizable, and demonstrate best practices for Infrastructure as Code (IaC).

## 🚀 Project Structure

```
.src/terraform/
├── linux/
│   └── terraform-azure-vm/
│       ├── main.tf
│       ├── variables.tf
│       ├── terraform.tfvars
│       └── versions.tf
└── windows/
    └── terraform-azure-vm/
        ├── main.tf
        ├── variables.tf
        ├── terraform.tfvars
        └── versions.tf

```

- **`linux/`**: Contains the Terraform module for creating a Linux (Ubuntu 18.04-LTS) VM.
- **`windows/`**: Contains the Terraform module for creating a Windows Server 2019 Datacenter VM.

## 📄 File-by-File Breakdown

Here is a detailed explanation of each file and its purpose within the modules:

### `main.tf`

This is the heart of the module, where all the Azure resources are defined. It's responsible for creating the virtual machine and all its dependencies, including:

- **Resource Group:** A logical container for all the resources in the module.
- **Virtual Network (VNet):** An isolated network for the VM to communicate with other resources.
- **Subnet:** A sub-division of the VNet, where the VM's network interface is placed.
- **Public IP Address:** A static public IP address that allows you to connect to the VM from the internet.
- **Network Interface (NIC):** The network interface that connects the VM to the VNet.
- **Network Security Group (NSG):** A firewall that controls inbound and outbound traffic to the VM.
- **Virtual Machine:** The actual virtual machine, either Windows or Linux.

### `variables.tf`

This file defines all the input variables that can be used to customize the module. Each variable has a type, a description, and an optional default value. This allows you to easily change things like the VM size, location, and resource names without modifying the `main.tf` file.

### `terraform.tfvars`

This file is where you provide the actual values for the variables defined in `variables.tf`. This file is **not** checked into source control, as it may contain sensitive information like passwords. To use the modules, you will need to create your own `terraform.tfvars` file locally.

### `versions.tf`

This file specifies the required versions of Terraform and the Azure provider. This ensures that the module is used with compatible versions of the tools, which helps to prevent errors and ensure consistent behavior.

## ✨ Features & Advanced Concepts

These modules demonstrate a variety of advanced Terraform concepts, including:

- **Resource Tagging:** All resources are tagged with a consistent set of tags, making it easy to identify and manage resources.
- **Public IP Addresses:** A static public IP address is created for each VM, allowing for easy access.
- **Network Security Groups (NSGs):** An NSG is created for each VM, with rules to allow RDP (for Windows) or SSH (for Linux) access.
- **Outputs:** The public IP address and VM ID are exposed as outputs, making it easy to use this module with other Terraform configurations.
- **SSH Key Authentication (Linux):** The Linux module uses SSH key authentication for a more secure way to connect to the VM.

## 💻 Usage

To use these modules, you will need to have Terraform installed and configured with your Azure credentials.

1. **Clone the repository:**

   ```bash
   git clone https://github.com/vibhavramcahran/WhoAMI.git
   cd WhoAMI/src/terraform/<os-type>/terraform-azure-vm
   ```

2. **Create a `terraform.tfvars` file:**

   Create a `terraform.tfvars` file in the module directory and add the following content:

   ```hcl
   # For Windows
   admin_password = "YourSecurePassword"

   # For Linux, no extra variables are needed if you have an id_rsa.pub file in your ~/.ssh directory
   ```

3. **Initialize Terraform:**

   ```bash
   terraform init
   ```

4. **Plan the deployment:**

   ```bash
   terraform plan
   ```

5. **Apply the configuration:**

   ```bash
   terraform apply
   ```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a pull request with any improvements.