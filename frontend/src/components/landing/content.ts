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

/* ─── How It Works ─── */
export type HowItWorksStep = {
  number: string;
  title: string;
  description: string;
};

export const howItWorksSteps: HowItWorksStep[] = [
  {
    number: "01",
    title: "Connect your data",
    description:
      "Integrate event logs from ERP, CRM, databases, or any system through pre-built connectors or our SDK.",
  },
  {
    number: "02",
    title: "Discover processes",
    description:
      "Enma automatically reconstructs real process flows from raw event data — no manual modeling required.",
  },
  {
    number: "03",
    title: "Analyze bottlenecks",
    description:
      "Identify delays, deviations, and rework loops with built-in anomaly detection and conformance checking.",
  },
  {
    number: "04",
    title: "Optimize & monitor",
    description:
      "Set KPIs, track improvements over time, and get alerts when processes drift from expected behavior.",
  },
];

/* ─── Use Cases ─── */
export type UseCase = {
  id: string;
  title: string;
  industry: string;
  description: string;
  highlights: string[];
};

export const useCases: UseCase[] = [
  {
    id: "manufacturing",
    title: "Order-to-delivery tracking",
    industry: "Manufacturing",
    description:
      "Trace every step from purchase order to shipment. Detect production bottlenecks and reduce lead times across your supply chain.",
    highlights: ["30% faster lead times", "Real-time WIP visibility", "Deviation alerts"],
  },
  {
    id: "finance",
    title: "Audit & compliance",
    industry: "Finance",
    description:
      "Automatically verify that financial processes follow regulatory requirements. Generate audit trails without manual effort.",
    highlights: ["Automated audit trails", "SOX compliance", "Segregation of duties"],
  },
  {
    id: "it",
    title: "Incident management",
    industry: "IT Operations",
    description:
      "Map your ITSM workflows end-to-end. Find resolution delays, escalation patterns, and SLA breaches before they impact users.",
    highlights: ["MTTR reduction", "SLA monitoring", "Escalation analysis"],
  },
  {
    id: "logistics",
    title: "Supply chain optimization",
    industry: "Logistics",
    description:
      "Gain full visibility into procurement, warehousing, and distribution. Optimize routes and reduce inventory holding costs.",
    highlights: ["Route optimization", "Inventory insights", "Carrier benchmarking"],
  },
];

/* ─── Metrics ─── */
export type MetricItem = {
  value: number;
  suffix: string;
  label: string;
};

export const metrics: MetricItem[] = [
  { value: 40, suffix: "%", label: "Reduction in process cycle time" },
  { value: 10, suffix: "x", label: "Faster than manual audit" },
  { value: 99, suffix: ".9%", label: "Event coverage accuracy" },
  { value: 3, suffix: " min", label: "Average time to first insight" },
];

/* ─── Comparison ─── */
export type ComparisonRow = {
  aspect: string;
  without: string;
  with: string;
};

export const comparisonRows: ComparisonRow[] = [
  {
    aspect: "Process discovery",
    without: "Manual interviews & workshops",
    with: "Automatic from event logs",
  },
  {
    aspect: "Bottleneck detection",
    without: "Guesswork & anecdotes",
    with: "Data-driven, real-time alerts",
  },
  {
    aspect: "Compliance audit",
    without: "Weeks of manual checks",
    with: "Continuous automated verification",
  },
  {
    aspect: "Reporting",
    without: "Static Excel spreadsheets",
    with: "Live dashboards & custom KPIs",
  },
  {
    aspect: "Time to insight",
    without: "Days to weeks",
    with: "Minutes",
  },
];

/* ─── Integrations ─── */
export type Integration = {
  name: string;
  category: string;
};

export const integrations: Integration[] = [
  { name: "SAP", category: "ERP" },
  { name: "Oracle", category: "ERP" },
  { name: "1C", category: "ERP" },
  { name: "Salesforce", category: "CRM" },
  { name: "Jira", category: "Project" },
  { name: "ServiceNow", category: "ITSM" },
  { name: "Dynamics 365", category: "ERP" },
  { name: "PostgreSQL", category: "Database" },
  { name: "Kafka", category: "Streaming" },
  { name: "REST API", category: "Custom" },
  { name: "Snowflake", category: "Data" },
  { name: "CSV / XES", category: "Files" },
];

/* ─── Testimonials ─── */
export type Testimonial = {
  quote: string;
  name: string;
  role: string;
  company: string;
};

export const testimonials: Testimonial[] = [
  {
    quote:
      "Enma revealed bottlenecks in our order fulfillment that we never knew existed. We cut our average cycle time by 35% in the first quarter.",
    name: "Anna Petrov",
    role: "VP of Operations",
    company: "NovaTech Industries",
  },
  {
    quote:
      "What used to take our audit team three weeks now happens continuously in the background. Compliance has never been this effortless.",
    name: "Markus Chen",
    role: "Head of Internal Audit",
    company: "Meridian Financial",
  },
  {
    quote:
      "The SDK integration was dead simple — we had production events flowing in under an hour. The real-time process maps are a game changer.",
    name: "Leyla Osman",
    role: "Engineering Lead",
    company: "Streamline Logistics",
  },
];

/* ─── FAQ ─── */
export type FaqItem = {
  question: string;
  answer: string;
};

export const faqItems: FaqItem[] = [
  {
    question: "What is process mining?",
    answer:
      "Process mining is a data-driven technique that uses event logs from your IT systems to reconstruct, visualize, and analyze real business processes. Unlike traditional process modeling, it shows you what actually happens — not what people think happens.",
  },
  {
    question: "What data do I need to get started?",
    answer:
      "You need an event log with three essential columns: a case ID (e.g. order number), an activity name (e.g. 'order placed'), and a timestamp. Most ERP, CRM, and ticketing systems already record this data.",
  },
  {
    question: "How does enma integrate with my existing systems?",
    answer:
      "We provide an official .NET SDK for seamless integration with any .NET application. For all other technology stacks — Python, Java, Node.js, Go, and beyond — enma offers a REST API that lets you send events from virtually any environment. You choose the format and the data you send; we handle the rest.",
  },
  {
    question: "Is my data secure?",
    answer:
      "Your data security is our top priority. With enma, you have complete control over what information you send — you decide exactly which events and attributes to track, so no sensitive data ever leaves your systems unless you explicitly choose to include it. All data is transmitted over secure channels, access is governed by role-based permissions with full audit logging, and your information is never shared with third parties. You stay in the driver's seat at every step.",
  },
  {
    question: "How long does it take to see results?",
    answer:
      "Most teams see their first process maps and insights within minutes of connecting their data. Meaningful optimization opportunities typically surface within the first week of use.",
  },
  {
    question: "Can I try enma for free?",
    answer:
      "Absolutely — right now enma is completely free. We are in an active development phase, and the free plan is currently the only available tier, giving you full, unrestricted access to every feature with no limitations. No credit card, no trials, no hidden caps. Jump in, explore everything enma has to offer, and help us shape the product as we grow together.",
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
