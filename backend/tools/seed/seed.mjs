#!/usr/bin/env node

// ─── Enma Ingest Seed Script ────────────────────────────────────────
// Generates realistic event batches and sends them to the ingest API.
//
// Usage:
//   node seed.mjs [options]
//
// Options:
//   --base-url        Ingest gateway URL         (default: http://localhost:8090)
//   --org-id          Organization ID (GUID)     (required)
//   --project-id      Project ID (GUID)          (required)
//   --sdk-client-id   SDK client ID (GUID)       (default: random)
//   --process-def-id  Process definition ID       (default: random)
//   --scenario        Scenario name              (default: ecommerce)
//   --instances       Number of process instances (default: 50)
//   --batch-size      Events per HTTP request     (default: 200)
//   --delay           Delay between batches (ms)  (default: 100)
//   --dry-run         Print events without sending
//   --help            Show this help
// ─────────────────────────────────────────────────────────────────────

import { randomUUID } from "node:crypto";

// ─── CLI parsing ─────────────────────────────────────────────────────

function parseArgs() {
  const args = process.argv.slice(2);
  const opts = {
    baseUrl: "http://localhost:8090",
    orgId: "",
    projectId: "",
    sdkClientId: randomUUID(),
    processDefId: "",
    scenario: "ecommerce",
    instances: 50,
    batchSize: 200,
    delay: 100,
    dryRun: false,
  };

  for (let i = 0; i < args.length; i++) {
    switch (args[i]) {
      case "--base-url":
        opts.baseUrl = args[++i];
        break;
      case "--org-id":
        opts.orgId = args[++i];
        break;
      case "--project-id":
        opts.projectId = args[++i];
        break;
      case "--sdk-client-id":
        opts.sdkClientId = args[++i];
        break;
      case "--process-def-id":
        opts.processDefId = args[++i];
        break;
      case "--scenario":
        opts.scenario = args[++i];
        break;
      case "--instances":
        opts.instances = parseInt(args[++i], 10);
        break;
      case "--batch-size":
        opts.batchSize = parseInt(args[++i], 10);
        break;
      case "--delay":
        opts.delay = parseInt(args[++i], 10);
        break;
      case "--dry-run":
        opts.dryRun = true;
        break;
      case "--help":
        console.log(
          [
            "Enma Ingest Seed Script",
            "",
            "Options:",
            "  --base-url        Ingest gateway URL          (default: http://localhost:8090)",
            "  --org-id          Organization ID (GUID)      (required)",
            "  --project-id      Project ID (GUID)           (required)",
            "  --sdk-client-id   SDK client ID (GUID)        (default: random)",
            "  --process-def-id  Process definition ID       (default: random)",
            "  --scenario        ecommerce | support | auth  (default: ecommerce)",
            "  --instances       Process instances count      (default: 50)",
            "  --batch-size      Events per HTTP batch        (default: 200)",
            "  --delay           Delay between batches in ms  (default: 100)",
            "  --dry-run         Print events, don't send",
            "  --help            Show this help",
          ].join("\n")
        );
        process.exit(0);
    }
  }

  if (!opts.orgId || !opts.projectId) {
    console.error("Error: --org-id and --project-id are required.\n");
    console.error("Run with --help for usage info.");
    process.exit(1);
  }

  return opts;
}

// ─── Helpers ─────────────────────────────────────────────────────────

function randomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function pick(arr) {
  return arr[randomInt(0, arr.length - 1)];
}

function randomDate(baseDate, offsetMinutes) {
  return new Date(baseDate.getTime() + offsetMinutes * 60_000);
}

function makeEvent(name, payload, actor, processKeys, occurredAt, sdkClientId) {
  return {
    eventId: randomUUID(),
    sdkClientId,
    eventName: name,
    payload: payload ?? null,
    tags: null,
    processKeys,
    actor,
    occurredAt: occurredAt.toISOString(),
  };
}

// ─── Scenarios ───────────────────────────────────────────────────────

const SCENARIOS = {
  // E-commerce order flow
  ecommerce: (processDefId, sdkClientId) => {
    const steps = [
      {
        name: "order_created",
        payload: () => ({
          items: randomInt(1, 5),
          total: +(Math.random() * 500 + 10).toFixed(2),
          currency: "RUB",
        }),
      },
      {
        name: "payment_pending",
        payload: () => ({ method: pick(["card", "sbp", "wallet"]) }),
      },
      {
        name: "payment_processed",
        payload: () => ({ success: Math.random() > 0.1 }),
        // 10% chance of failure — some instances will stop here
        skipIf: (p) => !p.success,
      },
      {
        name: "order_confirmed",
        payload: () => ({ warehouse: pick(["MSK-1", "SPB-2", "NSK-3"]) }),
      },
      {
        name: "order_packed",
        payload: () => ({ packageWeight: +(Math.random() * 10 + 0.5).toFixed(1) }),
      },
      {
        name: "order_shipped",
        payload: () => ({
          carrier: pick(["CDEK", "Boxberry", "DPD", "PochtaRF"]),
          trackingId: `TRK-${randomInt(100000, 999999)}`,
        }),
      },
      {
        name: "order_delivered",
        payload: () => ({ signedBy: pick(["customer", "relative", "locker"]) }),
      },
    ];

    return generateProcessInstances(steps, processDefId, sdkClientId, "order");
  },

  // Customer support ticket flow
  support: (processDefId, sdkClientId) => {
    const steps = [
      {
        name: "ticket_opened",
        payload: () => ({
          channel: pick(["web", "email", "phone", "telegram"]),
          priority: pick(["low", "medium", "high", "critical"]),
          category: pick(["billing", "technical", "account", "feature_request"]),
        }),
      },
      {
        name: "ticket_assigned",
        payload: () => ({ agent: `agent-${randomInt(1, 20)}` }),
      },
      {
        name: "first_response_sent",
        payload: () => ({ responseTimeMin: randomInt(1, 120) }),
      },
      {
        name: "ticket_escalated",
        payload: () => ({ level: pick(["L2", "L3"]) }),
        chance: 0.3, // only 30% of tickets get escalated
      },
      {
        name: "ticket_resolved",
        payload: () => ({
          resolution: pick(["fixed", "workaround", "duplicate", "wont_fix"]),
        }),
      },
      {
        name: "ticket_closed",
        payload: () => ({
          satisfaction: randomInt(1, 5),
        }),
      },
    ];

    return generateProcessInstances(steps, processDefId, sdkClientId, "ticket");
  },

  // User authentication flow
  auth: (processDefId, sdkClientId) => {
    const steps = [
      {
        name: "session_started",
        payload: () => ({
          device: pick(["desktop", "mobile", "tablet"]),
          browser: pick(["Chrome", "Firefox", "Safari", "Edge"]),
          os: pick(["Windows", "macOS", "Linux", "iOS", "Android"]),
        }),
      },
      {
        name: "login_attempted",
        payload: () => ({
          method: pick(["password", "google_oauth", "github_oauth"]),
        }),
      },
      {
        name: "login_succeeded",
        payload: () => ({ mfa: Math.random() > 0.7 }),
        chance: 0.85,
      },
      {
        name: "login_failed",
        payload: () => ({
          reason: pick(["wrong_password", "account_locked", "mfa_failed"]),
        }),
        chance: 0.15,
      },
      {
        name: "page_viewed",
        payload: () => ({
          page: pick(["/dashboard", "/settings", "/analytics", "/projects"]),
        }),
      },
      {
        name: "session_ended",
        payload: () => ({
          reason: pick(["logout", "timeout", "token_expired"]),
          durationMin: randomInt(1, 180),
        }),
      },
    ];

    return generateProcessInstances(steps, processDefId, sdkClientId, "session");
  },
};

// ─── Instance generator ──────────────────────────────────────────────

let instanceCounter = 0;

function generateProcessInstances(steps, processDefId, sdkClientId, prefix) {
  return (count) => {
    const allEvents = [];

    for (let i = 0; i < count; i++) {
      instanceCounter++;
      const processId = `${prefix}-${String(instanceCounter).padStart(6, "0")}`;
      const userId = `user-${randomInt(1, 200)}`;
      const actor = { userId, anonymousId: null };
      const processKeys = [{ processDefinitionId: processDefId, processId }];

      // Random start time within last 30 days
      const baseTime = new Date(Date.now() - randomInt(0, 30 * 24 * 60) * 60_000);
      let cursor = baseTime;
      let stopped = false;

      for (const step of steps) {
        if (stopped) break;

        // Some steps are probabilistic
        if (step.chance !== undefined && Math.random() > step.chance) continue;

        cursor = randomDate(cursor, randomInt(1, 60)); // 1-60 min between steps
        const payload = step.payload();

        allEvents.push(
          makeEvent(step.name, payload, actor, processKeys, cursor, sdkClientId)
        );

        // Some steps can stop the process early
        if (step.skipIf && step.skipIf(payload)) {
          stopped = true;
        }
      }
    }

    return allEvents;
  };
}

// ─── Sender ──────────────────────────────────────────────────────────

async function sendBatch(url, events) {
  const resp = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(events),
  });

  if (!resp.ok) {
    const text = await resp.text();
    throw new Error(`HTTP ${resp.status}: ${text}`);
  }
  return resp.status;
}

function sleep(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

// ─── Main ────────────────────────────────────────────────────────────

async function main() {
  const opts = parseArgs();
  const scenarioFn = SCENARIOS[opts.scenario];

  if (!scenarioFn) {
    console.error(
      `Unknown scenario: "${opts.scenario}". Available: ${Object.keys(SCENARIOS).join(", ")}`
    );
    process.exit(1);
  }

  const processDefId = opts.processDefId || randomUUID();

  console.log("Enma Ingest Seed");
  console.log("────────────────────────────────────");
  console.log(`  Scenario:       ${opts.scenario}`);
  console.log(`  Instances:      ${opts.instances}`);
  console.log(`  Base URL:       ${opts.baseUrl}`);
  console.log(`  Org ID:         ${opts.orgId}`);
  console.log(`  Project ID:     ${opts.projectId}`);
  console.log(`  SDK Client ID:  ${opts.sdkClientId}`);
  console.log(`  Process Def ID: ${processDefId}`);
  console.log(`  Batch size:     ${opts.batchSize}`);
  console.log(`  Dry run:        ${opts.dryRun}`);
  console.log("────────────────────────────────────\n");

  const generator = scenarioFn(processDefId, opts.sdkClientId);
  const events = generator(opts.instances);

  console.log(`Generated ${events.length} events for ${opts.instances} process instances.\n`);

  if (opts.dryRun) {
    console.log(JSON.stringify(events.slice(0, 3), null, 2));
    console.log(`\n... and ${events.length - 3} more events (dry run, not sent).`);
    return;
  }

  const url = `${opts.baseUrl}/api/ingest/v1/organizations/${opts.orgId}/projects/${opts.projectId}/events/batch`;

  let sent = 0;
  for (let i = 0; i < events.length; i += opts.batchSize) {
    const chunk = events.slice(i, i + opts.batchSize);
    try {
      const status = await sendBatch(url, chunk);
      sent += chunk.length;
      console.log(
        `  [${sent}/${events.length}] Sent ${chunk.length} events — HTTP ${status}`
      );
    } catch (err) {
      console.error(`  FAILED at offset ${i}: ${err.message}`);
      process.exit(1);
    }

    if (i + opts.batchSize < events.length && opts.delay > 0) {
      await sleep(opts.delay);
    }
  }

  console.log(`\nDone! Sent ${sent} events total.`);
  console.log(`\nProcess Definition ID (save for admin): ${processDefId}`);
}

main();
