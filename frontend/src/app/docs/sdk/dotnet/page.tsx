export default function DotnetSdkPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        .NET SDK
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2
            id="enma-client"
            className="text-xl font-semibold text-zinc-900"
          >
            EnmaClient
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Основная точка входа. Клиент реализует{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              IAsyncDisposable
            </code>
            , поэтому рекомендуется использовать{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              await using
            </code>
            :
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`await using var enma = new EnmaClient(o =>
{
    o.ApiToken       = "sdk_your_token";
    o.OrganizationId = Guid.Parse("...");
    o.ProjectId      = Guid.Parse("...");
    o.SdkClientId    = Guid.Parse("...");
});`}</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            При завершении (
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              DisposeAsync
            </code>
            ) все оставшиеся события автоматически отправляются на сервер.
          </p>
        </div>

        <div>
          <h2 id="track" className="text-xl font-semibold text-zinc-900">
            Track(eventName, configure?)
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Fire-and-forget отправка. Событие помещается в очередь и
            отправляется в фоне. Возвращает управление немедленно.
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`// Минимальный вызов
enma.Track("page.viewed");

// С данными
enma.Track("order.created", e =>
{
    e.Actor.UserId = "user-42";
    e.Payload = new { Amount = 99.99, Currency = "USD" };
    e.Tag("region", "eu-west");
});`}</code>
          </pre>
        </div>

        <div>
          <h2
            id="track-async"
            className="text-xl font-semibold text-zinc-900"
          >
            TrackAsync(eventName, configure?, ct?)
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Асинхронная отправка. Task завершается, когда событие доставлено на
            сервер. Используйте, когда нужна гарантия доставки.
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`await enma.TrackAsync("payment.processed", e =>
{
    e.Actor.UserId = "user-42";
    e.Payload = new { TransactionId = "tx-001", Amount = 150.00 };
    e.Tag("provider", "stripe");
});`}</code>
          </pre>
        </div>

        <div>
          <h2
            id="flush-async"
            className="text-xl font-semibold text-zinc-900"
          >
            FlushAsync(ct?)
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Принудительная отправка всех буферизованных событий. Полезно перед
            завершением приложения, если не используется{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              await using
            </code>{" "}
            или ASP.NET Core интеграция.
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>await enma.FlushAsync();</code>
          </pre>
        </div>

        <div>
          <h2
            id="event-builder"
            className="text-xl font-semibold text-zinc-900"
          >
            EventBuilder
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Callback-построитель настраивает отдельные события:
          </p>
          <div className="mt-3 overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-zinc-200 text-left">
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    Член
                  </th>
                  <th className="pb-2 font-semibold text-zinc-900">
                    Описание
                  </th>
                </tr>
              </thead>
              <tbody className="text-zinc-700">
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    Actor.UserId
                  </td>
                  <td className="py-2">ID идентифицированного пользователя</td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    Actor.AnonymousId
                  </td>
                  <td className="py-2">ID анонимного посетителя</td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">Payload</td>
                  <td className="py-2">
                    Произвольный объект (сериализуется в JSON)
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    ProcessKey(Guid defId, string processId)
                  </td>
                  <td className="py-2">
                    Привязка события к экземпляру процесса
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">
                    Tag(string key, string value)
                  </td>
                  <td className="py-2">Добавить один тег</td>
                </tr>
                <tr>
                  <td className="py-2 pr-4 font-mono text-sm">
                    {"Tags(Dictionary<string, string>)"}
                  </td>
                  <td className="py-2">Добавить несколько тегов</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div>
          <h2
            id="process-keys"
            className="text-xl font-semibold text-zinc-900"
          >
            Ключи процессов
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Привязывайте события к экземплярам процессов. Одно событие может быть
            связано с несколькими процессами:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`enma.Track("step.completed", e =>
{
    e.Actor.UserId = "user-42";
    e.ProcessKey(Guid.Parse("...process-def-id..."), "order-123");
    e.ProcessKey("another-def-id-guid", "session-456");
});`}</code>
          </pre>
        </div>

        <div>
          <h2 id="tags" className="text-xl font-semibold text-zinc-900">
            Теги
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Теги — это метаданные ключ-значение. Теги события имеют приоритет
            над{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              DefaultTags
            </code>{" "}
            при совпадении ключей:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`enma.Track("item.added", e =>
{
    e.Tag("category", "electronics");
    e.Tags(new Dictionary<string, string>
    {
        ["source"] = "mobile",
        ["ab_test"] = "variant-b"
    });
});`}</code>
          </pre>
        </div>

        <div>
          <h2 id="middleware" className="text-xl font-semibold text-zinc-900">
            Middleware
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Используйте middleware для обогащения или фильтрации событий перед
            помещением в очередь. Вызовите{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              next
            </code>
            , чтобы передать событие дальше по конвейеру, или пропустите вызов,
            чтобы отбросить событие:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`var enma = new EnmaClient(o =>
{
    // ... учётные данные

    // Добавляем имя хоста к каждому событию
    o.Use(next => async (evt, ct) =>
    {
        evt.Tags ??= new Dictionary<string, string>();
        evt.Tags["host"] = Environment.MachineName;
        await next(evt, ct);
    });

    // Фильтруем внутренние события
    o.Use(next => async (evt, ct) =>
    {
        if (!evt.EventName.StartsWith("internal."))
            await next(evt, ct);
    });
});`}</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Middleware выполняется в порядке регистрации.
          </p>
        </div>

        <div>
          <h2
            id="error-handling"
            className="text-xl font-semibold text-zinc-900"
          >
            Обработка ошибок
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Зарегистрируйте callback для обработки ошибок доставки:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`var enma = new EnmaClient(o =>
{
    // ... учётные данные

    o.OnError = (events, ex) =>
    {
        Console.Error.WriteLine(
            $"Не удалось отправить {events.Count} событий: {ex.Message}");
    };
});`}</code>
          </pre>
        </div>
      </section>
    </article>
  );
}
