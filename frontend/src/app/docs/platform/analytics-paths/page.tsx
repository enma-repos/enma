export default function AnalyticsPathsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Аналитика — Пути
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Страница «Пути» отображает интерактивный граф переходов между
            событиями. Каждый узел — тип события, каждое ребро показывает, как
            часто пользователи переходили от одного события к другому.
          </p>
        </div>

        <div>
          <h2
            id="reading-the-graph"
            className="text-xl font-semibold text-zinc-900"
          >
            Чтение графа
          </h2>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>
              <strong>Узлы</strong> — каждый блок представляет тип события.
              Метка содержит название и общее количество.
            </li>
            <li>
              <strong>Рёбра</strong> — стрелки между узлами показывают переходы.
              Метка отображает количество и процент.
            </li>

          </ul>
        </div>

        <div>
          <h2
            id="interaction"
            className="text-xl font-semibold text-zinc-900"
          >
            Взаимодействие с графом
          </h2>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>
              <strong>Панорамирование и масштаб</strong> — перетаскивайте холст
              для перемещения; прокручивайте колёсико для масштабирования.
            </li>
            <li>
              <strong>Выбор узла</strong> — нажмите на узел, чтобы открыть
              панель деталей справа. В ней отображаются входящие и исходящие
              переходы с количеством и процентами.
            </li>
            <li>
              <strong>Панель инструментов</strong> — кнопки для подгонки графа
              под область видимости, масштабирования и сброса представления.
            </li>
          </ul>
        </div>

        <div>
          <h2
            id="node-detail"
            className="text-xl font-semibold text-zinc-900"
          >
            Панель деталей узла
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            При выборе узла открывается боковая панель, содержащая:
          </p>
          <ul className="mt-3 list-disc space-y-1 pl-6 text-zinc-700">
            <li>Название события и общее количество</li>
            <li>
              Входящие переходы — какие события предшествуют текущему и как
              часто
            </li>
            <li>
              Исходящие переходы — какие события следуют за текущим и их
              частоту
            </li>
          </ul>
        </div>

        <div>
          <h2 id="filters" className="text-xl font-semibold text-zinc-900">
            Фильтры
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Как и на странице «Сводка», вы можете фильтровать по периоду и
            процессу. Граф автоматически перестраивается при изменении фильтров.
          </p>
        </div>

        <div>
          <h2
            id="path-filtering"
            className="text-xl font-semibold text-zinc-900"
          >
            Фильтрация путей
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Нажмите на узел, чтобы отфильтровать граф и показать только пути,
            достижимые из этого узла. Используется обход в ширину (BFS) для
            выделения релевантного подграфа, что упрощает отслеживание конкретных
            пользовательских маршрутов.
          </p>
        </div>
      </section>
    </article>
  );
}
