export default function DocsPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Introduction
      </h1>
      <p className="mt-3 text-lg text-zinc-600">
        Welcome to the enma documentation. Learn how to integrate process mining
        into your product with our SDK and REST API.
      </p>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-6">
        <div>
          <h2 id="what-is-enma" className="text-xl font-semibold text-zinc-900">
            What is enma?
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            enma is a process mining platform that helps you understand how users
            navigate your product. Track events, build process maps, and detect
            anomalies — all in real time.
          </p>
        </div>

        <div>
          <h2 id="key-features" className="text-xl font-semibold text-zinc-900">
            Key Features
          </h2>
          <ul className="mt-3 list-disc space-y-2 pl-6 text-zinc-700">
            <li>Event tracking via lightweight SDK</li>
            <li>Automatic process map generation</li>
            <li>Anomaly detection and alerts</li>
            <li>Team workspaces and access control</li>
            <li>REST API for programmatic access</li>
          </ul>
        </div>

        <div>
          <h2 id="quick-start" className="text-xl font-semibold text-zinc-900">
            Quick Start
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Get started in minutes by installing the JavaScript SDK:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>npm install @enma/sdk</code>
          </pre>
          <p className="mt-3 leading-7 text-zinc-700">
            Initialize the client with your API key:
          </p>
          <pre className="mt-3 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-50 p-4 text-sm text-zinc-800">
            <code>{`import { Enma } from '@enma/sdk';

const enma = new Enma({
  apiKey: 'your-api-key',
});

enma.track('page_view', {
  page: '/dashboard',
});`}</code>
          </pre>
        </div>

        <div>
          <h2 id="next-steps" className="text-xl font-semibold text-zinc-900">
            Next Steps
          </h2>
          <ul className="mt-3 space-y-2 text-zinc-700">
            <li>
              <span className="font-medium text-zinc-900">SDK Setup</span> —
              Detailed installation and configuration guide
            </li>
            <li>
              <span className="font-medium text-zinc-900">Event Tracking</span> —
              Learn how to track custom events
            </li>
            <li>
              <span className="font-medium text-zinc-900">REST API</span> —
              Full API reference documentation
            </li>
          </ul>
        </div>
      </section>
    </article>
  );
}
