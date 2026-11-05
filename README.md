# Client-Portal

ASP.NET Core (C#, Razor Pages + Identity, SQLite) klantenportaal:
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
