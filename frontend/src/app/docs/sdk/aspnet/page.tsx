export default function AspnetPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        ASP.NET Core
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2
            id="registration"
            className="text-xl font-semibold text-zinc-900"
          >
            Регистрация
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Вызовите{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              AddEnma()
            </code>{" "}
            в{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              Program.cs
            </code>
            :
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`builder.Services.AddEnma(o =>
{
    o.ApiToken       = builder.Configuration["Enma:ApiToken"]!;
    o.OrganizationId = Guid.Parse(builder.Configuration["Enma:OrgId"]!);
    o.ProjectId      = Guid.Parse(builder.Configuration["Enma:ProjectId"]!);
    o.SdkClientId    = Guid.Parse(builder.Configuration["Enma:SdkClientId"]!);
});`}</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              AddEnma()
            </code>{" "}
            регистрирует:
          </p>
          <ul className="mt-2 list-disc space-y-1 pl-6 text-zinc-700">
            <li>
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                IEnmaClient
              </code>{" "}
              как singleton
            </li>
            <li>
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                IHostedService
              </code>{" "}
              для корректного завершения (отправка оставшихся событий при
              остановке хоста)
            </li>
          </ul>
        </div>

        <div>
          <h2
            id="config-binding"
            className="text-xl font-semibold text-zinc-900"
          >
            Привязка через IConfiguration
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Вместо инлайн-конфигурации можно привязать секцию целиком:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`// appsettings.json
{
  "Enma": {
    "ApiToken": "sdk_...",
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
        </div>

        <div>
          <h2
            id="injection"
            className="text-xl font-semibold text-zinc-900"
          >
            Использование через DI
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Внедряйте{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              IEnmaClient
            </code>{" "}
            в контроллеры, сервисы или middleware:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`public class OrdersController : ControllerBase
{
    private readonly IEnmaClient _enma;

    public OrdersController(IEnmaClient enma) => _enma = enma;

    [HttpPost]
    public IActionResult CreateOrder(CreateOrderRequest request)
    {
        _enma.Track("order.created", e =>
        {
            e.Actor.UserId = User.FindFirst("sub")?.Value;
            e.Payload = new { request.ProductId, request.Quantity };
        });

        return Ok();
    }
}`}</code>
          </pre>
        </div>

        <div>
          <h2
            id="graceful-shutdown"
            className="text-xl font-semibold text-zinc-900"
          >
            Корректное завершение
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            При использовании{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              AddEnma()
            </code>{" "}
            все буферизованные события автоматически отправляются при остановке
            хоста через{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              IHostedService
            </code>
            . Вызывать{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              FlushAsync()
            </code>{" "}
            вручную не нужно.
          </p>
        </div>

        <div>
          <h2
            id="middleware-example"
            className="text-xl font-semibold text-zinc-900"
          >
            Пример с middleware и тегами
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`builder.Services.AddEnma(o =>
{
    o.ApiToken       = "sdk_...";
    o.OrganizationId = Guid.Parse("...");
    o.ProjectId      = Guid.Parse("...");
    o.SdkClientId    = Guid.Parse("...");

    // Глобальные теги
    o.DefaultTags["service"] = "order-api";
    o.DefaultTags["env"]     = builder.Environment.EnvironmentName;

    // Middleware — добавляем имя хоста
    o.Use(next => async (evt, ct) =>
    {
        evt.Tags ??= new Dictionary<string, string>();
        evt.Tags["host"] = Environment.MachineName;
        await next(evt, ct);
    });
});`}</code>
          </pre>
        </div>
      </section>
    </article>
  );
}
