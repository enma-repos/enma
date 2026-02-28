import {FeaturesSection, HeroSection, ShowcaseSection} from "@/components/landing";

export default function Page() {
  return (
      <div className="min-h-screen bg-zinc-50">
        <main>
          <HeroSection />
          <ShowcaseSection />
          <FeaturesSection />
        </main>
      </div>
  );
}
