import { Card, CardContent, CardDescription, CardHeader, CardTitle, IconExternalLink } from "@/components/shared";

export function FlowPlaceholder() {
    return (
        <Card className="h-full">
            <CardHeader className="flex flex-row items-start justify-between gap-3">
                <div>
                    <CardTitle>Диаграмма пользовательских путей</CardTitle>
                    <CardDescription>
                        Визуализация всех маршрутов по которым проходят пользователи в вашем приложении
                    </CardDescription>
                </div>
                <IconExternalLink className="h-4 w-4 text-zinc-400" aria-hidden="true" />
            </CardHeader>
            <CardContent>
                <div className="grid h-[420px] place-items-center rounded-2xl border border-dashed border-zinc-200 bg-zinc-50 text-sm text-zinc-400">
                    React Flow граф добавишь здесь
                </div>
            </CardContent>
        </Card>
    );
}