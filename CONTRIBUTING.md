# Contributing to CheckMate2

Thank you for your interest in contributing! This document outlines our process for branch protection and CI requirements.

## Branch Protection
The `main` branch should be protected with the following rules:

> These rules must be configured in GitHub branch protection settings for the repository and may not be present in forks by default.

- **Pull request reviews required** — At least one approving review from a code owner is required before merging.
- **No direct pushes** — All changes must go through a pull request.
- **Status checks must pass** — The following CI checks should be required before merging:
  - **Backend Build & Test** — Backend build and test validation
  - **Frontend Build & Lint** — Frontend build and lint validation

## How to Contribute

1. Create a feature branch from `main`.
2. Make your changes in the appropriate directory (`backend/` or `frontend/`).
3. Ensure all CI checks pass locally before pushing.
4. Open a pull request targeting `main`.
5. Once all status checks pass, your PR can be merged.

## Running Checks Locally

### Backend

```bash
cd backend/CheckMate2.Api
dotnet build
dotnet test ../CheckMate2.Api.Tests
```

### Frontend

```bash
cd frontend
npm install
npm run lint
npm run build
```
