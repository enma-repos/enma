import { Card } from "@/components/shared";

export function OrganizationsEmpty() {
  return (
    <Card className="p-7">
      <h2 className="text-sm font-semibold text-zinc-900">Организации не найдены</h2>
      <p className="mt-2 text-sm text-zinc-500">
        Создайте первую организацию, чтобы начать работу.
      </p>
    </Card>
  );
}

