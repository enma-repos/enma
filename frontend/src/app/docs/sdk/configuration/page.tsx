export default function ConfigurationPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Конфигурация
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2
            id="required-options"
            className="text-xl font-semibold text-zinc-900"
          >
            Обязательные параметры
          </h2>
          <div className="mt-3 overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-zinc-200 text-left">
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    Параметр
                  </th>
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">Тип</th>
                  <th className="pb-2 font-semibold text-zinc-900">
                    Описание
                  </th>
                </tr>
              </thead>
              <tbody className="text-zinc-700">
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">ApiToken</td>
                  <td className="py-2 pr-4">string</td>
                  <td className="py-2">
                    Токен аутентификации (формат{" "}
                    <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                      sdk_...
                    </code>
                    )
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    OrganizationId
                  </td>
                  <td className="py-2 pr-4">Guid</td>
                  <td className="py-2">ID организации</td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">ProjectId</td>
                  <td className="py-2 pr-4">Guid</td>
                  <td className="py-2">ID проекта</td>
                </tr>
                <tr>
                  <td className="py-2 pr-4 font-mono text-sm">SdkClientId</td>
                  <td className="py-2 pr-4">Guid</td>
                  <td className="py-2">ID приложения (SDK-клиента)</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div>
          <h2
            id="optional-options"
            className="text-xl font-semibold text-zinc-900"
          >
            Необязательные параметры
          </h2>
          <div className="mt-3 overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-zinc-200 text-left">
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    Параметр
                  </th>
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">Тип</th>
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    По умолчанию
                  </th>
                  <th className="pb-2 font-semibold text-zinc-900">
                    Описание
                  </th>
                </tr>
              </thead>
              <tbody className="text-zinc-700">
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">BaseUrl</td>
                  <td className="py-2 pr-4">Uri</td>
                  <td className="py-2 pr-4">https://sdk.enma.tech</td>
                  <td className="py-2">Базовый URL Ingest API</td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">BatchSize</td>
                  <td className="py-2 pr-4">int</td>
                  <td className="py-2 pr-4">50</td>
                  <td className="py-2">
                    Событий в одном HTTP-запросе (макс. 200)
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    FlushInterval
                  </td>
                  <td className="py-2 pr-4">TimeSpan</td>
                  <td className="py-2 pr-4">5 сек</td>
                  <td className="py-2">
                    Интервал автоматической отправки
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    MaxQueueSize
                  </td>
                  <td className="py-2 pr-4">int</td>
                  <td className="py-2 pr-4">10 000</td>
                  <td className="py-2">
                    Ёмкость ограниченной очереди
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    MaxRetries
                  </td>
                  <td className="py-2 pr-4">int</td>
                  <td className="py-2 pr-4">3</td>
                  <td className="py-2">
                    Повторные попытки при 5xx / сетевых ошибках
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">DefaultTags</td>
                  <td className="py-2 pr-4">
                    {"Dictionary<string, string>"}
                  </td>
                  <td className="py-2 pr-4">{"{}"}</td>
                  <td className="py-2">
                    Глобальные теги для всех событий
                  </td>
                </tr>
                <tr>
                  <td className="py-2 pr-4 font-mono text-sm">OnError</td>
                  <td className="py-2 pr-4">Action</td>
                  <td className="py-2 pr-4">null</td>
                  <td className="py-2">
                    Callback при ошибке доставки пакета
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div>
          <h2 id="example" className="text-xl font-semibold text-zinc-900">
            Полный пример
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`await using var enma = new EnmaClient(o =>
{
    // Обязательные
    o.ApiToken       = "sdk_your_token";
    o.OrganizationId = Guid.Parse("...");
    o.ProjectId      = Guid.Parse("...");
    o.SdkClientId    = Guid.Parse("...");

    // Транспорт
    o.BaseUrl       = new Uri("https://sdk.enma.tech"); // по умолчанию
    o.BatchSize     = 100;
    o.FlushInterval = TimeSpan.FromSeconds(10);
    o.MaxQueueSize  = 50_000;
    o.MaxRetries    = 5;

    // Глобальные теги
    o.DefaultTags["service"] = "order-api";
    o.DefaultTags["env"]     = "production";

    // Middleware
    o.Use(next => async (evt, ct) =>
    {
        evt.Tags ??= new Dictionary<string, string>();
        evt.Tags["host"] = Environment.MachineName;
        await next(evt, ct);
    });

    // Обработка ошибок
    o.OnError = (events, ex) =>
    {
        logger.LogError(ex, "Ошибка отправки {Count} событий", events.Count);
    };
});`}</code>
          </pre>
        </div>

        <div>
          <h2
            id="appsettings"
            className="text-xl font-semibold text-zinc-900"
          >
            Конфигурация через appsettings.json
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            При использовании ASP.NET Core параметры можно вынести в
            конфигурационный файл или переменные окружения:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`{
  "Enma": {
    "ApiToken": "sdk_your_token",
    "OrganizationId": "...",
    "ProjectId": "...",
    "SdkClientId": "...",
    "BatchSize": 100,
    "MaxRetries": 5
  }
}`}</code>
          </pre>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>builder.Services.AddEnma(builder.Configuration.GetSection("Enma"));</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Подробнее —{" "}
            <a
              href="/docs/sdk/aspnet"
              className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
            >
              ASP.NET Core интеграция
            </a>
            .
          </p>
        </div>
      </section>
    </article>
  );
}
