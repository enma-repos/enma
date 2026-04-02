export default function LogsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Журнал действий
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Страница «Журнал действий» содержит хронологическую запись всех
            значимых операций в проекте — создание, обновление и удаление
            ресурсов: приложений, событий, процессов и участников команды.
          </p>
        </div>

        <div>
          <h2
            id="logs-table"
            className="text-xl font-semibold text-zinc-900"
          >
            Таблица логов
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Каждая запись в таблице содержит:
          </p>
          <ul className="mt-3 list-disc space-y-1 pl-6 text-zinc-700">
            <li>
              <strong>Действие</strong> — что произошло (создание, обновление,
              удаление).
            </li>
            <li>
              <strong>Ресурс</strong> — тип и название затронутого ресурса.
            </li>
            <li>
              <strong>Пользователь</strong> — кто выполнил действие.
            </li>
            <li>
              <strong>Время</strong> — когда это произошло.
            </li>
          </ul>
        </div>

        <div>
          <h2 id="filtering" className="text-xl font-semibold text-zinc-900">
            Фильтрация
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Используйте панель инструментов для фильтрации логов по типу
            действия, типу ресурса или пользователю. Строка поиска позволяет
            искать по всем полям. Комбинируйте фильтры для сужения результатов.
          </p>
        </div>

        <div>
          <h2
            id="log-detail"
            className="text-xl font-semibold text-zinc-900"
          >
            Детали записи
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Нажмите на запись, чтобы открыть диалог деталей с полным контекстом
            действия, включая значения до и после для обновлений и полные
            метаданные затронутого ресурса.
          </p>
        </div>
      </section>
    </article>
  );
}
