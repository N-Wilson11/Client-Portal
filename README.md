# Client-Portal

ASP.NET Core (C#, Razor Pages + Identity, SQLite lokaal / Supabase op Vercel) klantenportaal:
registreren, inloggen, profiel (`/Account/Profile`) en abonnement (`/Account/Subscription`) beheren.

Starten: `cd src/ClientPortal && dotnet run`

## Met Docker Compose

Start de app vanuit de repository-root:

```bash
docker compose up --build
```

Open [http://localhost:8080](http://localhost:8080). Stop de app met `Ctrl+C`,
of op de achtergrond met `docker compose up --build -d` en `docker compose down`.
De SQLite-database blijft bewaard in het Docker-volume `client-portal-data`,
ook wanneer de container wordt verwijderd.

## Deployen naar Vercel

Vercel gebruikt de `Dockerfile` in de repository-root om de ASP.NET Core-app als
container te deployen. Verbind de repository met Vercel en stel de **Root
Directory** in op de repository-root. Als dit een bestaand Vercel-project is,
selecteer dan de **Container**-preset.

De app gebruikt in Vercel Supabase PostgreSQL; SQLite-bestanden en Docker-volumes
zijn daar niet duurzaam. Maak een Supabase-project en voeg in
**Project Settings → Environment Variables** deze variabelen toe voor Preview
en Production:

| Variabele | Waarde |
| --- | --- |
| `PORT` | `8080` |
| `Database__Provider` | `Supabase` |
| `ConnectionStrings__Supabase` | Npgsql-verbinding van Supabase |

Gebruik de connection string uit de Supabase-dashboardinstellingen (de
Session pooler is geschikt wanneer de hostingomgeving geen IPv6 ondersteunt).
Stel de verbinding in met Npgsql's key/value-indeling, niet met de
`postgresql://...` URL. Supabase gebruikt PostgreSQL, dus de app verbindt via
EF Core/Npgsql; aanmelden blijft beheerd door ASP.NET Core Identity. De app
maakt de Identity-tabellen bij de eerste start aan. Push daarna naar de
gekoppelde repository of start een deployment met Vercel CLI.
