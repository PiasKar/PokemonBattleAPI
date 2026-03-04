// Ustaw port backendu .NET (np. https://localhost:7178)
const API_BASE = 'https://localhost:XXXX'

async function request(path, options = {}) {
  const response = await fetch(`${API_BASE}${path}`, options)

  if (!response.ok) {
    const message = await response.text()
    throw new Error(message || `Request failed: ${response.status}`)
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

export function importPokemon(nameOrId) {
  return request(`/api/pokemons/import/${encodeURIComponent(nameOrId)}`, {
    method: 'POST',
  })
}

export function getPokemons() {
  return request('/api/pokemons')
}

export function fightPokemons(pokemonAId, pokemonBId) {
  const params = new URLSearchParams({
    pokemonAId: String(pokemonAId),
    pokemonBId: String(pokemonBId),
  })

  return request(`/api/pokemons/fight?${params.toString()}`, {
    method: 'POST',
  })
}

export { API_BASE }
