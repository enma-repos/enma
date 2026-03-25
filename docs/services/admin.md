# Admin

Organization, project, team, API key, SDK client, and event/process definition management.

## Responsibilities

- Organization and project CRUD
- Member and role management
- Invitations
- API keys and SDK clients
- Event and process definitions
- Audit logs and notifications
- gRPC server for Auth Service

## API

| Method | Path | Description |
|--------|------|-------------|
| GET/POST | `/api/admin/v1/organizations` | Organizations |
| GET | `/api/admin/v1/organizations/by-slug/{slug}` | Organization by slug |
| GET/POST | `/api/admin/v1/projects` | Projects |
| GET/POST/DELETE | `/api/admin/v1/api-keys` | API keys |
| GET/POST | `/api/admin/v1/invites` | Invitations |
| GET/POST | `/api/admin/v1/organization-members` | Organization members |
| GET/POST | `/api/admin/v1/project-members` | Project members |
| GET/POST | `/api/admin/v1/event-definitions` | Event definitions |
| GET/POST | `/api/admin/v1/process-definitions` | Process definitions |
| GET | `/api/admin/v1/audit-logs` | Audit logs |
| GET | `/api/admin/v1/notifications` | Notifications |
| GET/POST | `/api/admin/v1/sdk-clients` | SDK clients |

## Dependencies

- **PostgreSQL** — primary data store

## Configuration

| Parameter | Description |
|-----------|-------------|
| `ConnectionStrings:Postgres` | Database connection string |
| `Jwt:Key` | JWT validation key |

## Ports

- **8080** — REST (HTTP/1.1 + HTTP/2)
- **8081** — gRPC (HTTP/2)
