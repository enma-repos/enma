import { Card } from "@/components/shared";

export function ProcessesEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">Процессы не найдены</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Создайте первый процесс, чтобы начать работу.
      </p>
    </Card>
  );
}
