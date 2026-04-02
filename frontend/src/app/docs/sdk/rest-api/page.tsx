export default function RestApiPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        REST API
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Enma Ingest API принимает события по HTTP. SDK автоматически
            формирует запросы, группирует события в пакеты и обрабатывает ошибки.
            Прямое использование REST API рекомендуется только для платформ, где
            .NET SDK недоступен.
          </p>
        </div>

        <div>
          <h2 id="base-url" className="text-xl font-semibold text-zinc-900">
            Базовый URL
          </h2>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>https://sdk.enma.tech</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Базовый URL можно переопределить через параметр{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              BaseUrl
            </code>{" "}
            в настройках SDK.
          </p>
        </div>

        <div>
          <h2
            id="authentication"
            className="text-xl font-semibold text-zinc-900"
          >
            Аутентификация
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Запросы аутентифицируются с помощью API-токена (формат{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              sdk_...
            </code>
            ), который генерируется на странице приложения.
            Токен передаётся в заголовке:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>Authorization: Bearer sdk_your_token</code>
          </pre>
        </div>

        <div>
          <h2 id="transport" className="text-xl font-semibold text-zinc-900">
            Транспорт
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            SDK отправляет события методом{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              HTTP POST
            </code>{" "}
            пакетами. Каждый пакет содержит до{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              BatchSize
            </code>{" "}
            событий (по умолчанию 50, максимум 200).
          </p>
          <p className="mt-2 leading-7 text-zinc-700">
            Каждый запрос включает идентификаторы организации, проекта и
            приложения (SDK-клиента), а также массив событий с названием, данными
            об акторе, полезной нагрузкой и тегами.
          </p>
        </div>

        <div>
          <h2
            id="event-object"
            className="text-xl font-semibold text-zinc-900"
          >
            Структура события
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Каждое событие, отправляемое через SDK, содержит следующие поля:
          </p>
          <div className="mt-3 overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-zinc-200 text-left">
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    Поле
                  </th>
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">Тип</th>
                  <th className="pb-2 pr-4 font-semibold text-zinc-900">
                    Обязательно
                  </th>
                  <th className="pb-2 font-semibold text-zinc-900">
                    Описание
                  </th>
                </tr>
              </thead>
              <tbody className="text-zinc-700">
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">eventName</td>
                  <td className="py-2 pr-4">string</td>
                  <td className="py-2 pr-4">да</td>
                  <td className="py-2">
                    Название события (например,{" "}
                    <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                      order.created
                    </code>
                    )
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">actor</td>
                  <td className="py-2 pr-4">object</td>
                  <td className="py-2 pr-4">нет</td>
                  <td className="py-2">
                    Объект с{" "}
                    <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                      userId
                    </code>{" "}
                    и/или{" "}
                    <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
                      anonymousId
                    </code>
                  </td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">payload</td>
                  <td className="py-2 pr-4">object</td>
                  <td className="py-2 pr-4">нет</td>
                  <td className="py-2">Произвольные данные (JSON)</td>
                </tr>
                <tr className="border-b border-zinc-100">
                  <td className="py-2 pr-4 font-mono text-sm">tags</td>
                  <td className="py-2 pr-4">object</td>
                  <td className="py-2 pr-4">нет</td>
                  <td className="py-2">
                    Метаданные ключ-значение (строки)
                  </td>
                </tr>
                <tr>
                  <td className="py-2 pr-4 font-mono text-sm">processKeys</td>
                  <td className="py-2 pr-4">array</td>
                  <td className="py-2 pr-4">нет</td>
                  <td className="py-2">
                    Привязки к процессам: каждый элемент содержит ID определения
                    процесса и строковый идентификатор экземпляра
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div>
          <h2
            id="retry-policy"
            className="text-xl font-semibold text-zinc-900"
          >
            Политика повторов
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            При ошибках 5xx и сетевых сбоях SDK автоматически повторяет запрос с
            экспоненциальной задержкой: 1с → 2с → 4с.
            Количество попыток настраивается параметром{" "}
            <code className="rounded bg-zinc-100 px-1.5 py-0.5 text-sm font-mono">
              MaxRetries
            </code>{" "}
            (по умолчанию 3).
          </p>
        </div>

        <div>
          <h2
            id="recommendation"
            className="text-xl font-semibold text-zinc-900"
          >
            Рекомендация
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Используйте{" "}
            <a
              href="/docs/sdk/dotnet"
              className="font-medium text-zinc-900 underline decoration-zinc-300 underline-offset-4 hover:decoration-zinc-900"
            >
              .NET SDK
            </a>{" "}
            вместо прямых HTTP-запросов. SDK обеспечивает автоматическую
            группировку, повторы, lock-free очередь и корректное завершение
            работы при остановке приложения.
          </p>
        </div>
      </section>
    </article>
  );
}
