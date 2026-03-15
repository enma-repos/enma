"use client";

import { useCallback, useMemo, useState } from "react";
import {
  ReactFlow,
  Background,
  BackgroundVariant,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  type NodeMouseHandler,
  type NodeTypes,
  type EdgeTypes,
} from "@xyflow/react";
import "@xyflow/react/dist/style.css";

import type { FlowGraphDto } from "@/types/analytics.types";
import { mapFlowToReactFlow } from "@/lib/analytics/map-flow-to-react-flow";
import { COLORS } from "@/lib/analytics/flow-constants";
import { EventNode } from "./event-node";
import { TransitionEdge } from "./transition-edge";
import { FlowEmptyState } from "./flow-empty-state";

const nodeTypes: NodeTypes = { eventNode: EventNode };
const edgeTypes: EdgeTypes = { transitionEdge: TransitionEdge };

interface Props {
  data: FlowGraphDto;
  minTransitions?: number;
  readonly?: boolean;
  onNodeSelect?: (eventName: string | null) => void;
}

export function FlowCanvas({
  data,
  minTransitions = 0,
  readonly = false,
  onNodeSelect,
}: Props) {
  const { nodes: initialNodes, edges: initialEdges } = useMemo(
    () => mapFlowToReactFlow(data, minTransitions),
    [data, minTransitions],
  );

  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const [selectedNode, setSelectedNode] = useState<string | null>(null);

  // sync when data or filter changes
  useMemo(() => {
    setNodes(initialNodes);
    setEdges(initialEdges);
  }, [initialNodes, initialEdges, setNodes, setEdges]);

  const handleNodeClick: NodeMouseHandler = useCallback(
    (_event, node) => {
      if (readonly) return;
      const name = node.id;
      const next = name === selectedNode ? null : name;
      setSelectedNode(next);
      onNodeSelect?.(next);
    },
    [readonly, selectedNode, onNodeSelect],
  );

  const handlePaneClick = useCallback(() => {
    if (readonly) return;
    setSelectedNode(null);
    onNodeSelect?.(null);
  }, [readonly, onNodeSelect]);

  if (!initialNodes.length) {
    return <FlowEmptyState />;
  }

  return (
    <div className="h-full w-full rounded-2xl border border-zinc-200 bg-white">
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={readonly ? undefined : onNodesChange}
        onEdgesChange={readonly ? undefined : onEdgesChange}
        onNodeClick={handleNodeClick}
        onPaneClick={handlePaneClick}
        nodeTypes={nodeTypes}
        edgeTypes={edgeTypes}
        defaultViewport={{ x: 50, y: 20, zoom: 0.9 }}
        nodesDraggable={!readonly}
        nodesConnectable={false}
        elementsSelectable={!readonly}
        panOnDrag
        zoomOnScroll
        minZoom={0.2}
        maxZoom={2}
        proOptions={{ hideAttribution: true }}
      >
        <Background variant={BackgroundVariant.Dots} gap={20} size={1.5} color="#d4d4d8" />
        {!readonly && (
          <>
            <Controls
              showInteractive={false}
              className="!rounded-xl !border-zinc-200 !shadow-sm"
            />
            <MiniMap
              nodeColor={() => COLORS.nodeBorder}
              maskColor="rgba(0,0,0,0.08)"
              className="!rounded-xl !border-zinc-200 !shadow-sm"
            />
          </>
        )}
      </ReactFlow>
    </div>
  );
}
