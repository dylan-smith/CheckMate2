# CheckMate2

[![CI](https://github.com/dylan-smith/CheckMate2/actions/workflows/ci.yml/badge.svg)](https://github.com/dylan-smith/CheckMate2/actions/workflows/ci.yml)

A checklist management app with:
- **Backend:** ASP.NET Core Web API + EF Core + SQL Server
- **Frontend:** React + TypeScript (Vite)

## Features
- Create checklist
- Edit checklist
- Delete checklist
- Enforces unique checklist names

## Run backend
```bash
cd backend/CheckMate2.Api
dotnet run
```

API base URL (development): `http://localhost:5269`

## Run frontend
```bash
cd frontend
npm ci
npm run dev
```

Set `VITE_API_BASE_URL` if your backend URL is different.

## Azure Deployment

The project includes a GitHub Actions workflow (`.github/workflows/deploy.yml`) that deploys the backend and frontend to Azure on every push to `main`.

### Deployment Architecture

| Component | Azure Service | Endpoint |
|-----------|--------------|----------|
| Backend API | Azure App Service | `https://<AZURE_BACKEND_APP_NAME>.azurewebsites.net` |
| Frontend | Azure Static Web Apps | `https://<your-static-web-app>.azurestaticapps.net` |

### Required GitHub Secrets

| Secret | Description |
|--------|-------------|
| `AZURE_CLIENT_ID` | Azure service principal client ID (for OIDC login) |
| `AZURE_TENANT_ID` | Azure Active Directory tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |
| `AZURE_BACKEND_APP_NAME` | Name of the Azure App Service for the backend |
| `AZURE_BACKEND_URL` | Public URL of the backend API (e.g. `https://checkmate2-api.azurewebsites.net`) |
| `AZURE_SQL_CONNECTION_STRING` | SQL Server connection string for the deployed database |
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Deployment token for Azure Static Web Apps |

### Environment Promotion

The workflow supports environment promotion through GitHub Environments. Configure `production` and `staging` environments in your repository settings to enable approval gates and environment-specific secrets. Pushes to `main` deploy to the `production` environment; other triggers use `staging`.

## Contributing

Use the provided templates when opening issues or pull requests:

- [Bug Report](https://github.com/dylan-smith/CheckMate2/issues/new?template=bug_report.md)
- [Feature Request](https://github.com/dylan-smith/CheckMate2/issues/new?template=feature_request.md)
- [Enhancement](https://github.com/dylan-smith/CheckMate2/issues/new?template=enhancement.md)

Pull requests should follow the [PR template](./.github/PULL_REQUEST_TEMPLATE.md) checklist before requesting review.
