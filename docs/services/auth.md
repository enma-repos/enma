# Auth

User authentication, JWT token issuance and renewal, external OAuth provider integration.

## Responsibilities

- User registration and authentication
- JWT access_token / refresh_token issuance (HttpOnly cookies)
- Token refresh
- Google OAuth
- Onboarding flow
- gRPC communication with Admin Service for user creation

## API

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/auth/v1/refresh` | Refresh JWT tokens |
| POST | `/api/auth/v1/logout` | Log out |
| GET | `/api/auth/v1/me` | Current user profile |
| POST | `/api/auth/v1/onboarding/complete` | Complete onboarding |
| — | `/api/auth/v1/external/*` | Google OAuth flow |

## Dependencies

- **PostgreSQL** — accounts and sessions
- **Redis** — token cache
- **Admin Service** — gRPC client for user creation

## Configuration

| Parameter | Description |
|-----------|-------------|
| `Jwt:Key` | Symmetric signing key |
| `Jwt:Issuer` / `Jwt:Audience` | Token parameters |
| `Google:ClientId` / `Google:ClientSecret` | OAuth credentials |
| `ConnectionStrings:Postgres` | Database connection string |
| `ConnectionStrings:Redis` | Redis connection string |

## Port

- **8082** — HTTP/1.1 + HTTP/2
