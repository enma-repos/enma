"use client";

import {
  ReactFlow,
  ReactFlowProvider,
  Background,
  BackgroundVariant,
  useNodesState,
  useEdgesState,
  type NodeTypes,
  type EdgeTypes,
  type Node,
  type Edge,
  Position,
  MarkerType,
} from "@xyflow/react";
import "@xyflow/react/dist/style.css";

import { EventNode } from "@/components/app/analytics/flow/event-node";
import { TransitionEdge } from "@/components/app/analytics/flow/transition-edge";
import { COLORS } from "@/lib/analytics/flow-constants";

const nodeTypes: NodeTypes = { eventNode: EventNode };
const edgeTypes: EdgeTypes = { transitionEdge: TransitionEdge };

/* ── Mock data ── */

const mockNodes: Node[] = [
  {
    id: "order_placed",
    type: "eventNode",
    position: { x: 250, y: 10 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "order_placed", visits: 1247, entries: 1247, exits: 0, uniqueChains: 980, sizeFactor: 1 },
  },
  {
    id: "payment_received",
    type: "eventNode",
    position: { x: 80, y: 170 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "payment_received", visits: 1185, entries: 0, exits: 0, uniqueChains: 940, sizeFactor: 0.9 },
  },
  {
    id: "fraud_review",
    type: "eventNode",
    position: { x: 440, y: 170 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "fraud_review", visits: 187, entries: 0, exits: 42, uniqueChains: 150, sizeFactor: 0.15 },
  },
  {
    id: "processing",
    type: "eventNode",
    position: { x: 250, y: 340 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "processing", visits: 1102, entries: 0, exits: 0, uniqueChains: 870, sizeFactor: 0.85 },
  },
  {
    id: "quality_check",
    type: "eventNode",
    position: { x: 80, y: 510 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "quality_check", visits: 1065, entries: 0, exits: 37, uniqueChains: 840, sizeFactor: 0.82 },
  },
  {
    id: "back_order",
    type: "eventNode",
    position: { x: 440, y: 510 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "back_order", visits: 83, entries: 0, exits: 0, uniqueChains: 70, sizeFactor: 0.07 },
  },
  {
    id: "shipping",
    type: "eventNode",
    position: { x: 130, y: 680 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "shipping", visits: 1019, entries: 0, exits: 0, uniqueChains: 820, sizeFactor: 0.8 },
  },
  {
    id: "delivered",
    type: "eventNode",
    position: { x: 250, y: 850 },
    sourcePosition: Position.Bottom,
    targetPosition: Position.Top,
    data: { eventName: "delivered", visits: 998, entries: 0, exits: 998, uniqueChains: 800, sizeFactor: 0.78 },
  },
];

const palette = COLORS.edgePalette;

const e = (id: string, source: string, target: string, transitions: number, colorIdx: number, width: number): Edge => ({
  id,
  source,
  target,
  type: "transitionEdge",
  data: { transitions },
  style: { strokeWidth: width, stroke: palette[colorIdx] },
  markerEnd: { type: MarkerType.ArrowClosed, width: 16, height: 16, color: palette[colorIdx] },
});

const mockEdges: Edge[] = [
  e("order_placed__payment_received", "order_placed", "payment_received", 1185, 0, 6),
  e("order_placed__fraud_review", "order_placed", "fraud_review", 187, 2, 2),
  e("fraud_review__processing", "fraud_review", "processing", 145, 3, 1.5),
  e("payment_received__processing", "payment_received", "processing", 1102, 1, 5.5),
  e("processing__quality_check", "processing", "quality_check", 1065, 4, 5),
  e("processing__back_order", "processing", "back_order", 83, 7, 1.5),
  e("back_order__quality_check", "back_order", "quality_check", 83, 8, 1.5),
  e("quality_check__shipping", "quality_check", "shipping", 1019, 5, 4.5),
  e("shipping__delivered", "shipping", "delivered", 998, 0, 4),
];

/* ── Canvas ── */

function DemoCanvas() {
  const [nodes, , onNodesChange] = useNodesState(mockNodes);
  const [edges, , onEdgesChange] = useEdgesState(mockEdges);

  return (
    <ReactFlow
      nodes={nodes}
      edges={edges}
      onNodesChange={onNodesChange}
      onEdgesChange={onEdgesChange}
      nodeTypes={nodeTypes}
      edgeTypes={edgeTypes}
      fitView
      fitViewOptions={{ padding: 0.15 }}
      nodesDraggable
      nodesConnectable={false}
      elementsSelectable={false}
      panOnDrag={false}
      panOnScroll={false}
      autoPanOnNodeDrag={false}
      zoomOnScroll={false}
      zoomOnPinch={false}
      zoomOnDoubleClick={false}
      minZoom={0.95}
      maxZoom={0.95}
      proOptions={{ hideAttribution: true }}
    >
      <Background variant={BackgroundVariant.Dots} gap={20} size={1.5} color="#d4d4d8" />
    </ReactFlow>
  );
}

/* ── Section ── */

export function ProductPreviewSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-12 sm:px-8 sm:py-14">
      <div className="overflow-hidden rounded-xl border border-zinc-200 shadow-lg transition-shadow duration-300 hover:shadow-xl">
        {/* Browser chrome */}
        <div className="flex items-center gap-2 border-b border-zinc-200 bg-zinc-100 px-4 py-3">
          <span className="h-3 w-3 rounded-full bg-[#ff5f57]" />
          <span className="h-3 w-3 rounded-full bg-[#febc2e]" />
          <span className="h-3 w-3 rounded-full bg-[#28c840]" />
          <div className="ml-3 flex-1">
            <div className="mx-auto max-w-sm rounded-md bg-white/80 px-4 py-1.5 text-center text-xs text-zinc-400">
              enma.tech/app
            </div>
          </div>
          <div className="w-[52px]" />
        </div>

        {/* Dashboard content */}
        <div className="bg-zinc-50 p-4 sm:p-6">
          <div className="grid gap-4 lg:grid-cols-[1fr_260px]">
            {/* Interactive process map */}
            <div className="h-[900px] overflow-hidden rounded-lg border border-zinc-200 bg-white">
              <ReactFlowProvider>
                <DemoCanvas />
              </ReactFlowProvider>
            </div>

            {/* Sidebar stats */}
            <div className="flex flex-col gap-3">
              {[
                { label: "Cycle time", value: "2.4h", change: "-18%", positive: true },
                { label: "Cases today", value: "1,247", change: "+12%", positive: true },
                { label: "Deviations", value: "8.3%", change: "+2.1%", positive: false },
                { label: "Conformance", value: "91.7%", change: "+0.5%", positive: true },
              ].map((stat) => (
                <div
                  key={stat.label}
                  className="rounded-lg border border-zinc-200 bg-white px-4 py-3"
                >
                  <p className="text-[11px] font-medium uppercase tracking-wider text-zinc-400">
                    {stat.label}
                  </p>
                  <div className="mt-1 flex items-baseline gap-2">
                    <span className="text-xl font-semibold text-zinc-900">
                      {stat.value}
                    </span>
                    <span
                      className={`text-xs font-medium ${
                        stat.positive ? "text-emerald-600" : "text-red-500"
                      }`}
                    >
                      {stat.change}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
