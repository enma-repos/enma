"use client";

import { useState } from "react";
import { ReactFlowProvider } from "@xyflow/react";

import { useFlowGraph } from "@/hooks/useFlowGraph";
import { useEventDetail } from "@/hooks/useEventDetail";
import { FlowCanvas } from "./flow-canvas";
import { FlowToolbar } from "./flow-toolbar";
import { NodeDetailPanel } from "./node-detail-panel";
import { FlowEmptyState } from "./flow-empty-state";

interface Props {
  organizationId: string;
  projectId: string;
  processDefinitionId: string | null;
  from: string;
  to: string;
  readonly?: boolean;
}

export function FlowGraph({
  organizationId,
  projectId,
  processDefinitionId,
  from,
  to,
  readonly = false,
}: Props) {
  const [minTransitions, setMinTransitions] = useState(0);
  const [selectedEventName, setSelectedEventName] = useState<string | null>(
    null,
  );

  const {
    data: flowData,
    isLoading: flowLoading,
    isError: flowError,
  } = useFlowGraph(
    organizationId,
    projectId,
    processDefinitionId,
    from,
    to,
  );

  const { data: eventDetail, isLoading: detailLoading } = useEventDetail(
    organizationId,
    projectId,
    processDefinitionId,
    selectedEventName,
    from,
    to,
  );

  // --- loading skeleton ---
  if (flowLoading) {
    return (
      <div className="grid h-[850px] place-items-center rounded-2xl border border-zinc-200 bg-zinc-50">
        <div className="flex flex-col items-center gap-2">
          <div className="h-6 w-6 animate-spin rounded-full border-2 border-zinc-300 border-t-zinc-600" />
          <span className="text-sm text-zinc-400">Загрузка графа...</span>
        </div>
      </div>
    );
  }

  if (flowError || !flowData) {
    return <FlowEmptyState message="Не удалось загрузить данные графа" />;
  }

  // --- readonly (compact) mode for summary page ---
  if (readonly) {
    return (
      <ReactFlowProvider>
        <div className="h-[680px]">
          <FlowCanvas data={flowData} readonly />
        </div>
      </ReactFlowProvider>
    );
  }

  // --- full interactive mode ---
  return (
    <ReactFlowProvider>
      <div className="flex flex-col gap-4">
        <FlowToolbar
          minTransitions={minTransitions}
          onMinTransitionsChange={setMinTransitions}
        />

        <div className="flex gap-4" style={{ height: 850 }}>
          <div className="flex-1">
            <FlowCanvas
              data={flowData}
              minTransitions={minTransitions}
              onNodeSelect={setSelectedEventName}
            />
          </div>

          {selectedEventName && (
            <NodeDetailPanel
              detail={eventDetail}
              isLoading={detailLoading}
              onClose={() => setSelectedEventName(null)}
            />
          )}
        </div>
      </div>
    </ReactFlowProvider>
  );
}
