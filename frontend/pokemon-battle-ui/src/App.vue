<script setup>
import PokemonCollection from './components/PokemonCollection.vue'
</script>

<template>
  <main>
    <h1>Pokemon Battle</h1>
    <PokemonCollection />
  </main>
</template>

<style scoped>
main {
  max-width: 920px;
  margin: 2rem auto;
  padding: 0 1rem 2rem;
  font-family: Arial, sans-serif;
  color: #1f2937;
}

h1 {
  margin-bottom: 1rem;
}
</style>
