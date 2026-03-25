import { Card } from "@/components/shared";

export function AuditLogsEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">Записей не найдено</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Журнал действий пока пуст. Записи будут появляться по мере использования системы.
      </p>
    </Card>
  );
}
