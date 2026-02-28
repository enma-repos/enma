import { OrganizationCard } from "@/components/app/organizations/organization-card";
import type { OrganizationDto } from "@/types/admin.types";

export type OrganizationsGridProps = {
  organizations: OrganizationDto[];
};

export function OrganizationsGrid({ organizations }: OrganizationsGridProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {organizations.map((organization) => (
        <OrganizationCard key={organization.id} organization={organization} />
      ))}
    </div>
  );
}
