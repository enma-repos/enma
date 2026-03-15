export type DocsSidebarItem = {
  title: string;
  href: string;
  items?: DocsSidebarItem[];
};

export type DocsSidebarSection = {
  title: string;
  items: DocsSidebarItem[];
};

export const docsSidebarSections: DocsSidebarSection[] = [
  {
    title: "Getting Started",
    items: [
      { title: "Introduction", href: "/docs" },
      { title: "Quick Start", href: "/docs/quick-start" },
      { title: "Installation", href: "/docs/installation" },
    ],
  },
  {
    title: "SDK",
    items: [
      { title: "Overview", href: "/docs/sdk/overview" },
      { title: "JavaScript SDK", href: "/docs/sdk/javascript" },
      { title: "Python SDK", href: "/docs/sdk/python" },
      { title: "REST API", href: "/docs/sdk/rest-api" },
    ],
  },
  {
    title: "Events",
    items: [
      { title: "Tracking Events", href: "/docs/events/tracking" },
      { title: "Event Definitions", href: "/docs/events/definitions" },
      { title: "Custom Properties", href: "/docs/events/properties" },
    ],
  },
  {
    title: "Processes",
    items: [
      { title: "Process Mining", href: "/docs/processes/mining" },
      { title: "Process Maps", href: "/docs/processes/maps" },
      { title: "Anomaly Detection", href: "/docs/processes/anomalies" },
    ],
  },
  {
    title: "Guides",
    items: [
      { title: "Authentication", href: "/docs/guides/authentication" },
      { title: "Organizations", href: "/docs/guides/organizations" },
      { title: "API Keys", href: "/docs/guides/api-keys" },
    ],
  },
];
