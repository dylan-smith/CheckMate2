# Contributing to CheckMate2

Thank you for your interest in contributing! This document outlines our process for branch protection and CI requirements.

## Branch Protection

The `main` branch should be protected with the following rules (configured in GitHub repository settings under Settings → Branches → Branch protection rules):

- **No direct pushes** — All changes should go through a pull request.
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
