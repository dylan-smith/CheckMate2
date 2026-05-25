import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

test.describe('Accessibility', () => {
  test('main page has no detectable accessibility violations', async ({
    page,
  }) => {
    await page.goto('/')
    await expect(page.getByRole('heading', { name: 'CheckMate' })).toBeVisible()

    const results = await new AxeBuilder({ page }).analyze()

    expect(results.violations).toEqual([])
  })

  test('create checklist form has no detectable accessibility violations', async ({
    page,
  }) => {
    await page.goto('/')
    await expect(
      page.getByRole('heading', { name: 'Create checklist' }),
    ).toBeVisible()

    const results = await new AxeBuilder({ page }).analyze()

    expect(results.violations).toEqual([])
  })

  test('edit checklist form has no detectable accessibility violations', async ({
    page,
  }) => {
    await page.goto('/')

    const checklistName = `A11y test checklist ${Date.now()}`

    // Create a checklist so we can enter edit mode
    await page.getByLabel('Checklist name').fill(checklistName)
    await page.getByRole('button', { name: 'Create checklist' }).click()
    await expect(page.getByText(checklistName)).toBeVisible()

    try {
      // Enter edit mode for the specific checklist
      await page
        .getByRole('listitem')
        .filter({ hasText: checklistName })
        .getByRole('button', { name: 'Edit' })
        .click()
      await expect(
        page.getByRole('heading', { name: 'Edit checklist' }),
      ).toBeVisible()

      const results = await new AxeBuilder({ page }).analyze()

      expect(results.violations).toEqual([])
    } finally {
      // Clean up: cancel edit mode if active, then delete the checklist
      if (await page.getByRole('button', { name: 'Cancel' }).isVisible()) {
        await page.getByRole('button', { name: 'Cancel' }).click()
      }
      await page
        .getByRole('listitem')
        .filter({ hasText: checklistName })
        .getByRole('button', { name: 'Delete' })
        .click()
    }
  })
})
