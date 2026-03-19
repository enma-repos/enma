import {
  HeroSection,
  ProductPreviewSection,
  FeaturesSection,
  HowItWorksSection,
  FaqSection,
} from "@/components/landing";

export default function Page() {
  return (
    <div className="min-h-screen bg-white">
      <main>
        <HeroSection />
        <ProductPreviewSection />
        <FeaturesSection />
        <HowItWorksSection />
        <FaqSection />
      </main>
    </div>
  );
}
