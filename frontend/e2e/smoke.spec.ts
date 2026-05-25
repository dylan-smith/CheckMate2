import { test, expect } from '@playwright/test'

test('smoke: app loads and displays the main heading', async ({ page }) => {
  await page.goto('/')

  await expect(page.getByRole('heading', { name: 'CheckMate' })).toBeVisible()
})

test('smoke: can create, list, and delete a checklist', async ({ page }) => {
  const checklistName = `Smoke Test ${Date.now()}`

  await page.goto('/')
  await expect(page.getByRole('heading', { name: 'CheckMate' })).toBeVisible()

  // Create a checklist
  await page.getByLabel('Checklist name').fill(checklistName)
  await page.getByRole('button', { name: 'Create checklist' }).click()

  const checklistItem = page
    .getByRole('listitem')
    .filter({ hasText: checklistName })
  const deleteButton = checklistItem.getByRole('button', { name: 'Delete' })

  try {
    // Verify it appears in the list
    await expect(checklistItem).toBeVisible()
  } finally {
    // Best-effort cleanup so we don't leave data behind if the test fails mid-run
    await deleteButton.click().catch(() => {})
  }

  await expect(checklistItem).not.toBeVisible()
})
