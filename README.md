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
npm install
npm run dev
```

Set `VITE_API_BASE_URL` if your backend URL is different.

## Contributing

Use the provided templates when opening issues or pull requests:

- [Bug Report](/.github/ISSUE_TEMPLATE/bug_report.md)
- [Feature Request](/.github/ISSUE_TEMPLATE/feature_request.md)
- [Enhancement](/.github/ISSUE_TEMPLATE/enhancement.md)

Pull requests should follow the [PR template](/.github/PULL_REQUEST_TEMPLATE.md) checklist before requesting review.
