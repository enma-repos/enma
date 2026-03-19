import {
  HeroSection,
  ShowcaseSection,
  FeaturesSection,
  HowItWorksSection,
  UseCasesSection,
  ComparisonSection,
  FaqSection,
  CtaSection,
} from "@/components/landing";

export default function Page() {
  return (
    <div className="min-h-screen bg-white">
      <main>
        <HeroSection />
        <ShowcaseSection />
        <FeaturesSection />
        <HowItWorksSection />
        <UseCasesSection />
        <ComparisonSection />
        <FaqSection />
        <CtaSection />
      </main>
    </div>
  );
}
