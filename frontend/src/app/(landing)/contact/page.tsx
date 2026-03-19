import {
  ContactHeroSection,
  TeamSection,
  ContactInfoSection,
} from "@/components/landing/contact";

export default function Page() {
  return (
    <div className="min-h-screen bg-white">
      <main>
        <ContactHeroSection />
        <TeamSection />
        <ContactInfoSection />
      </main>
    </div>
  );
}
