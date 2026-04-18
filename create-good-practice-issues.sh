#!/usr/bin/env bash
set -euo pipefail

# This script creates GitHub Issues for all recommended good-practice items
# for the CheckMate2 project.
#
# Prerequisites: gh CLI authenticated (`gh auth login`)
# Usage: ./create-good-practice-issues.sh

REPO="dylan-smith/CheckMate2"

echo "Creating good-practice issues in $REPO ..."

###############################################################################
# 1 – CI Workflow
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add CI workflow for build, test, and lint" \
  --body "## Description
Set up a GitHub Actions CI workflow that runs on every push to \`main\` and on every pull request.

## Tasks
- [ ] Create \`.github/workflows/ci.yml\`
- [ ] Build the backend (\`dotnet build\`)
- [ ] Run backend unit tests (\`dotnet test\`) with code-coverage collection
- [ ] Install frontend dependencies (\`npm ci\`)
- [ ] Build the frontend (\`npm run build\`)
- [ ] Run frontend lint (\`npm run lint\`)
- [ ] Use .NET 10 and current Node LTS versions
- [ ] Cache NuGet packages and npm dependencies for faster runs

## Acceptance Criteria
- The workflow passes on the current codebase
- Both backend and frontend are validated in every PR"

###############################################################################
# 2 – CodeQL Security Scanning
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add CodeQL security scanning workflow" \
  --body "## Description
Enable GitHub CodeQL analysis to automatically detect security vulnerabilities and code quality issues.

## Tasks
- [ ] Create \`.github/workflows/codeql.yml\`
- [ ] Configure CodeQL for C# (backend) and JavaScript/TypeScript (frontend)
- [ ] Run on push to \`main\`, on pull requests, and on a weekly schedule
- [ ] Review and triage any initial findings

## Acceptance Criteria
- CodeQL runs on every PR and on a schedule
- Security tab in GitHub shows scan results
- No unaddressed critical or high severity findings"

###############################################################################
# 3 – Azure Deployment Pipeline
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add Azure deployment pipeline" \
  --body "## Description
Set up a GitHub Actions workflow to deploy the backend API and frontend to Azure on merge to \`main\`.

## Tasks
- [ ] Create \`.github/workflows/deploy.yml\`
- [ ] Deploy backend to Azure App Service (or Azure Container Apps)
- [ ] Deploy frontend to Azure Static Web Apps (or Azure Storage + CDN)
- [ ] Use environment secrets for Azure credentials
- [ ] Add staging and production environments with appropriate approval gates
- [ ] Configure environment-specific app settings (connection strings, CORS origins, etc.)

## Acceptance Criteria
- Merging to \`main\` triggers an automatic deployment
- Staging deployment happens first; production requires manual approval
- Application is accessible at the deployed URL"

###############################################################################
# 4 – Infrastructure as Code (Bicep)
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add Infrastructure as Code with Bicep" \
  --body "## Description
Define all Azure infrastructure using Bicep templates so environments are reproducible and version-controlled.

## Tasks
- [ ] Create \`infra/\` directory with Bicep modules
- [ ] Define App Service Plan and App Service for the backend API
- [ ] Define Azure Static Web App (or Storage + CDN) for the frontend
- [ ] Define Azure SQL Database
- [ ] Define Application Insights resource
- [ ] Add a GitHub Actions workflow (or step in deploy workflow) to deploy infrastructure
- [ ] Parameterize for dev/staging/production environments

## Acceptance Criteria
- Running \`az deployment\` with the Bicep templates provisions the entire environment
- Infrastructure changes are tracked in version control and reviewed via PR"

###############################################################################
# 5 – Playwright E2E Tests
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add Playwright end-to-end UI tests" \
  --body "## Description
Add Playwright-based end-to-end tests to validate critical user flows in the browser.

## Tasks
- [ ] Install Playwright and configure it in the frontend project
- [ ] Write E2E tests for core flows:
  - [ ] Create a checklist
  - [ ] Edit a checklist
  - [ ] Delete a checklist
  - [ ] Verify unique name enforcement
- [ ] Add a CI workflow (or job in existing CI) to run Playwright tests
- [ ] Upload test artifacts (screenshots, traces) on failure
- [ ] Consider running against a containerized or in-memory backend

## Acceptance Criteria
- Playwright tests run in CI on every PR
- Test failures produce screenshots and traces for debugging
- All core user flows are covered"

###############################################################################
# 6 – Frontend Unit Tests (Vitest)
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add frontend unit tests with Vitest and React Testing Library" \
  --body "## Description
Set up a frontend unit testing framework to test React components and utilities.

## Tasks
- [ ] Install Vitest, \`@testing-library/react\`, \`@testing-library/jest-dom\`, and \`jsdom\`
- [ ] Configure Vitest in \`vite.config.ts\`
- [ ] Add an \`npm run test\` script to \`package.json\`
- [ ] Write initial unit tests for existing components
- [ ] Integrate frontend tests into the CI workflow

## Acceptance Criteria
- \`npm run test\` runs all frontend unit tests
- Frontend tests run as part of the CI workflow
- Test coverage is reported"

###############################################################################
# 7 – Code Coverage Reporting
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add code coverage reporting" \
  --body "## Description
Track and report code coverage for both backend and frontend to maintain test quality.

## Tasks
- [ ] Configure \`coverlet\` (already a dependency) to generate coverage reports for backend tests
- [ ] Configure Vitest to generate coverage reports for frontend tests
- [ ] Upload coverage reports as CI artifacts
- [ ] Consider integrating with a coverage service (e.g. Codecov or Coveralls)
- [ ] Optionally add coverage badges to the README
- [ ] Consider setting minimum coverage thresholds

## Acceptance Criteria
- Coverage reports are generated in CI for both backend and frontend
- Coverage trends are visible over time
- Team is aware of current coverage levels"

###############################################################################
# 8 – Dependabot Configuration
###############################################################################
gh issue create --repo "$REPO" \
  --title "Configure Dependabot for automated dependency updates" \
  --body "## Description
Enable Dependabot to automatically create PRs for dependency updates across all ecosystems.

## Tasks
- [ ] Create \`.github/dependabot.yml\`
- [ ] Configure for NuGet (backend .NET dependencies)
- [ ] Configure for npm (frontend JavaScript/TypeScript dependencies)
- [ ] Configure for GitHub Actions (workflow action versions)
- [ ] Set appropriate update schedule (e.g. weekly)
- [ ] Configure grouping for minor/patch updates to reduce PR noise

## Acceptance Criteria
- Dependabot creates PRs for outdated dependencies automatically
- PRs are validated by CI before merge
- Update frequency is manageable for the team"

###############################################################################
# 9 – Branch Protection Rules
###############################################################################
gh issue create --repo "$REPO" \
  --title "Configure branch protection rules for main" \
  --body "## Description
Protect the \`main\` branch to enforce code quality and review standards.

## Tasks
- [ ] Require pull request reviews before merging (at least 1 approval)
- [ ] Require status checks to pass (CI workflow) before merging
- [ ] Require branches to be up to date before merging
- [ ] Require CodeQL checks to pass
- [ ] Prevent force pushes to \`main\`
- [ ] Prevent branch deletion

## Acceptance Criteria
- Direct pushes to \`main\` are blocked
- PRs require passing CI and at least one approval
- Force pushes are prevented"

###############################################################################
# 10 – PR Template
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add pull request template" \
  --body "## Description
Create a PR template to standardize pull request descriptions and ensure important information is included.

## Tasks
- [ ] Create \`.github/pull_request_template.md\`
- [ ] Include sections for: Description, Type of Change, How Has This Been Tested, Checklist
- [ ] Include checklist items for common PR requirements (tests added, docs updated, etc.)

## Acceptance Criteria
- New PRs automatically use the template
- Template encourages thorough PR descriptions"

###############################################################################
# 11 – Issue Templates
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add issue templates for bugs and feature requests" \
  --body "## Description
Create structured issue templates to make it easier to report bugs and request features.

## Tasks
- [ ] Create \`.github/ISSUE_TEMPLATE/bug_report.yml\` with structured form fields
- [ ] Create \`.github/ISSUE_TEMPLATE/feature_request.yml\` with structured form fields
- [ ] Create \`.github/ISSUE_TEMPLATE/config.yml\` to configure template chooser
- [ ] Include fields for reproduction steps, expected behavior, environment info (bugs)
- [ ] Include fields for problem statement, proposed solution, alternatives considered (features)

## Acceptance Criteria
- Creating a new issue presents a template chooser
- Bug reports and feature requests have consistent structure"

###############################################################################
# 12 – Docker Support
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add Docker support for local development and deployment" \
  --body "## Description
Containerize the application to simplify local development and enable container-based deployments.

## Tasks
- [ ] Create \`Dockerfile\` for the backend API
- [ ] Create \`Dockerfile\` for the frontend (multi-stage build with nginx)
- [ ] Create \`docker-compose.yml\` for local development (API + frontend + SQL Server)
- [ ] Add \`.dockerignore\` files
- [ ] Document Docker-based development workflow in README

## Acceptance Criteria
- \`docker compose up\` starts the full application stack locally
- Container images are production-ready (multi-stage builds, non-root user, etc.)
- Docker setup is documented in the README"

###############################################################################
# 13 – Application Logging and Monitoring
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add structured logging and application monitoring" \
  --body "## Description
Implement proper logging and monitoring to support production operations.

## Tasks
- [ ] Add structured logging with Serilog or the built-in .NET logging
- [ ] Integrate with Azure Application Insights for production monitoring
- [ ] Add request/response logging middleware
- [ ] Configure log levels for different environments (Development vs Production)
- [ ] Add health check endpoint (see separate issue)
- [ ] Set up alerts for critical errors

## Acceptance Criteria
- Application logs are structured (JSON) and include correlation IDs
- Application Insights collects telemetry in deployed environments
- Errors and performance issues are detectable through monitoring"

###############################################################################
# 14 – API Health Check Endpoint
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add API health check endpoint" \
  --body "## Description
Add a health check endpoint to support monitoring, load balancer probes, and deployment readiness checks.

## Tasks
- [ ] Add ASP.NET Core health checks (\`AddHealthChecks()\`)
- [ ] Add a SQL Server health check to verify database connectivity
- [ ] Map the health check endpoint at \`/health\` (or \`/healthz\`)
- [ ] Return detailed status in development, simplified status in production
- [ ] Use the health check endpoint in Azure App Service health probes

## Acceptance Criteria
- \`GET /health\` returns the health status of the API and its dependencies
- Health check is used by the deployment pipeline for readiness verification"

###############################################################################
# 15 – Database Migration Strategy
###############################################################################
gh issue create --repo "$REPO" \
  --title "Implement EF Core database migration strategy" \
  --body "## Description
Set up a proper database migration strategy using EF Core migrations instead of \`EnsureCreated()\`.

## Tasks
- [ ] Replace \`EnsureCreated()\` with EF Core migrations
- [ ] Create initial migration from current model
- [ ] Add migration step to the deployment pipeline
- [ ] Document the migration workflow (adding migrations, applying them locally)
- [ ] Consider adding a migration bundle for production deployments

## Acceptance Criteria
- Database schema changes are tracked as migrations in version control
- Migrations are applied automatically during deployment
- \`EnsureCreated()\` is removed from production code paths"

###############################################################################
# 16 – Security Headers and HTTPS
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add security headers and enforce HTTPS" \
  --body "## Description
Harden the application by adding security headers and ensuring HTTPS is properly enforced.

## Tasks
- [ ] Add security headers middleware (Content-Security-Policy, X-Content-Type-Options, X-Frame-Options, Strict-Transport-Security, etc.)
- [ ] Ensure HTTPS redirection is properly configured for production
- [ ] Configure CORS to only allow the specific frontend origin in production
- [ ] Review and restrict API endpoints as needed
- [ ] Consider adding rate limiting

## Acceptance Criteria
- Security headers are present in all API responses
- HTTPS is enforced in production
- CORS policy is restrictive and environment-specific"

###############################################################################
# 17 – Contributing Guide and Documentation
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add contributing guide and improve documentation" \
  --body "## Description
Improve project documentation to make it easy for new contributors to get started.

## Tasks
- [ ] Create \`CONTRIBUTING.md\` with development setup instructions
- [ ] Document the architecture (backend API, frontend, database)
- [ ] Add a section on coding standards and conventions
- [ ] Document how to run tests locally
- [ ] Add API documentation (Swagger/OpenAPI is already available in dev)
- [ ] Update \`README.md\` with badges (CI status, coverage, etc.)
- [ ] Document environment variables and configuration

## Acceptance Criteria
- A new developer can set up the project by following the documentation
- Architecture and conventions are documented
- README has status badges"

###############################################################################
# 18 – Accessibility Testing
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add accessibility (a11y) testing" \
  --body "## Description
Ensure the application is accessible to all users by integrating accessibility testing.

## Tasks
- [ ] Add axe-core integration for automated a11y testing
- [ ] Integrate a11y checks into Playwright E2E tests (or as separate tests)
- [ ] Review and fix common accessibility issues (ARIA labels, keyboard navigation, color contrast, etc.)
- [ ] Add a11y linting rules to ESLint (eslint-plugin-jsx-a11y)
- [ ] Run a11y checks in CI

## Acceptance Criteria
- Automated a11y checks run in CI
- No critical accessibility violations
- Application is keyboard navigable"

###############################################################################
# 19 – License File
###############################################################################
gh issue create --repo "$REPO" \
  --title "Add a LICENSE file" \
  --body "## Description
Add an appropriate open-source license file to the repository.

## Tasks
- [ ] Decide on the appropriate license (e.g. MIT, Apache 2.0, etc.)
- [ ] Add \`LICENSE\` file to the repository root
- [ ] Add license header or reference in \`README.md\`

## Acceptance Criteria
- A \`LICENSE\` file exists in the repository root
- License is referenced in the README"

###############################################################################
# 20 – Environment Configuration Management
###############################################################################
gh issue create --repo "$REPO" \
  --title "Improve environment and secrets management" \
  --body "## Description
Ensure configuration and secrets are properly managed across environments.

## Tasks
- [ ] Add \`.env.example\` files documenting required environment variables
- [ ] Ensure no secrets or connection strings are committed to source control
- [ ] Configure GitHub repository secrets for deployment
- [ ] Use Azure Key Vault or GitHub environment secrets for sensitive configuration
- [ ] Document the environment variable setup in the contributing guide

## Acceptance Criteria
- No secrets in source control
- Environment variables are documented
- Production secrets are managed securely (e.g. Azure Key Vault)"

echo ""
echo "✅ All issues created successfully!"
echo ""
echo "You can view them at: https://github.com/$REPO/issues"
