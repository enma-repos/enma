export default function TeamPage() {
  return (
    <article>
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900">
        Команда
      </h1>

      <hr className="my-8 border-zinc-200" />

      <section className="space-y-8">
        <div>
          <h2 id="overview" className="text-xl font-semibold text-zinc-900">
            Обзор
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Управление командой доступно по иконке команды в правом верхнем углу
            приложения. Откроется всплывающее окно, где можно просматривать
            участников, приглашать новых пользователей и управлять ожидающими
            приглашениями.
          </p>
        </div>

        <div>
          <h2 id="members" className="text-xl font-semibold text-zinc-900">
            Просмотр участников
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Вкладка <strong>Участники</strong> показывает всех пользователей
            текущей организации. Каждая запись содержит имя, email, роль и дату
            присоединения. Нажмите на участника, чтобы увидеть детали профиля.
          </p>
        </div>

        <div>
          <h2
            id="inviting-members"
            className="text-xl font-semibold text-zinc-900"
          >
            Приглашение участников
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Перейдите на вкладку <strong>Пригласить</strong>:
          </p>
          <ol className="mt-2 list-decimal space-y-2 pl-6 text-zinc-700">
            <li>Введите email-адрес приглашаемого.</li>
            <li>Выберите роль.</li>
            <li>
              Нажмите <strong>Отправить приглашение</strong>. Получатель получит
              уведомление в приложении с приглашением в организацию.
            </li>
          </ol>
        </div>

        <div>
          <h2
            id="pending-invites"
            className="text-xl font-semibold text-zinc-900"
          >
            Ожидающие приглашения
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Ожидающие приглашения отображаются на вкладке приглашений. Вы можете
            отменить приглашение до его принятия.
          </p>
        </div>

        <div>
          <h2
            id="notifications"
            className="text-xl font-semibold text-zinc-900"
          >
            Уведомления
          </h2>
          <p className="mt-2 leading-7 text-zinc-700">
            Иконка колокольчика в верхней панели показывает непрочитанные
            уведомления. Вы получите уведомление, когда кто-то примет ваше
            приглашение или при изменениях в организации.
          </p>
        </div>
      </section>
    </article>
  );
}
