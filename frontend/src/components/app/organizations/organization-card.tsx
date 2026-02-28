import Link from "next/link";
import { Card, IconBox, cn } from "@/components/shared";
import type { OrganizationDto } from "@/types/admin.types";

function formatDate(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(date);
}

export type OrganizationCardProps = {
  organization: OrganizationDto;
};

export function OrganizationCard({ organization }: OrganizationCardProps) {
  return (
    <Card className="transition-shadow hover:shadow-md">
      <Link
        href={`/app/organizations/${encodeURIComponent(organization.slug)}/projects`}
        className={cn(
          "block rounded-2xl p-5 focus:outline-none focus:ring-2 focus:ring-zinc-900/10",
        )}
        aria-label={`Open organization ${organization.name}`}
      >
        <div className="flex items-start justify-between gap-4">
          <div className="grid h-12 w-12 place-items-center rounded-xl bg-blue-50 text-blue-600">
            <IconBox className="h-6 w-6" aria-hidden="true" />
          </div>
          <div className="text-xs text-zinc-400">{organization.slug}</div>
        </div>

        <div className="mt-4">
          <div className="truncate text-base font-semibold text-zinc-900">
            {organization.name}
          </div>
          <div className="mt-1 text-xs text-zinc-400">
            {formatDate(organization.createdAt)}
          </div>
        </div>
      </Link>
    </Card>
  );
}
