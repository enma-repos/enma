# SDK API Gateway

SDK-facing reverse proxy for event ingestion. Separate from the main API Gateway to allow independent scaling and security policies for high-throughput SDK traffic.

## Responsibilities

- Route SDK requests to Ingest Service
- IP-based rate limiting (fixed window)
- Security headers (HSTS, X-Frame-Options, etc.)
- URI length validation
- CORS (allows any origin for SDK clients)

## Configuration

| Section | Description |
|---------|-------------|
| `ReverseProxy` | YARP routes to Ingest |
| `RateLimiting` | Per-IP limits |
| `Cors` | Allowed origins |

## Health Check

```
GET /health
```

## Dev Tools

In development mode, exposes Scalar API reference with Ingest OpenAPI spec at `/scalar`.
