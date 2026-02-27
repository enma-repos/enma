import {
  FeaturesSection,
  HeroSection,
  LandingFooter,
  LandingHeader,
  ShowcaseSection,
} from "@/components/landing";

export default function Home() {
  return (
    <div className="min-h-screen bg-zinc-50">
      <LandingHeader />
      <main>
        <HeroSection />
        <ShowcaseSection />
        <FeaturesSection />
      </main>
      <LandingFooter />
    </div>
  );
}
