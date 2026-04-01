import dagre from "dagre";
import { MarkerType, Position, type Edge, type Node } from "@xyflow/react";
import type { FlowGraphDto } from "@/types/analytics.types";
import {
  NODE_WIDTH,
  NODE_HEIGHT,
  NODE_SEP,
  RANK_SEP,
  EDGE_MIN_WIDTH,
  EDGE_MAX_WIDTH,
  COLORS,
} from "./flow-constants";

export interface EventNodeData extends Record<string, unknown> {
  eventName: string;
  visits: number;
  entries: number;
  exits: number;
  uniqueChains: number;
  /** Normalized size factor 0..1 based on visits */
  sizeFactor: number;
}

/**
 * Converts backend FlowGraphDto into React Flow nodes + edges
 * with dagre-based auto layout.
 */
export function mapFlowToReactFlow(
  dto: FlowGraphDto,
  minTransitions = 0,
): { nodes: Node<EventNodeData>[]; edges: Edge[] } {
  if (!dto.nodes.length) return { nodes: [], edges: [] };

  const maxVisits = Math.max(...dto.nodes.map((n) => n.visits), 1);

  // --- build dagre graph ------------------------------------------------
  const g = new dagre.graphlib.Graph();
  g.setGraph({ rankdir: "TB", nodesep: NODE_SEP, ranksep: RANK_SEP });
  g.setDefaultEdgeLabel(() => ({}));

  for (const n of dto.nodes) {
    g.setNode(n.eventName, { width: NODE_WIDTH, height: NODE_HEIGHT });
  }

  const nodeNames = new Set(dto.nodes.map((n) => n.eventName));

  const filteredEdges = dto.edges.filter(
    (e) =>
      e.transitions >= minTransitions &&
      e.fromEvent !== e.toEvent &&
      nodeNames.has(e.fromEvent) &&
      nodeNames.has(e.toEvent),
  );

  for (const e of filteredEdges) {
    g.setEdge(e.fromEvent, e.toEvent);
  }

  dagre.layout(g);

  // --- center solo nodes per rank ----------------------------------------
  // Group nodes by their Y (rank level). If a rank has a single node,
  // center it to the median X of all nodes so it doesn't stick to the left.
  const posMap = new Map<string, { x: number; y: number }>();
  for (const n of dto.nodes) {
    const pos = g.node(n.eventName);
    if (pos) posMap.set(n.eventName, { x: pos.x, y: pos.y });
  }

  const rankGroups = new Map<number, string[]>();
  for (const [name, pos] of posMap) {
    const ry = Math.round(pos.y);
    if (!rankGroups.has(ry)) rankGroups.set(ry, []);
    rankGroups.get(ry)!.push(name);
  }

  const allXs = [...posMap.values()].map((p) => p.x).sort((a, b) => a - b);
  const medianX = allXs[Math.floor(allXs.length / 2)] ?? 0;

  for (const [, members] of rankGroups) {
    if (members.length === 1) {
      const pos = posMap.get(members[0])!;
      pos.x = medianX;
    }
  }

  // --- map nodes --------------------------------------------------------
  const nodes: Node<EventNodeData>[] = dto.nodes.map((n) => {
    const pos = posMap.get(n.eventName) ?? { x: 0, y: 0 };
    return {
      id: n.eventName,
      type: "eventNode",
      position: {
        x: pos.x - NODE_WIDTH / 2,
        y: pos.y - NODE_HEIGHT / 2,
      },
      data: {
        eventName: n.eventName,
        visits: n.visits,
        entries: n.entries,
        exits: n.exits,
        uniqueChains: n.uniqueChains,
        sizeFactor: n.visits / maxVisits,
      },
      sourcePosition: Position.Bottom,
      targetPosition: Position.Top,
    };
  });

  // --- map edges --------------------------------------------------------
  const maxTransitions = Math.max(
    ...filteredEdges.map((e) => e.transitions),
    1,
  );

  // Assign a stable color per source node
  const sourceNodes = [...new Set(filteredEdges.map((e) => e.fromEvent))];
  const sourceColorMap = new Map<string, string>();
  sourceNodes.forEach((name, i) => {
    sourceColorMap.set(name, COLORS.edgePalette[i % COLORS.edgePalette.length]);
  });

  const edges: Edge[] = filteredEdges.map((e) => {
    const ratio = e.transitions / maxTransitions;
    const strokeWidth =
      EDGE_MIN_WIDTH + ratio * (EDGE_MAX_WIDTH - EDGE_MIN_WIDTH);
    const color = sourceColorMap.get(e.fromEvent) ?? COLORS.edgeDefault;

    return {
      id: `${e.fromEvent}__${e.toEvent}`,
      source: e.fromEvent,
      target: e.toEvent,
      type: "transitionEdge",
      animated: false,
      data: {
        transitions: e.transitions,
        uniqueChains: e.uniqueChains,
      },
      style: {
        strokeWidth,
        stroke: color,
      },
      markerEnd: {
        type: MarkerType.ArrowClosed,
        width: 16,
        height: 16,
        color,
      },
    };
  });

  return { nodes, edges };
}
