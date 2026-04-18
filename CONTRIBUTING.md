# Contributing to CheckMate2

Thank you for your interest in contributing! This document outlines our process for code reviews, branch protection, and CI requirements.

## Code Ownership

We use a [CODEOWNERS](/.github/CODEOWNERS) file to automatically assign reviewers based on the files changed in a pull request:

| Path | Description |
|------|-------------|
| `/backend/` | ASP.NET Core Web API, EF Core, and related code |
| `/frontend/` | React + TypeScript (Vite) frontend application |
| `/.github/` | GitHub Actions workflows and CI configuration |

When you open a pull request that touches files in these paths, the designated code owners will be automatically requested for review.

## Branch Protection

The `main` branch is protected with the following rules:

- **Pull request reviews required** — At least one approving review from a code owner is required before merging.
- **No direct pushes** — All changes must go through a pull request.
- **Status checks must pass** — The following CI checks are required before merging:
  - **Backend Build & Test** — Backend build and test validation
  - **Frontend Build & Lint** — Frontend build and lint validation

## How to Contribute

1. Fork the repository or create a feature branch from `main`.
2. Make your changes in the appropriate directory (`backend/` or `frontend/`).
3. Ensure all CI checks pass locally before pushing.
4. Open a pull request targeting `main`.
5. Address any review feedback from the assigned code owners.
6. Once approved and all status checks pass, your PR can be merged.

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
