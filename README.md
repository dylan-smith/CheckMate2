# CheckMate2

[![CI](https://github.com/dylan-smith/CheckMate2/actions/workflows/ci.yml/badge.svg)](https://github.com/dylan-smith/CheckMate2/actions/workflows/ci.yml)

A checklist management app with:
- **Backend:** ASP.NET Core Web API (.NET 10) + Entity Framework Core + SQL Server
- **Frontend:** React 19 + TypeScript (Vite)

## Features

- Create, edit, and delete checklists
- Enforces unique checklist names
- RESTful API with OpenAPI support

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS recommended)
- [npm](https://www.npmjs.com/)
- SQL Server (optional — an in-memory database is used by default in development)

## Local Development Setup

### Backend

```bash
cd backend/CheckMate2.Api
dotnet run
```

The API starts at `http://localhost:5269` by default. In development mode an in-memory database is used automatically (configured in `appsettings.Development.json`).

To use SQL Server for local development instead, set `UseInMemoryDatabase` to `false` and update the connection string in `backend/CheckMate2.Api/appsettings.Development.json`.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

The frontend dev server starts at `http://localhost:5173`. Set the `VITE_API_BASE_URL` environment variable if your backend runs on a different URL.

## Common Tasks

### Running Backend Tests

```bash
cd backend/CheckMate2.Api.Tests
dotnet test
```

### Linting the Frontend

```bash
cd frontend
npm run lint
```

### Building the Frontend for Production

```bash
cd frontend
npm run build
```

The output is written to `frontend/dist`.

### Previewing the Production Build

```bash
cd frontend
npm run preview
```

## Deployment

1. **Backend** — Publish the API with `dotnet publish -c Release` from `backend/CheckMate2.Api`. Deploy the output to any host that supports .NET 10 (Azure App Service, Docker, etc.). Configure the `ConnectionStrings:CheckMate2` setting to point to your production SQL Server instance and set `UseInMemoryDatabase` to `false`. If the frontend will be served from a different origin than the API, also configure `Cors:AllowedOrigins` to include the production frontend URL(s) so the browser can call the API.

2. **Frontend** — Run `npm run build` in `frontend` and serve the contents of `frontend/dist` with any static file host (Azure Static Web Apps, Nginx, etc.). Set `VITE_API_BASE_URL` to the production API URL before building. When using a different origin for the frontend, make sure the backend `Cors:AllowedOrigins` setting includes that frontend URL.

## Project Structure

```
CheckMate2/
├── backend/
│   ├── CheckMate2.Api/          # ASP.NET Core Web API
│   └── CheckMate2.Api.Tests/    # xUnit backend tests
├── frontend/                    # React + TypeScript (Vite)
├── CONTRIBUTING.md
├── SECURITY.md
├── LICENSE
└── README.md
```

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to get started.

## Security

To report a security vulnerability, see [SECURITY.md](SECURITY.md).

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
