export default function SdkOverviewPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Обзор SDK
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="about" className="text-xl font-semibold text-zinc-900">
            О SDK
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            <strong>Enma.Sdk</strong> — это .NET-библиотека с буферизованной
            пакетной отправкой событий, автоматическими повторами, конвейером
            middleware и интеграцией с ASP.NET Core. Целевая платформа — .NET
            Standard 2.1, без внешних зависимостей.
          </p>
        </div>

        <div>
          <h2
            id="installation"
            className="text-xl font-semibold text-zinc-900"
          >
            Установка
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>dotnet add package Enma.Sdk</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Требуется .NET Standard 2.1+ (.NET Core 3.0+, .NET 5+).
          </p>
        </div>

        <div>
          <h2
            id="quick-usage"
            className="text-xl font-semibold text-zinc-900"
          >
            Быстрый пример
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`using Enma.Sdk.Core;

await using var enma = new EnmaClient(o =>
{
    o.ApiToken       = "sdk_your_token";
    o.OrganizationId = Guid.Parse("your-org-id");
    o.ProjectId      = Guid.Parse("your-project-id");
    o.SdkClientId    = Guid.Parse("your-app-id");
});

// Fire-and-forget
enma.Track("page.viewed", e =>
{
    e.Actor.UserId = "user-42";
    e.Payload = new { page = "/dashboard" };
});

// Ожидание подтверждения доставки
await enma.TrackAsync("order.created", e =>
{
    e.Actor.UserId = "user-42";
    e.Payload = new { Amount = 99.99 };
    e.Tag("region", "eu-west");
});

await enma.FlushAsync();`}</code>
          </pre>
        </div>

        <div>
          <h2
            id="how-it-works"
            className="text-xl font-semibold text-zinc-900"
          >
            Как это работает
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`Track() ──► Channel<T> queue ──► Background loop ──► HTTP POST (batch)
              (lock-free)         (flush by timer       (retry on 5xx)
                                   or batch size)`}</code>
          </pre>
          <ol className="mt-3 list-decimal space-y-1 pl-6 text-zinc-700">
            <li>
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                Track()
              </code>{" "}
              помещает событие в ограниченную очередь (
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                System.Threading.Channels
              </code>
              ).
            </li>
            <li>Фоновый цикл читает события и группирует в пакеты.</li>
            <li>
              Пакеты отправляются автоматически каждые{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                FlushInterval
              </code>{" "}
              (5с) или при заполнении пакета.
            </li>
            <li>
              HTTP-запрос повторяется при 5xx/сетевых ошибках (1с → 2с → 4с).
            </li>
            <li>
              При{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                DisposeAsync()
              </code>{" "}
              или{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                FlushAsync()
              </code>{" "}
              все оставшиеся события отправляются.
            </li>
          </ol>
        </div>

        <div>
          <h2 id="features" className="text-xl font-semibold text-zinc-900">
            Ключевые возможности
          </h2>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>
              <strong>Автоматическая группировка</strong> — события собираются в
              пакеты (по умолчанию 50, максимум 200).
            </li>
            <li>
              <strong>Автоматические повторы</strong> — экспоненциальная задержка
              при ошибках 5xx и сетевых сбоях.
            </li>
            <li>
              <strong>Конвейер middleware</strong> — обогащение и фильтрация
              событий перед отправкой.
            </li>
            <li>
              <strong>Lock-free очередь</strong> — на основе{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                System.Threading.Channels
              </code>
              .
            </li>
            <li>
              <strong>ASP.NET Core</strong> — DI и корректное завершение через
              IHostedService.
            </li>
            <li>
              <strong>Без зависимостей</strong> — только стандартные типы .NET.
            </li>
          </ul>
        </div>

        <div>
          <h2
            id="source-code"
            className="text-xl font-semibold text-zinc-900"
          >
            Исходный код
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            SDK с открытым исходным кодом:{" "}
            <a
              href="https://github.com/enma-repos/enma-sdk"
              target="_blank"
              rel="noopener noreferrer"
              className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
            >
              enma-repos/enma-sdk
            </a>
          </p>
        </div>

        <div>
          <h2 id="next-steps" className="text-xl font-semibold text-zinc-900">
            Следующие шаги
          </h2>
          <ul className="mt-3 space-y-2 text-zinc-700">
            <li>
              <a
                href="/docs/sdk/dotnet"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                .NET SDK
              </a>{" "}
              — справочник API и продвинутые паттерны
            </li>
            <li>
              <a
                href="/docs/sdk/configuration"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                Конфигурация
              </a>{" "}
              — все параметры и значения по умолчанию
            </li>
            <li>
              <a
                href="/docs/sdk/aspnet"
                className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
              >
                ASP.NET Core
              </a>{" "}
              — DI и hosted service
            </li>
          </ul>
        </div>
      </section>
    </article>
  );
}
