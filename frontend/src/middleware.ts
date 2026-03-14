import { type NextRequest, NextResponse } from "next/server";

const DEFAULT_API_GATEWAY_URL = "http://localhost:8080";
const PENDING_PROFILE_STATUS = 0;

export const config = {
  matcher: "/app/:path*",
};

export async function middleware(request: NextRequest) {
  const apiGatewayUrl =
    process.env.NEXT_PUBLIC_API_GATEWAY_URL ?? DEFAULT_API_GATEWAY_URL;
  const meUrl = new URL("/api/auth/v1/me", apiGatewayUrl);
  const cookieHeader = request.headers.get("cookie") ?? "";

  try {
    const response = await fetch(meUrl, {
      method: "GET",
      headers: { accept: "application/json", cookie: cookieHeader },
    });

    if (response.status !== 200) {
      return NextResponse.next();
    }

    const me = await response.json();
    const pathname = request.nextUrl.pathname;
    const isOnboarding = pathname.startsWith("/app/onboarding");

    if (me.account.status === PENDING_PROFILE_STATUS && !isOnboarding) {
      return NextResponse.redirect(new URL("/app/onboarding", request.url));
    }

    if (me.account.status !== PENDING_PROFILE_STATUS && isOnboarding) {
      return NextResponse.redirect(
        new URL("/app/organizations", request.url),
      );
    }
  } catch {
    // If fetch fails, let the layout handle the error
  }

  return NextResponse.next();
}
