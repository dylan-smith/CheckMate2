import { defineConfig } from '@playwright/test'

export default defineConfig({
  testDir: './e2e',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: 1,
  reporter: process.env.CI ? [['github'], ['html']] : 'list',
  use: {
    baseURL: 'http://localhost:4173',
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: { browserName: 'chromium' },
    },
  ],
  webServer: [
    {
      command: 'dotnet run --no-build --project ../backend/CheckMate2.Api',
      url: 'http://localhost:5269/api/checklists',
      reuseExistingServer: !process.env.CI,
      timeout: 120000,
      env: {
        ASPNETCORE_URLS: 'http://localhost:5269',
        ASPNETCORE_ENVIRONMENT: 'Development',
      },
    },
    {
      command: 'npm run build && npm run preview -- --port 4173 --strictPort',
      url: 'http://localhost:4173',
      reuseExistingServer: !process.env.CI,
      timeout: 60000,
    },
  ],
})
