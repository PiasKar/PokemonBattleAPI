<script setup>
import { computed, onMounted, ref } from 'vue'
import { fightPokemons, getPokemons, importPokemon } from '../services/pokemonApi'

const pokemonName = ref('')
const pokemons = ref([])
const pokemonAId = ref('')
const pokemonBId = ref('')
const loading = ref(false)
const actionMessage = ref('')
const actionError = ref('')
const battleResult = ref(null)

const canFight = computed(() => {
  return pokemonAId.value && pokemonBId.value && pokemonAId.value !== pokemonBId.value
})

const normalize = (item, camelKey, pascalKey) => item?.[camelKey] ?? item?.[pascalKey]

async function loadPokemons() {
  loading.value = true
  actionError.value = ''

  try {
    const data = await getPokemons()
    pokemons.value = Array.isArray(data) ? data : []

    if (!pokemons.value.some((p) => String(normalize(p, 'id', 'Id')) === pokemonAId.value)) {
      pokemonAId.value = ''
    }

    if (!pokemons.value.some((p) => String(normalize(p, 'id', 'Id')) === pokemonBId.value)) {
      pokemonBId.value = ''
    }
  } catch (error) {
    actionError.value = `Could not load pokemons: ${error.message}`
  } finally {
    loading.value = false
  }
}

async function addPokemon() {
  const name = pokemonName.value.trim()

  if (!name) {
    actionError.value = 'Enter pokemon name first.'
    return
  }

  actionMessage.value = ''
  actionError.value = ''

  try {
    await importPokemon(name)
    actionMessage.value = `Pokemon "${name}" added.`
    pokemonName.value = ''
    await loadPokemons()
  } catch (error) {
    actionError.value = `Could not add pokemon: ${error.message}`
  }
}

async function fight() {
  if (!canFight.value) {
    actionError.value = 'Choose two different pokemons.'
    return
  }

  actionMessage.value = ''
  actionError.value = ''

  try {
    battleResult.value = await fightPokemons(pokemonAId.value, pokemonBId.value)
  } catch (error) {
    actionError.value = `Could not start fight: ${error.message}`
  }
}

onMounted(loadPokemons)
</script>

<template>
  <section>
    <div class="card">
      <h2>Add pokemon to collection</h2>
      <div class="row">
        <input
          v-model="pokemonName"
          type="text"
          placeholder="Enter pokemon name"
          @keyup.enter="addPokemon"
        />
        <button type="button" @click="addPokemon">Add</button>
      </div>
      <p v-if="actionMessage" class="success">{{ actionMessage }}</p>
      <p v-if="actionError" class="error">{{ actionError }}</p>
    </div>

    <div class="card">
      <h2>Pokemon collection</h2>
      <button type="button" @click="loadPokemons">Refresh</button>
      <p v-if="loading">Loading...</p>
      <table v-else>
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>PrimaryType</th>
            <th>Power</th>
            <th>Speed</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="pokemon in pokemons" :key="normalize(pokemon, 'id', 'Id')">
            <td>{{ normalize(pokemon, 'id', 'Id') }}</td>
            <td>{{ normalize(pokemon, 'name', 'Name') }}</td>
            <td>{{ normalize(pokemon, 'primaryType', 'PrimaryType') }}</td>
            <td>{{ normalize(pokemon, 'power', 'Power') }}</td>
            <td>{{ normalize(pokemon, 'speed', 'Speed') }}</td>
          </tr>
          <tr v-if="!pokemons.length">
            <td colspan="5">No pokemons in collection.</td>
          </tr>
        </tbody>
      </table>
    </div>

    <div class="card">
      <h2>Fight</h2>
      <div class="row">
        <label>
          Pokemon A
          <select v-model="pokemonAId">
            <option disabled value="">Choose pokemon</option>
            <option
              v-for="pokemon in pokemons"
              :key="`a-${normalize(pokemon, 'id', 'Id')}`"
              :value="String(normalize(pokemon, 'id', 'Id'))"
            >
              {{ normalize(pokemon, 'name', 'Name') }} (ID: {{ normalize(pokemon, 'id', 'Id') }})
            </option>
          </select>
        </label>

        <label>
          Pokemon B
          <select v-model="pokemonBId">
            <option disabled value="">Choose pokemon</option>
            <option
              v-for="pokemon in pokemons"
              :key="`b-${normalize(pokemon, 'id', 'Id')}`"
              :value="String(normalize(pokemon, 'id', 'Id'))"
            >
              {{ normalize(pokemon, 'name', 'Name') }} (ID: {{ normalize(pokemon, 'id', 'Id') }})
            </option>
          </select>
        </label>

        <button type="button" :disabled="!canFight" @click="fight">Fight</button>
      </div>

      <div v-if="battleResult" class="result">
        <h3>Battle result</h3>
        <p>
          {{ normalize(battleResult, 'pokemonA', 'PokemonA') }}
          vs
          {{ normalize(battleResult, 'pokemonB', 'PokemonB') }}
        </p>
        <p><strong>Winner:</strong> {{ normalize(battleResult, 'winner', 'Winner') }}</p>
        <p><strong>EffectivePowerA:</strong> {{ normalize(battleResult, 'effectivePowerA', 'EffectivePowerA') }}</p>
        <p><strong>EffectivePowerB:</strong> {{ normalize(battleResult, 'effectivePowerB', 'EffectivePowerB') }}</p>
        <p><strong>Reason:</strong> {{ normalize(battleResult, 'reason', 'Reason') }}</p>
        <p><strong>CreatedAt:</strong> {{ normalize(battleResult, 'createdAt', 'CreatedAt') }}</p>
      </div>
    </div>
  </section>
</template>

<style scoped>
.card {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
}

.row {
  display: flex;
  gap: 0.75rem;
  align-items: end;
  flex-wrap: wrap;
}

input,
select,
button {
  padding: 0.5rem;
}

table {
  width: 100%;
  margin-top: 0.75rem;
  border-collapse: collapse;
}

th,
td {
  border: 1px solid #e5e7eb;
  padding: 0.5rem;
  text-align: left;
}

.success {
  color: #166534;
}

.error {
  color: #b91c1c;
}

.result {
  margin-top: 1rem;
  background: #f9fafb;
  padding: 0.75rem;
  border-radius: 6px;
}
</style>
