import { Card } from "@/components/shared";

export function EventsEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">События не найдены</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Создайте первое событие, чтобы начать работу.
      </p>
    </Card>
  );
}
