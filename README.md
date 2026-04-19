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

## Run Playwright E2E tests

The Playwright tests automatically start the backend and frontend servers, so no manual setup is required.

```bash
cd frontend
npm install
npx playwright install chromium --with-deps
VITE_API_BASE_URL=http://localhost:5269 npm run build
npx playwright test
```

To run tests with the interactive UI:
```bash
npx playwright test --ui
```

## Contributing

Use the provided templates when opening issues or pull requests:

- [Bug Report](https://github.com/dylan-smith/CheckMate2/issues/new?template=bug_report.md)
- [Feature Request](https://github.com/dylan-smith/CheckMate2/issues/new?template=feature_request.md)
- [Enhancement](https://github.com/dylan-smith/CheckMate2/issues/new?template=enhancement.md)

Pull requests should follow the [PR template](./.github/PULL_REQUEST_TEMPLATE.md) checklist before requesting review.
