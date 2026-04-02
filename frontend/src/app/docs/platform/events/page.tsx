export default function EventsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        События
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Страница «События» содержит список всех определений событий в
            проекте. Определения описывают типы действий пользователей (например,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              page_view
            </code>
            ,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              button_click
            </code>
            ,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              checkout_started
            </code>
            ). При отправке события через SDK его имя сопоставляется с этими
            определениями.
          </p>
        </div>

        <div>
          <h2
            id="creating-events"
            className="text-xl font-semibold text-zinc-900"
          >
            Создание определения события
          </h2>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>
              Нажмите <strong>Создать событие</strong> на панели инструментов.
            </li>
            <li>
              Введите уникальное имя события (рекомендуется нижний регистр с
              подчёркиваниями, например{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                order_placed
              </code>
              ).
            </li>
            <li>
              Добавьте необязательное описание, объясняющее, когда срабатывает
              это событие.
            </li>
            <li>Нажмите подтвердить для сохранения.</li>
          </ol>
        </div>

        <div>
          <h2
            id="events-table"
            className="text-xl font-semibold text-zinc-900"
          >
            Таблица событий
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            В таблице отображаются все определения событий с названием,
            описанием и датой создания. Используйте строку поиска для
            фильтрации по имени. Нажмите на строку, чтобы открыть диалог
            деталей.
          </p>
        </div>

        <div>
          <h2
            id="event-detail"
            className="text-xl font-semibold text-zinc-900"
          >
            Детали события
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Диалог деталей показывает полную информацию о событии: ID, название,
            описание и временные метки. Описание можно редактировать прямо из
            этого диалога.
          </p>
        </div>
      </section>
    </article>
  );
}
