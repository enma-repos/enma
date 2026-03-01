import { Card, CardContent, CardDescription, CardHeader, CardTitle, IconExternalLink } from "@/components/shared";

export function FlowPlaceholder() {
    return (
        <Card className="h-full">
            <div className="py-0 my-0 grid h-full w-full place-items-center rounded-2xl border border-dashed border-zinc-200 bg-zinc-50 text-sm text-zinc-400">
                React Flow граф будет здесь
            </div>
        </Card>
    );
}