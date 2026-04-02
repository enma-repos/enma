export default function QuickStartPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Быстрый старт
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2
            id="create-organization"
            className="text-xl font-semibold text-zinc-900"
          >
            1. Создайте организацию
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            После входа через Google вы попадёте на страницу онбординга.
            Заполните профиль и создайте первую организацию. Организация —
            верхний уровень иерархии, объединяющий проекты и участников команды.
          </p>
        </div>

        <div>
          <h2
            id="create-project"
            className="text-xl font-semibold text-zinc-900"
          >
            2. Создайте проект
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Внутри организации создайте проект. Проекты изолируют данные между
            приложениями или окружениями (например,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              production
            </code>
            ,{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              staging
            </code>
            ).
          </p>
        </div>

        <div>
          <h2
            id="register-app"
            className="text-xl font-semibold text-zinc-900"
          >
            3. Зарегистрируйте приложение
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Перейдите на страницу <strong>Приложения</strong> и нажмите{" "}
            <strong>Создать приложение</strong>. Укажите название и описание.
            После создания сгенерируйте API-ключ — он понадобится для SDK.
          </p>
        </div>

        <div>
          <h2
            id="install-sdk"
            className="text-xl font-semibold text-zinc-900"
          >
            4. Установите SDK
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Добавьте NuGet-пакет Enma.Sdk в ваш .NET проект:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>dotnet add package Enma.Sdk</code>
          </pre>
        </div>

        <div>
          <h2
            id="track-events"
            className="text-xl font-semibold text-zinc-900"
          >
            5. Отправьте первое событие
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Инициализируйте клиент и отправьте событие:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`using Enma.Sdk.Core;

await using var enma = new EnmaClient(o =>
{
    o.ApiToken       = "sdk_your_token";
    o.OrganizationId = Guid.Parse("your-org-id");
    o.ProjectId      = Guid.Parse("your-project-id");
    o.SdkClientId    = Guid.Parse("your-app-id");
});

enma.Track("page.viewed", e =>
{
    e.Actor.UserId = "user-123";
    e.Payload = new { page = "/dashboard" };
});

await enma.FlushAsync();`}</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            События автоматически группируются и отправляются пакетами. Откройте{" "}
            <strong>Аналитика → Сводка</strong>, чтобы увидеть поступающие
            данные.
          </p>
        </div>

        <div>
          <h2 id="next-steps" className="text-xl font-semibold text-zinc-900">
            Следующие шаги
          </h2>
          <ul className="mt-3 space-y-2 text-zinc-700">
            <li>
              <a
                href="/docs/platform/events"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Определите типы событий
              </a>{" "}
              — задайте названия и метаданные для ваших событий
            </li>
            <li>
              <a
                href="/docs/platform/processes"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Создайте процессы
              </a>{" "}
              — сгруппируйте события в рабочие потоки
            </li>
            <li>
              <a
                href="/docs/sdk/dotnet"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Изучите .NET SDK
              </a>{" "}
              — продвинутые паттерны отслеживания
            </li>
          </ul>
        </div>
      </section>
    </article>
  );
}
