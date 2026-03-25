# API Gateway

Single entry point for all clients. YARP-based reverse proxy with routing, rate limiting, and request transformation.

## Responsibilities

- Route requests to backend services
- Extract JWT from `access_token` cookie and forward as `Authorization: Bearer` header
- IP-based rate limiting (fixed window)
- Security headers (HSTS, X-Frame-Options, etc.)
- `X-Request-Id` propagation
- CORS

## Configuration

| Section | Description |
|---------|-------------|
| `ReverseProxy` | YARP routes (clusters, routes) |
| `RateLimiting` | Per-IP limits |
| `Cors` | Allowed origins |

## Port

- **8080** — HTTP/1.1 + HTTP/2

## Health Check

```
GET /health
```
