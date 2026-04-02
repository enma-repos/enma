export default function AppsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Приложения и API-ключи
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Каждый проект может содержать несколько приложений. Приложение
            представляет одну точку интеграции — например, серверный сервис,
            мобильное приложение или веб-клиент. У каждого приложения свой набор
            API-ключей.
          </p>
        </div>

        <div>
          <h2
            id="creating-apps"
            className="text-xl font-semibold text-zinc-900"
          >
            Создание приложения
          </h2>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>
              Перейдите в раздел <strong>Приложения</strong> в боковом меню.
            </li>
            <li>
              Нажмите <strong>Создать приложение</strong>.
            </li>
            <li>
              Введите название (например,{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                backend-api
              </code>
              ) и необязательное описание.
            </li>
            <li>Подтвердите создание.</li>
          </ol>
        </div>

        <div>
          <h2
            id="app-detail"
            className="text-xl font-semibold text-zinc-900"
          >
            Страница приложения
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Нажмите на приложение в таблице, чтобы открыть его детальную
            страницу. Здесь вы увидите:
          </p>
          <ul className="mt-3 list-disc space-y-1 pl-6 text-zinc-700">
            <li>
              <strong>ID приложения</strong> — уникальный идентификатор,
              используемый как{" "}
              <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                SdkClientId
              </code>{" "}
              в конфигурации SDK.
            </li>
            <li>
              <strong>Тип</strong> — тип приложения (только для чтения).
            </li>
            <li>
              <strong>Описание</strong> — редактируемое поле описания.
            </li>
            <li>
              <strong>API-ключи</strong> — список ключей этого приложения.
            </li>
          </ul>
        </div>

        <div>
          <h2 id="api-keys" className="text-xl font-semibold text-zinc-900">
            API-ключи
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            API-ключи аутентифицируют запросы SDK. Каждый ключ принадлежит
            конкретному приложению.
          </p>
          <h3 className="mt-4 text-lg font-medium text-zinc-900">
            Генерация ключа
          </h3>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>Откройте страницу приложения.</li>
            <li>
              Нажмите <strong>Сгенерировать API-ключ</strong>.
            </li>
            <li>
              Скопируйте ключ сразу — он показывается только один раз.
            </li>
          </ol>
          <h3 className="mt-4 text-lg font-medium text-zinc-900">
            Отзыв ключа
          </h3>
          <p className="mt-2 leading-7 text-zinc-700">
            Нажмите кнопку удаления рядом с ключом, чтобы отозвать его. Все
            экземпляры SDK, использующие этот ключ, перестанут отправлять
            события. Это действие необратимо.
          </p>
        </div>

        <div>
          <h2
            id="sdk-configuration"
            className="text-xl font-semibold text-zinc-900"
          >
            Использование ключей в SDK
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Передайте API-ключ в параметре{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              ApiToken
            </code>{" "}
            при создании{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              EnmaClient
            </code>
            :
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`await using var enma = new EnmaClient(o =>
{
    o.ApiToken       = "sdk_your_token";
    o.OrganizationId = Guid.Parse("your-org-id");
    o.ProjectId      = Guid.Parse("your-project-id");
    o.SdkClientId    = Guid.Parse("your-app-id");
});`}</code>
          </pre>
        </div>
      </section>
    </article>
  );
}
