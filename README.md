# Overview

A basic .NET API project created to explore and integrate key Azure services.
The goal is to **learn Azure by doing** — using real features like **Azure Service Bus**, **Application Insights**, **Azure Functions**, and more.

# Azure Service Bus

## Authentication

This section summarizes various ways to authenticate with **Azure Service Bus**.

### Connection String

* Simplest for quick setup
* Uses **Shared Access Signature (SAS)**
* **Not recommended for production**. If a SAS is leaked, it can be used by anyone who obtains it.

Resources:

* [Service Bus access with SAS (microsoft.com)](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-sas)

### Managed Identity

Managed identities are **Azure-created identities** for your resources that let them authenticate to **other Azure services securely, without storing secrets** like connection strings or client secrets.

* Picked up by `DefaultAzureCredential`
* **No hard-coded secrets**
* Preferred for production
* Important! **Managed Identity only works inside Azure**
    * It does **not** work for local development
    * For local development, use **Service Principal**, **Azure CLI login**, or **authenticate via Visual Studio**

Resources:

* [Service Bus access with Managed Identity (microsoft.com)](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity)

#### Types of Managed Identities

There are two types of Managed Identities:

| Type            | Description                                                         |
|-----------------|---------------------------------------------------------------------|
| System-assigned | One identity tied to a single Azure resource (auto-deleted with it) |
| User-assigned   | Standalone identity that can be reused across Azure resources       |

### Service Principal

A Service Principal is an **identity for an application** to securely access Azure resources.
Every Service Principal is based on an **App Registration** - register the app first.

Used for **local development**, **CI/CD**, or **external apps**.

Setup steps:

1. Register an app in Microsoft Entra ID
2. Create a Service Principal (automatically or manually)
3. Assign Azure RBAC roles to the Service Principal
3. Set environment variables: `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_CLIENT_SECRET`.

Resources:

* [Service Bus access with Service Principal (microsoft.com)](https://learn.microsoft.com/en-us/azure/service-bus-messaging/authenticate-application)

### Visual Studio login

* Authenticates using the signed-in user in Visual Studio
* Picked up by `DefaultAzureCredential`
* Useful for local development

Resources:

* [Azure authentication via Visual Studio (microsoft.com)](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet#authenticate-via-visual-studio)

### DefaultAzureCredential

`DefaultAzureCredential` is a built-in Azure SDK feature that automatically picks the best way to authenticate — whether you're working locally, in CI/CD, or running in Azure - all without changing your code.

* Automatically picks the best available credential depending on where your app is running
* One credential for all environments
* No secrets in code

It tries multiple credential types in order, including:

* Environment variables
* Managed Identity (only within Azure)
* Visual Studio, Azure CLI (for local development)

Resources:

* [DefaultAzureCredential (microsoft.com)](https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/credential-chains?tabs=dac#defaultazurecredential-overview)
