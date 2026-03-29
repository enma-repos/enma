import { Suspense } from "react";
import { AuthScreen } from "@/components/app/auth/auth-screen";

export default function Page() {
  return (
    <Suspense>
      <AuthScreen />
    </Suspense>
  );
}
