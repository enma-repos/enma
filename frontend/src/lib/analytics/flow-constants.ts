/** Visual constants for the flow graph */

/** Default node dimensions */
export const NODE_WIDTH = 220;
export const NODE_HEIGHT = 80;

/** Spacing between nodes in dagre layout */
export const NODE_SEP = 120;
export const RANK_SEP = 120;

/** Edge thickness range (px) */
export const EDGE_MIN_WIDTH = 1.5;
export const EDGE_MAX_WIDTH = 8;

/** Threshold: edges with fewer transitions are hidden when filter is on */
export const DEFAULT_MIN_TRANSITIONS = 0;

/** Colors */
export const COLORS = {
  entry: "#10b981",     // emerald-500
  exit: "#ef4444",      // red-500
  node: "#ffffff",
  nodeBorder: "#e4e4e7", // zinc-200
  nodeHover: "#f4f4f5",  // zinc-100
  edgeDefault: "#a1a1aa", // zinc-400
  edgeHigh: "#6366f1",    // indigo-500
  edgePalette: [
    "#6366f1", // indigo-500
    "#8b5cf6", // violet-500
    "#ec4899", // pink-500
    "#f97316", // orange-500
    "#14b8a6", // teal-500
    "#3b82f6", // blue-500
    "#eab308", // yellow-500
    "#10b981", // emerald-500
    "#f43f5e", // rose-500
    "#06b6d4", // cyan-500
  ],
  selected: "#6366f1",    // indigo-500
} as const;
