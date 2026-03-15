#!/usr/bin/env node

// ─── Enma Live Traffic Simulator ────────────────────────────────────
// Simulates real-time user activity by continuously generating and
// sending events with realistic timing. Runs until Ctrl+C.
//
// Usage:
//   node live.mjs [options]
//
// Options:
//   --base-url        Ingest gateway URL         (default: http://localhost:8090)
//   --org-id          Organization ID (GUID)     (required)
//   --project-id      Project ID (GUID)          (required)
//   --sdk-client-id   SDK client ID (GUID)       (default: random)
//   --scenario        Scenario name              (default: ecommerce)
//   --users           Concurrent simulated users  (default: 10)
//   --rate            New process instances / min  (default: 5)
//   --process-def-id  Process definition ID       (default: random)
//   --help            Show this help
// ─────────────────────────────────────────────────────────────────────

import { randomUUID } from "node:crypto";

// ─── CLI ─────────────────────────────────────────────────────────────

function parseArgs() {
  const args = process.argv.slice(2);
  const opts = {
    baseUrl: "http://localhost:8090",
    orgId: "",
    projectId: "",
    sdkClientId: randomUUID(),
    scenario: "ecommerce",
    users: 10,
    rate: 5,
    processDefId: "",
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
      case "--scenario":
        opts.scenario = args[++i];
        break;
      case "--users":
        opts.users = parseInt(args[++i], 10);
        break;
      case "--rate":
        opts.rate = parseFloat(args[++i]);
        break;
      case "--process-def-id":
        opts.processDefId = args[++i];
        break;
      case "--help":
        console.log(
          [
            "Enma Live Traffic Simulator",
            "",
            "Options:",
            "  --base-url        Ingest gateway URL          (default: http://localhost:8090)",
            "  --org-id          Organization ID (GUID)      (required)",
            "  --project-id      Project ID (GUID)           (required)",
            "  --sdk-client-id   SDK client ID (GUID)        (default: random)",
            "  --scenario        ecommerce | support | auth  (default: ecommerce)",
            "  --users           Concurrent simulated users   (default: 10)",
            "  --rate            New instances per minute      (default: 5)",
            "  --process-def-id  Process definition ID        (default: random)",
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

  if (!opts.processDefId) opts.processDefId = randomUUID();

  return opts;
}

// ─── Helpers ─────────────────────────────────────────────────────────

function randomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function pick(arr) {
  return arr[randomInt(0, arr.length - 1)];
}

function sleep(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

// ─── Scenario step definitions ───────────────────────────────────────

const SCENARIO_STEPS = {
  ecommerce: [
    {
      name: "order_created",
      delaySecRange: [0, 2],
      payload: () => ({
        items: randomInt(1, 5),
        total: +(Math.random() * 500 + 10).toFixed(2),
        currency: "RUB",
      }),
    },
    {
      name: "payment_pending",
      delaySecRange: [3, 15],
      payload: () => ({ method: pick(["card", "sbp", "wallet"]) }),
    },
    {
      name: "payment_processed",
      delaySecRange: [5, 30],
      payload: () => ({ success: Math.random() > 0.1 }),
      stopIf: (p) => !p.success,
    },
    {
      name: "order_confirmed",
      delaySecRange: [2, 10],
      payload: () => ({ warehouse: pick(["MSK-1", "SPB-2", "NSK-3"]) }),
    },
    {
      name: "order_packed",
      delaySecRange: [10, 60],
      payload: () => ({ packageWeight: +(Math.random() * 10 + 0.5).toFixed(1) }),
    },
    {
      name: "order_shipped",
      delaySecRange: [5, 30],
      payload: () => ({
        carrier: pick(["CDEK", "Boxberry", "DPD", "PochtaRF"]),
        trackingId: `TRK-${randomInt(100000, 999999)}`,
      }),
    },
    {
      name: "order_delivered",
      delaySecRange: [15, 90],
      payload: () => ({ signedBy: pick(["customer", "relative", "locker"]) }),
    },
  ],

  support: [
    {
      name: "ticket_opened",
      delaySecRange: [0, 2],
      payload: () => ({
        channel: pick(["web", "email", "phone", "telegram"]),
        priority: pick(["low", "medium", "high", "critical"]),
        category: pick(["billing", "technical", "account", "feature_request"]),
      }),
    },
    {
      name: "ticket_assigned",
      delaySecRange: [5, 30],
      payload: () => ({ agent: `agent-${randomInt(1, 20)}` }),
    },
    {
      name: "first_response_sent",
      delaySecRange: [10, 60],
      payload: () => ({ responseTimeMin: randomInt(1, 120) }),
    },
    {
      name: "ticket_escalated",
      delaySecRange: [10, 40],
      payload: () => ({ level: pick(["L2", "L3"]) }),
      chance: 0.3,
    },
    {
      name: "ticket_resolved",
      delaySecRange: [15, 60],
      payload: () => ({ resolution: pick(["fixed", "workaround", "duplicate", "wont_fix"]) }),
    },
    {
      name: "ticket_closed",
      delaySecRange: [5, 20],
      payload: () => ({ satisfaction: randomInt(1, 5) }),
    },
  ],

  auth: [
    {
      name: "session_started",
      delaySecRange: [0, 2],
      payload: () => ({
        device: pick(["desktop", "mobile", "tablet"]),
        browser: pick(["Chrome", "Firefox", "Safari", "Edge"]),
        os: pick(["Windows", "macOS", "Linux", "iOS", "Android"]),
      }),
    },
    {
      name: "login_attempted",
      delaySecRange: [2, 8],
      payload: () => ({ method: pick(["password", "google_oauth", "github_oauth"]) }),
    },
    {
      name: "login_succeeded",
      delaySecRange: [1, 5],
      payload: () => ({ mfa: Math.random() > 0.7 }),
      chance: 0.85,
    },
    {
      name: "login_failed",
      delaySecRange: [1, 3],
      payload: () => ({ reason: pick(["wrong_password", "account_locked", "mfa_failed"]) }),
      chance: 0.15,
      stopAfter: true,
    },
    {
      name: "page_viewed",
      delaySecRange: [3, 20],
      payload: () => ({ page: pick(["/dashboard", "/settings", "/analytics", "/projects"]) }),
    },
    {
      name: "session_ended",
      delaySecRange: [10, 60],
      payload: () => ({
        reason: pick(["logout", "timeout", "token_expired"]),
        durationMin: randomInt(1, 180),
      }),
    },
  ],
};

// ─── Live user simulation ────────────────────────────────────────────

let instanceCounter = 0;
let totalEventsSent = 0;
let activeUsers = 0;

async function sendEvent(url, event) {
  const resp = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify([event]),
  });
  if (!resp.ok) {
    const text = await resp.text();
    throw new Error(`HTTP ${resp.status}: ${text}`);
  }
}

async function simulateUser(opts, steps) {
  instanceCounter++;
  activeUsers++;

  const prefix =
    opts.scenario === "ecommerce"
      ? "order"
      : opts.scenario === "support"
        ? "ticket"
        : "session";

  const processId = `${prefix}-${String(instanceCounter).padStart(6, "0")}`;
  const userId = `user-${randomInt(1, 200)}`;
  const actor = { userId, anonymousId: null };
  const processKeys = [
    { processDefinitionId: opts.processDefId, processId },
  ];

  const url = `${opts.baseUrl}/api/ingest/v1/organizations/${opts.orgId}/projects/${opts.projectId}/events/batch`;

  for (const step of steps) {
    // Probabilistic steps
    if (step.chance !== undefined && Math.random() > step.chance) continue;

    // Wait real time between steps
    const [minSec, maxSec] = step.delaySecRange;
    const delaySec = randomInt(minSec, maxSec);
    if (delaySec > 0) await sleep(delaySec * 1000);

    const payload = step.payload();
    const event = {
      eventId: randomUUID(),
      sdkClientId: opts.sdkClientId,
      eventName: step.name,
      payload,
      tags: null,
      processKeys,
      actor,
      occurredAt: new Date().toISOString(),
    };

    try {
      await sendEvent(url, event);
      totalEventsSent++;
      const ts = new Date().toLocaleTimeString();
      console.log(
        `  [${ts}]  ${processId}  ->  ${step.name}  (active: ${activeUsers}, total: ${totalEventsSent})`
      );
    } catch (err) {
      console.error(`  ERROR  ${processId}  ${step.name}: ${err.message}`);
    }

    // Early stop conditions
    if (step.stopIf && step.stopIf(payload)) break;
    if (step.stopAfter) break;
  }

  activeUsers--;
}

// ─── Main loop ───────────────────────────────────────────────────────

async function main() {
  const opts = parseArgs();
  const steps = SCENARIO_STEPS[opts.scenario];

  if (!steps) {
    console.error(
      `Unknown scenario: "${opts.scenario}". Available: ${Object.keys(SCENARIO_STEPS).join(", ")}`
    );
    process.exit(1);
  }

  console.log("Enma Live Traffic Simulator");
  console.log("────────────────────────────────────");
  console.log(`  Scenario:       ${opts.scenario}`);
  console.log(`  Users (max):    ${opts.users}`);
  console.log(`  Rate:           ${opts.rate} instances/min`);
  console.log(`  Base URL:       ${opts.baseUrl}`);
  console.log(`  Org ID:         ${opts.orgId}`);
  console.log(`  Project ID:     ${opts.projectId}`);
  console.log(`  SDK Client ID:  ${opts.sdkClientId}`);
  console.log(`  Process Def ID: ${opts.processDefId}`);
  console.log("────────────────────────────────────");
  console.log("  Press Ctrl+C to stop.\n");

  const spawnIntervalMs = (60 / opts.rate) * 1000;

  // Graceful shutdown
  let running = true;
  process.on("SIGINT", () => {
    console.log(`\nStopping... (sent ${totalEventsSent} events total)`);
    running = false;
  });

  while (running) {
    if (activeUsers < opts.users) {
      // Fire and forget — the user simulation runs in the background
      simulateUser(opts, steps).catch((err) =>
        console.error(`  User simulation error: ${err.message}`)
      );
    }

    await sleep(spawnIntervalMs);
  }

  // Wait a bit for in-flight users to finish
  console.log(`  Waiting for ${activeUsers} active users to finish...`);
  while (activeUsers > 0) {
    await sleep(500);
  }

  console.log(`\nDone! Sent ${totalEventsSent} events total.`);
}

main();
