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
    title: "Начало работы",
    items: [
      { title: "Введение", href: "/docs" },
      { title: "Быстрый старт", href: "/docs/quick-start" },
    ],
  },
  {
    title: "Платформа",
    items: [
      { title: "Организации и проекты", href: "/docs/platform/organizations" },
      { title: "Аналитика — Сводка", href: "/docs/platform/analytics-summary" },
      { title: "Аналитика — Пути", href: "/docs/platform/analytics-paths" },
      { title: "События", href: "/docs/platform/events" },
      { title: "Приложения и API-ключи", href: "/docs/platform/apps" },
      { title: "Процессы", href: "/docs/platform/processes" },
      { title: "Команда", href: "/docs/platform/team" },
      { title: "Журнал действий", href: "/docs/platform/logs" },
    ],
  },
  {
    title: "SDK",
    items: [
      { title: "Обзор", href: "/docs/sdk/overview" },
      { title: ".NET SDK", href: "/docs/sdk/dotnet" },
      { title: "Конфигурация", href: "/docs/sdk/configuration" },
      { title: "ASP.NET Core", href: "/docs/sdk/aspnet" },
      { title: "REST API", href: "/docs/sdk/rest-api" },
    ],
  },
];
