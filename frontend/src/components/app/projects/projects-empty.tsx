import { Card } from "@/components/shared";

export function ProjectsEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">Проекты не найдены</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Создайте первый проект, чтобы начать работу.
      </p>
    </Card>
  );
}

