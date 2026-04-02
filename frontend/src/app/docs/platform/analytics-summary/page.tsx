export default function AnalyticsSummaryPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Аналитика — Сводка
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Страница «Сводка» — это представление по умолчанию при входе в
            проект. Здесь отображаются ключевые метрики и список самых популярных
            событий за выбранный период.
          </p>
        </div>

        <div>
          <h2
            id="metric-cards"
            className="text-xl font-semibold text-zinc-900"
          >
            Карточки метрик
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            В верхней части страницы расположен ряд карточек:
          </p>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>
              <strong>Всего событий</strong> — общее количество событий за
              выбранный период.
            </li>
            <li>
              <strong>Уникальные пользователи</strong> — число уникальных
              пользователей, вызвавших события.
            </li>
            <li>
              <strong>Типы событий</strong> — количество различных типов
              событий.
            </li>
          </ul>
          <p className="mt-3 leading-7 text-zinc-700">
            Каждая карточка содержит счётчик, который обновляется
            при изменении периода.
          </p>
        </div>

        <div>
          <h2
            id="popular-events"
            className="text-xl font-semibold text-zinc-900"
          >
            Популярные события
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Под метриками расположена таблица самых частых событий, отсортированных
            по количеству. Используйте её, чтобы определить наиболее популярные
            действия пользователей.
          </p>
        </div>

        <div>
          <h2
            id="date-range"
            className="text-xl font-semibold text-zinc-900"
          >
            Выбор периода
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            В правом верхнем углу находится переключатель периода. Выберите
            предустановленный диапазон (последние 7 дней, 30 дней) или задайте
            произвольные даты начала и конца. Все метрики и таблицы обновятся
            автоматически.
          </p>
        </div>

        <div>
          <h2
            id="process-filter"
            className="text-xl font-semibold text-zinc-900"
          >
            Фильтр по процессу
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Если вы определили процессы, используйте выпадающий список для
            фильтрации сводки по конкретному процессу. Это позволяет сравнить
            активность между различными рабочими потоками.
          </p>
        </div>
      </section>
    </article>
  );
}
