# Copilot instructions for CheckMate2

## Project structure
- `backend/CheckMate2.Api` is the ASP.NET Core Web API.
- `backend/CheckMate2.Api.Tests` contains backend tests (xUnit).
- `frontend` is the React + TypeScript app (Vite).

## Working conventions
- Keep changes minimal and scoped to the requested issue.
- Reuse existing patterns and naming in nearby files.
- Avoid introducing new dependencies unless required.
- Do not edit unrelated files just to satisfy style preferences.

## Validation commands
- Backend:
  - `dotnet restore`
  - `dotnet build --no-restore`
  - `dotnet format CheckMate2.slnx --verify-no-changes --no-restore`
  - `dotnet test --no-build`
- Frontend:
  - `cd frontend && npm ci`
  - `cd frontend && npx tsc -b`
  - `cd frontend && npx vite build`
  - `cd frontend && npx eslint .`

## PR expectations
- Keep PRs small and focused.
- Ensure backend and frontend checks pass for affected areas.
- Whenever UI changes are involved, always take screenshots and attach them to the PR description showing the changes.
