export default function OrganizationsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Организации и проекты
      </h1>


      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2
            id="organizations"
            className="text-xl font-semibold text-zinc-900"
          >
            Организации
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Организация — верхний уровень иерархии в Enma. Она содержит проекты,
            участников команды и настройки. Каждый пользователь может состоять в
            нескольких организациях.
          </p>
          <h3 className="mt-4 text-lg font-medium text-zinc-900">
            Создание организации
          </h3>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>
              Откройте список организаций в верхнем меню.
            </li>
            <li>
              Нажмите <strong>Создать организацию</strong>.
            </li>
            <li>Введите название и необязательное описание, затем подтвердите.</li>
          </ol>

        </div>

        <div>
          <h2 id="projects" className="text-xl font-semibold text-zinc-900">
            Проекты
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Проекты находятся внутри организаций и изолируют данные событий.
            Типичные сценарии — отдельные проекты для{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              production
            </code>{" "}
            и{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              staging
            </code>
            , или отдельные проекты для разных продуктов.
          </p>
          <h3 className="mt-4 text-lg font-medium text-zinc-900">
            Создание проекта
          </h3>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>
              Откройте список проектов в верхнем меню.
            </li>
            <li>
              Нажмите <strong>Создать проект</strong>.
            </li>
          </ol>
          <h3 className="mt-4 text-lg font-medium text-zinc-900">
            Переключение между проектами
          </h3>
          <p className="mt-2 leading-7 text-zinc-700">
            Текущий проект отображается в верхней панели. Чтобы переключиться,
            нажмите на название проекта.
          </p>
        </div>

        <div>
          <h2 id="navigation" className="text-xl font-semibold text-zinc-900">
            Навигация
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Внутри проекта боковое меню отображает все доступные разделы:
          </p>
          <ul className="mt-3 list-disc space-y-1 pl-6 text-zinc-700">
            <li>
              <strong>Аналитика</strong> — сводные метрики и анализ путей
            </li>
            <li>
              <strong>События</strong> — определения событий
            </li>
            <li>
              <strong>Приложения</strong> — зарегистрированные приложения и
              API-ключи
            </li>
            <li>
              <strong>Процессы</strong> — определения процессов
            </li>
            <li>
              <strong>Логи</strong> — журнал действий
            </li>
          </ul>
        </div>
      </section>
    </article>
  );
}
