export type NavigationLink = {
  href: string;
  label: string;
  highlighted?: boolean;
};

export type FeatureItem = {
  title: string;
  description: string;
};

export type FooterColumn = {
  title: string;
  links: string[];
};

export const navigationLinks: NavigationLink[] = [
  { href: "/product", label: "Product",},
  { href: "/sdk", label: "SDK" },
  { href: "/pricing", label: "Pricing" },
  { href: "/contact", label: "Contact" }
];

export const featureItems: FeatureItem[] = [
  {
    title: "Data connectors",
    description:
      "Connect events from product analytics, CRM and internal systems in one pipeline.",
  },
  {
    title: "Live process maps",
    description:
      "Visualize every user path in real time to detect bottlenecks before they impact growth.",
  },
  {
    title: "Anomaly detection",
    description:
      "Get automatic alerts when funnel behavior changes and requires immediate action.",
  },
  {
    title: "Team workspaces",
    description:
      "Share dashboards and annotations with product, operations and leadership teams.",
  },
  {
    title: "Custom metrics",
    description:
      "Define KPIs for each process stage and monitor trends across segments and cohorts.",
  },
  {
    title: "Enterprise security",
    description:
      "Control access with role-based permissions, audit logs and secure key rotation.",
  },
];

export const footerColumns: FooterColumn[] = [
  {
    title: "Use cases",
    links: [
      "UI design",
      "UX design",
      "Wireframing",
      "Diagramming",
      "Brainstorming",
      "Online whiteboard",
      "Team collaboration",
    ],
  },
  {
    title: "Explore",
    links: [
      "Design",
      "Prototyping",
      "Development features",
      "Design systems",
      "Collaboration features",
      "Design process",
      "FigJam",
    ],
  },
  {
    title: "Resources",
    links: [
      "Blog",
      "Best practices",
      "Colors",
      "Color wheel",
      "Support",
      "Developers",
      "Resource library",
    ],
  },
];
