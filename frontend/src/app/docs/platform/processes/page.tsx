export default function ProcessesPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Процессы
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Процесс — это именованная коллекция определений событий,
            представляющая рабочий поток или пользовательский маршрут. Например,
            процесс{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              checkout
            </code>{" "}
            может включать события{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              cart_viewed
            </code>
            ,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              payment_entered
            </code>{" "}
            и{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              order_confirmed
            </code>
            .
          </p>
        </div>

        <div>
          <h2
            id="creating-process"
            className="text-xl font-semibold text-zinc-900"
          >
            Создание процесса
          </h2>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>
              Перейдите в раздел <strong>Процессы</strong> в боковом меню.
            </li>
            <li>
              Нажмите <strong>Создать процесс</strong>.
            </li>
            <li>Введите название и необязательное описание.</li>
            <li>Подтвердите сохранение.</li>
          </ol>
        </div>

        <div>
          <h2
            id="processes-table"
            className="text-xl font-semibold text-zinc-900"
          >
            Таблица процессов
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            В таблице перечислены все определения процессов с названием,
            описанием и количеством связанных событий. Нажмите на строку, чтобы
            открыть диалог деталей.
          </p>
        </div>

        <div>
          <h2
            id="process-analytics"
            className="text-xl font-semibold text-zinc-900"
          >
            Аналитика процессов
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            После определения процесса вы можете фильтровать страницы{" "}
            <a
              href="/docs/platform/analytics-summary"
              className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
            >
              Сводка
            </a>{" "}
            и{" "}
            <a
              href="/docs/platform/analytics-paths"
              className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
            >
              Пути
            </a>{" "}
            по этому процессу. Визуализация покажет только события внутри
            выбранного рабочего потока.
          </p>
        </div>

        <div>
          <h2
            id="process-keys"
            className="text-xl font-semibold text-zinc-900"
          >
            Ключи процессов в SDK
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            При отправке событий через SDK можно указать ключ(-и) процесса(-ов), чтобы
            связать события с конкретным экземпляром рабочего потока:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`enma.Track("cart.viewed", e =>
{
    e.Actor.UserId = "user-123";
    e.ProcessKey(Guid.Parse("checkout-process-def-id"), "order-456");
});`}</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Все события с одинаковым ключом процесса группируются вместе, что
            позволяет отслеживать отдельные экземпляры рабочих потоков.
          </p>
        </div>
      </section>
    </article>
  );
}
