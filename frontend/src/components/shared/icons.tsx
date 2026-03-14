"use client";

import type { SVGProps } from "react";

export type IconProps = SVGProps<SVGSVGElement> & { size?: number };

function Svg({ size = 18, ...props }: IconProps) {
  return (
    <svg width={size} height={size} viewBox="0 0 24 24" fill="none" {...props} />
  );
}

export function IconHome(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M4 10.5 12 4l8 6.5V20a1 1 0 0 1-1 1h-5v-6H10v6H5a1 1 0 0 1-1-1v-9.5Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconChart(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M4 20V10m5 10V4m5 16v-7m6 7V8"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}

export function IconList(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M8 6h13M8 12h13M8 18h13M3.5 6h.01M3.5 12h.01M3.5 18h.01"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}

export function IconUsers(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M16 11a4 4 0 1 0-8 0m14 10a6 6 0 0 0-12 0m-6 0a8 8 0 0 1 12-6.9"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}

export function IconBolt(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M13 2 4 14h7l-1 8 9-12h-7l1-8Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconBox(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M12 2 3 7v10l9 5 9-5V7l-9-5Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
      <path
        d="M3 7l9 5 9-5"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconGear(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M12 15.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7Z"
        stroke="currentColor"
        strokeWidth="1.8"
      />
      <path
        d="M19.4 15a8.8 8.8 0 0 0 .1-2l2-1.2-2-3.4-2.2.6a8.5 8.5 0 0 0-1.7-1L15.3 5h-3.6l-.3 2.1a8.5 8.5 0 0 0-1.7 1l-2.2-.6-2 3.4 2 1.2a8.8 8.8 0 0 0 .1 2l-2 1.2 2 3.4 2.2-.6c.5.4 1.1.8 1.7 1l.3 2.1h3.6l.3-2.1c.6-.2 1.2-.6 1.7-1l2.2.6 2-3.4-2-1.2Z"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconGrid(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M4 4h7v7H4V4Zm9 0h7v7h-7V4ZM4 13h7v7H4v-7Zm9 0h7v7h-7v-7Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconSearch(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M10.5 18a7.5 7.5 0 1 1 0-15 7.5 7.5 0 0 1 0 15Z"
        stroke="currentColor"
        strokeWidth="1.8"
      />
      <path
        d="M16.5 16.5 21 21"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}

export function IconBell(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M12 22a2.5 2.5 0 0 0 2.3-1.5H9.7A2.5 2.5 0 0 0 12 22Z"
        fill="currentColor"
      />
      <path
        d="M18 16V11a6 6 0 0 0-12 0v5l-2 2h16l-2-2Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconMoon(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M21 14.5A7.5 7.5 0 0 1 9.5 3a6 6 0 1 0 11.5 11.5Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconChevronDown(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="m6 9 6 6 6-6"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconExternalLink(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M14 5h5v5"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path
        d="M10 14 19 5"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
      <path
        d="M19 14v5a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1V6a1 1 0 0 1 1-1h5"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconGoogle({ size = 18, ...props }: IconProps) {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 48 48"
      xmlns="http://www.w3.org/2000/svg"
      {...props}
    >
      <path
        d="M44.5 20H24v8.5h11.8C34.7 34 30 37.5 24 37.5c-7.1 0-13-5.9-13-13s5.9-13 13-13c3.4 0 6.2 1.2 8.4 3.2l6-6C35.1 5.4 30 3.5 24 3.5 12 3.5 2.5 13 2.5 24S12 44.5 24 44.5c11 0 20-8 20-20 0-1.3-.2-2.3-.5-4.5Z"
        fill="#4285F4"
      />
      <path
        d="M6.3 14.7 13.3 20c1.9-4.6 6.4-8 10.7-8 3.4 0 6.2 1.2 8.4 3.2l6-6C35.1 5.4 30 3.5 24 3.5c-8 0-14.9 4.6-17.7 11.2Z"
        fill="#34A853"
      />
      <path
        d="M24 44.5c5.8 0 10.7-1.9 14.3-5.2l-6.6-5.4c-1.8 1.3-4.2 2.1-7.7 2.1-6 0-10.7-3.9-12.4-9.2l-7.1 5.5C7.2 39.2 14.9 44.5 24 44.5Z"
        fill="#FBBC05"
      />
      <path
        d="M44.5 20H24v8.5h11.8c-0.8 2.4-2.4 4.4-4.4 5.7l6.6 5.4C41.7 36 44 30.8 44 24.5c0-1.3-.2-2.3-.5-4.5Z"
        fill="#EA4335"
      />
    </svg>
  );
}

export function IconPlus(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M12 5v14M5 12h14"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}

export function IconTrash(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M5 7h14M9 7V5a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2m2 0v12a2 2 0 0 1-2 2H9a2 2 0 0 1-2-2V7h10Z"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </Svg>
  );
}

export function IconX(props: IconProps) {
  return (
    <Svg {...props}>
      <path
        d="M6 6l12 12M18 6 6 18"
        stroke="currentColor"
        strokeWidth="1.8"
        strokeLinecap="round"
      />
    </Svg>
  );
}
