# 1. Use PostgreSQL instead of SQL Server

## Status

Accepted

## Context

The project was originally scaffolded with SQL Server as the default
EF Core provider (`Microsoft.EntityFrameworkCore.SqlServer`), since
that's what the `dotnet new webapi` template's typical tutorials assume.
The developer did not have SQL Server installed locally and did not
want to install it solely to support a learning/portfolio project.

The developer also has an existing Supabase account (which runs on
PostgreSQL) and wants the option to deploy this project's database
there, or to AWS RDS, later on — both of which support PostgreSQL
natively. Using PostgreSQL locally keeps the local and eventual
hosted environments consistent.

## Decision

Use PostgreSQL as the database engine, accessed via the
`Npgsql.EntityFrameworkCore.PostgreSQL` EF Core provider. Run it
locally in a container (via Rancher Desktop / Docker Compose) rather
than installing Postgres natively, so the local environment is
disposable and reproducible.

## Consequences

- `Program.cs` uses `UseNpgsql(...)` instead of `UseSqlServer(...)`.
- Connection strings follow Npgsql's format
  (`Host=;Port=;Database=;Username=;Password=`) rather than SQL
  Server's (`Server=;Trusted_Connection=`).
- Local development requires Rancher Desktop running; the Postgres
  container and its data volume are defined in `docker-compose.yml`
  at the repo root.
- If a hosted Postgres provider (e.g. Supabase) is used for a
  deployed environment later, EF Core migrations must target the
  provider's direct/session connection, not a transaction-mode
  connection pooler, due to compatibility issues with DDL statements.
