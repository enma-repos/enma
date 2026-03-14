import { Card } from "@/components/shared";

export function AppsEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">Приложения не найдены</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Создайте первое приложение, чтобы начать работу.
      </p>
    </Card>
  );
}
