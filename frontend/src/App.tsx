import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'

type Checklist = {
  id: number
  name: string
}

type ErrorResponse = {
  message?: string
}

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5269'

function App() {
  const [checklists, setChecklists] = useState<Checklist[]>([])
  const [name, setName] = useState('')
  const [editingId, setEditingId] = useState<number | null>(null)
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')

  const isEditing = useMemo(() => editingId !== null, [editingId])
  const formTitle = useMemo(() => (isEditing ? 'Edit checklist' : 'Create checklist'), [isEditing])

  useEffect(() => {
    void loadChecklists()
  }, [])

  async function loadChecklists() {
    setLoading(true)
    setErrorMessage('')

    try {
      const response = await fetch(`${apiBaseUrl}/api/checklists`)
      if (!response.ok) {
        throw new Error('Unable to load checklists.')
      }

      const payload = (await response.json()) as Checklist[]
      setChecklists(payload)
    } catch {
      setErrorMessage('Unable to load checklists.')
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const trimmedName = name.trim()
    if (!trimmedName) {
      setErrorMessage('Checklist name is required.')
      return
    }

    setSubmitting(true)
    setErrorMessage('')

    try {
      const response = await fetch(
        isEditing ? `${apiBaseUrl}/api/checklists/${editingId}` : `${apiBaseUrl}/api/checklists`,
        {
          method: isEditing ? 'PUT' : 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ name: trimmedName }),
        },
      )

      if (!response.ok) {
        if (response.status === 409) {
          const error = (await response.json()) as ErrorResponse
          setErrorMessage(error.message ?? 'A checklist with this name already exists.')
          return
        }

        throw new Error('Unable to save checklist.')
      }

      setName('')
      setEditingId(null)
      await loadChecklists()
    } catch {
      setErrorMessage('Unable to save checklist.')
    } finally {
      setSubmitting(false)
    }
  }

  function startEdit(checklist: Checklist) {
    setEditingId(checklist.id)
    setName(checklist.name)
    setErrorMessage('')
  }

  function cancelEdit() {
    setEditingId(null)
    setName('')
    setErrorMessage('')
  }

  async function deleteChecklist(id: number) {
    setErrorMessage('')

    try {
      const response = await fetch(`${apiBaseUrl}/api/checklists/${id}`, {
        method: 'DELETE',
      })

      if (!response.ok && response.status !== 404) {
        throw new Error('Unable to delete checklist.')
      }

      if (editingId === id) {
        cancelEdit()
      }

      await loadChecklists()
    } catch {
      setErrorMessage('Unable to delete checklist.')
    }
  }

  return (
    <main className="app">
      <h1>CheckMate2</h1>
      <p className="subtitle">Create, edit, and delete your custom checklists.</p>

      <section className="panel">
        <h2>{formTitle}</h2>
        <form onSubmit={handleSubmit} className="form">
          <label htmlFor="checklist-name">Checklist name</label>
          <input
            id="checklist-name"
            type="text"
            value={name}
            onChange={(event) => setName(event.target.value)}
            maxLength={200}
            required
            placeholder="e.g. Daily chores"
          />

          <div className="actions">
            <button type="submit" disabled={submitting}>
              {submitting ? 'Saving…' : isEditing ? 'Save changes' : 'Create checklist'}
            </button>
            {isEditing && (
              <button type="button" className="secondary" onClick={cancelEdit}>
                Cancel
              </button>
            )}
          </div>
        </form>
      </section>

      {errorMessage && <p className="error">{errorMessage}</p>}

      <section className="panel">
        <h2>Checklists</h2>
        {loading ? (
          <p>Loading…</p>
        ) : checklists.length === 0 ? (
          <p>No checklists yet.</p>
        ) : (
          <ul className="checklist-list">
            {checklists.map((checklist) => (
              <li key={checklist.id}>
                <span>{checklist.name}</span>
                <div className="item-actions">
                  <button type="button" className="secondary" onClick={() => startEdit(checklist)}>
                    Edit
                  </button>
                  <button type="button" className="danger" onClick={() => void deleteChecklist(checklist.id)}>
                    Delete
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </section>
    </main>
  )
}

export default App
