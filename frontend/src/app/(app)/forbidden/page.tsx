export default function Page() {
  return (
    <div className="min-h-screen bg-zinc-50 px-6 py-10">
      <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-3xl items-center justify-center">
        <div className="w-full rounded-2xl border border-zinc-200 bg-white p-7 shadow-sm">
          <h1 className="text-xl font-semibold text-zinc-900">Нет доступа</h1>
          <p className="mt-2 text-sm text-zinc-500">
            У вашего аккаунта недостаточно прав для просмотра этого раздела.
          </p>
        </div>
      </div>
    </div>
  );
}

