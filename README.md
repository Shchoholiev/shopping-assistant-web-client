# shopping-assistant-web-client
Client web interface for a Shopping Assistant that utilizes Natural Language Processing (NLP) technology to interpret user queries for products and gifts. Users interact through a chat-style interface to communicate their shopping requirements.

## Table of Contents
- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)

## Features
- Chat-style interface for querying products and gifts using natural language.
- Personalized wishlists and cart management.
- Real-time chat message updates and suggestion handling.
- User authentication with email or phone number.
- User profile management and settings.
- Display of top deals and product cards with images, ratings, and prices.
- Integration with Amazon through affiliate links.
- Responsive frontend built with Blazor Server.
- Confirmation modals for destructive actions (e.g., deleting chats).
- Use of GraphQL for API communication.

## Stack
- C# & .NET 7.0 with Blazor Server for frontend.
- GraphQL.Client for API and GraphQL interactions.
- Newtonsoft.Json for JSON serialization/deserialization.
- Blazored.Modal for modal dialogs.
- CSS with Bootstrap 5 for styling.
- Natural Language Processing (NLP) support via backend (API).
- Azure DevOps / GitHub Actions for CI/CD and deployment.

## Installation

### Prerequisites
- .NET 7.0 SDK or later.
- A code editor or IDE compatible with Blazor, e.g., Visual Studio 2022 or VS Code with C# extension.
- Docker (optional) if using the provided devcontainer setup.
- Internet access for restoring NuGet packages.
- Access to the corresponding backend API (shopping-assistant-api).

### Setup Instructions
1. Clone this repository:
   ```bash
   git clone https://github.com/Shchoholiev/shopping-assistant-web-client.git
   cd shopping-assistant-web-client
   ```
2. Open the solution file `ShoppingAssistantWebClient.sln` in your IDE.
3. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```
4. Configure API URL in `appsettings.Development.json` or `appsettings.Production.json`.
5. Run the project:
   ```bash
   dotnet run --project ShoppingAssistantWebClient.Web
   ```
   or use your IDE's run/debug feature.
6. Access the client at `https://localhost:{port}` as indicated in the launch settings.
7. Use the chat interface to start interacting with the shopping assistant.

*Optional*:  
- Use the `.devcontainer/devcontainer.json` to run with VS Code Remote - Containers extension for a ready development environment.
- Use GitHub Actions workflows for automated build and deployment to Azure.

## Configuration

### API URL
Set the backend API URL in the respective environment configuration file:

- For development: `ShoppingAssistantWebClient.Web/appsettings.Development.json`
- For production: `ShoppingAssistantWebClient.Web/appsettings.Production.json`

Example:
```json
{
  "ApiUrl": "https://shopping-assistant-api.azurewebsites.net/"
}
```

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Set to `Development` or `Production` accordingly.
