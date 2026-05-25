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

    // Create a checklist so we can enter edit mode
    await page.getByLabel('Checklist name').fill('A11y test checklist')
    await page.getByRole('button', { name: 'Create checklist' }).click()
    await expect(page.getByText('A11y test checklist')).toBeVisible()

    // Enter edit mode
    await page.getByRole('button', { name: 'Edit' }).click()
    await expect(
      page.getByRole('heading', { name: 'Edit checklist' }),
    ).toBeVisible()

    const results = await new AxeBuilder({ page }).analyze()

    expect(results.violations).toEqual([])
  })
})
