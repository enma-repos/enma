"use client";

import type { SVGProps } from "react";
import {
  Home,
  BarChart3,
  List,
  Users,
  Zap,
  Box,
  Settings,
  LayoutGrid,
  Search,
  Bell,
  Moon,
  ChevronDown,
  ExternalLink,
  Plus,
  Trash2,
  X,
  Check,
  Info,
  Image,
} from "lucide-react";
import type { LucideProps } from "lucide-react";

export type IconProps = SVGProps<SVGSVGElement> & { size?: number };

function wrap(Icon: React.FC<LucideProps>) {
  const Wrapper = ({ size = 18, className, ...rest }: IconProps) => (
    <Icon size={size} className={className} {...(rest as LucideProps)} />
  );
  Wrapper.displayName = Icon.displayName;
  return Wrapper;
}

export const IconHome = wrap(Home);
export const IconChart = wrap(BarChart3);
export const IconList = wrap(List);
export const IconUsers = wrap(Users);
export const IconBolt = wrap(Zap);
export const IconBox = wrap(Box);
export const IconGear = wrap(Settings);
export const IconGrid = wrap(LayoutGrid);
export const IconSearch = wrap(Search);
export const IconBell = wrap(Bell);
export const IconMoon = wrap(Moon);
export const IconChevronDown = wrap(ChevronDown);
export const IconExternalLink = wrap(ExternalLink);
export const IconPlus = wrap(Plus);
export const IconTrash = wrap(Trash2);
export const IconX = wrap(X);
export const IconCheck = wrap(Check);
export const IconInfo = wrap(Info);
export const IconImage = wrap(Image);

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
