# pokemon-battle-ui

Prosty frontend w Vue 3 dla istniejącego backendu .NET Web API.

## 1) Instalacja

```bash
cd frontend/pokemon-battle-ui
npm install
```

## 2) Konfiguracja API

Otwórz plik `src/services/pokemonApi.js` i ustaw poprawny adres backendu:

```js
const API_BASE = 'https://localhost:XXXX'
```

W miejsce `XXXX` wpisz port backendu .NET (sprawdź np. `Properties/launchSettings.json` backendu).

## 3) Uruchomienie

```bash
npm run dev
```

## Szybkie utworzenie aplikacji (przykład)

```bash
npm create vue@latest
npm install
npm run dev
```

## Minimalny CORS (opcjonalnie, po stronie backendu)

Jeśli przeglądarka blokuje żądania, możesz dodać minimalny CORS w `Program.cs` backendu:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

app.UseCors("Frontend");
```

To nie zmienia logiki endpointów, tylko zezwala frontendowi na wywołania API.
