export default function DocsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Введение
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-6">
        <div>
          <h2 id="what-is-enma" className="text-xl font-semibold text-zinc-900">
            Что такое enma?
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Enma — это платформа процессного анализа (process mining), которая
            помогает понять, как пользователи взаимодействуют с вашим продуктом.
            Отслеживайте события, стройте карты процессов и выявляйте аномалии —
            всё в реальном времени.
          </p>
        </div>

        <div>
          <h2
            id="key-features"
            className="text-xl font-semibold text-zinc-900"
          >
            Основные возможности
          </h2>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>
              <span className="font-medium text-zinc-900">
                Отслеживание событий
              </span>{" "}
              — .NET SDK с автоматической группировкой и повторами
            </li>
            <li>
              <span className="font-medium text-zinc-900">Карты процессов</span>{" "}
              — автоматическая визуализация пользовательских потоков и переходов
            </li>
            <li>
              <span className="font-medium text-zinc-900">Аналитика</span>{" "}
              — сводные метрики, популярные события и анализ путей
            </li>
            <li>
              <span className="font-medium text-zinc-900">
                Командная работа
              </span>{" "}
              — организации, проекты и управление доступом
            </li>
            <li>
              <span className="font-medium text-zinc-900">
                Журнал действий
              </span>{" "}
              — полная история всех операций для аудита
            </li>
            <li>
              <span className="font-medium text-zinc-900">REST API</span>{" "}
              — программный доступ ко всем возможностям платформы
            </li>
          </ul>
        </div>

        <div>
          <h2
            id="how-it-works"
            className="text-xl font-semibold text-zinc-900"
          >
            Как это работает
          </h2>
          <ol className="mt-3 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>Создайте организацию и проект в дашборде enma.</li>
            <li>Зарегистрируйте приложение и сгенерируйте API-ключ.</li>
            <li>
              Подключите .NET SDK к вашему приложению и начните отправлять
              события.
            </li>
            <li>
              Определите процессы, чтобы сгруппировать связанные события в
              осмысленные рабочие потоки.
            </li>
            <li>
              Анализируйте поведение пользователей через сводные дашборды и
              интерактивные карты процессов.
            </li>
          </ol>
        </div>

        <div>
          <h2 id="next-steps" className="text-xl font-semibold text-zinc-900">
            Следующие шаги
          </h2>
          <ul className="mt-3 space-y-2 text-zinc-700">
            <li>
              <a
                href="/docs/quick-start"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Быстрый старт
              </a>{" "}
              — настройте проект и отправьте первые события за 5 минут
            </li>
            <li>
              <a
                href="/docs/sdk/overview"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Обзор SDK
              </a>{" "}
              — установка и настройка .NET SDK
            </li>
            <li>
              <a
                href="/docs/platform/organizations"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Руководство по платформе
              </a>{" "}
              — как пользоваться дашбордом
            </li>
          </ul>
        </div>
      </section>
    </article>
  );
}
