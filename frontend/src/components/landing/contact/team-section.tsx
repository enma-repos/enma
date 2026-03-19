import { Linkedin, Github, Code, BarChart3 } from "lucide-react";
import type { ElementType } from "react";
import {
  Card,
  CardHeader,
  CardContent,
  SocialLink,
} from "@/components/shared";

const team: {
  name: string;
  role: string;
  icon: ElementType;
  faculty: string;
  university: string;
  links: readonly { href: string; icon: ElementType; label: string }[];
}[] = [
  {
    name: "Mukhtarov Ramin Elshan ogly",
    role: "CTO, Fullstack Engineer",
    icon: Code,
    faculty: "Faculty of Secure Information Technologies",
    university: "ITMO University",
    links: [
      {
        href: "https://www.linkedin.com/in/ramin-muhtarov/",
        icon: Linkedin,
        label: "LinkedIn",
      },
      {
        href: "https://github.com/ulkiorra4th",
        icon: Github,
        label: "GitHub",
      },
    ],
  },
  {
    name: "Gracheva Anna Romanovna",
    role: "CPO, Business Analyst",
    icon: BarChart3,
    faculty: "Faculty of Technological Management and Innovations",
    university: "ITMO University",
    links: [],
  },
] as const;

export function TeamSection() {
  return (
    <section className="mx-auto max-w-7xl px-6 mt-10">
      <div className="mx-auto grid max-w-7xl gap-8 sm:grid-cols-2">
        {team.map((member) => (
          <Card key={member.name} className="flex flex-col px-7 py-10 transition-shadow duration-300 hover:shadow-lg">
            <CardHeader className="flex flex-row items-start gap-5 p-0">
              <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-blue-50 text-blue-600">
                <member.icon className="h-6 w-6" />
              </div>
              <div>
                <p className="text-3xl font-semibold text-zinc-900">
                  {member.name}
                </p>
                <p className="mt-3 text-base font-medium text-blue-700">
                  {member.role}
                </p>
              </div>
            </CardHeader>
            <CardContent className="mt-8 p-0">
              <p className="text-base text-zinc-500">
                {member.university}, {member.faculty}
              </p>

              <div className="mt-5 flex gap-2">
                {member.links.map((link) => (
                  <SocialLink
                    key={link.label}
                    href={link.href}
                    icon={link.icon}
                    label={`${member.name} ${link.label}`}
                  />
                ))}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </section>
  );
}
