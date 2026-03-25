# Analytics

Business process analytics: time trends, funnels, flow graphs, actor breakdowns.

## Responsibilities

- Time trends
- Funnels
- Process flow graphs
- Top events
- Actor breakdowns
- Entry/exit analysis
- Event details

## API

All endpoints are prefixed with `/api/analytics/v1/organizations/{orgId}/projects/{projectId}/process-definitions/{procDefId}/`.

| Method | Path | Description |
|--------|------|-------------|
| GET | `.../top-events` | Top events |
| GET | `.../time-trends` | Time trends |
| GET | `.../funnel` | Funnel |
| GET | `.../flow-graph` | Flow graph |
| GET | `.../actor-breakdown` | Actor breakdown |
| GET | `.../entry-exit` | Entry/exit analysis |
| GET | `.../event-detail` | Event details |

## Dependencies

- **MongoDB** — analytics data

## Configuration

| Parameter | Description |
|-----------|-------------|
| `ConnectionStrings:MongoDB` | Connection string |
| `Jwt:Key` | JWT validation key |

## Port

- **8081** — HTTP/1.1 + HTTP/2
