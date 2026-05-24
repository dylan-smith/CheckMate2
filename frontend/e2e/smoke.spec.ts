import { test, expect } from '@playwright/test'

test('smoke: app loads and displays the main heading', async ({ page }) => {
  await page.goto('/')

  await expect(page.getByRole('heading', { name: 'CheckMate2' })).toBeVisible()
})

test('smoke: can create, list, and delete a checklist', async ({ page }) => {
  const checklistName = `Smoke Test ${Date.now()}`

  await page.goto('/')
  await expect(page.getByRole('heading', { name: 'CheckMate2' })).toBeVisible()

  // Create a checklist
  await page.getByLabel('Checklist name').fill(checklistName)
  await page.getByRole('button', { name: 'Create checklist' }).click()

  // Verify it appears in the list
  await expect(page.getByText(checklistName)).toBeVisible()

  // Delete it to leave the app in a clean state
  await page
    .getByRole('listitem')
    .filter({ hasText: checklistName })
    .getByRole('button', { name: 'Delete' })
    .click()

  await expect(page.getByText(checklistName)).not.toBeVisible()
})
