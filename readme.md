# BASE.Modules.SvcProspectus

Service marketing and brochure content module.

## Purpose

Prospectus provides the "About the Service" content:
- **Features** - Service capabilities and benefits
- **Team** - Team member profiles
- **Timelines/Roadmap** - Release history and upcoming
- **Pricing Overview** - Tier descriptions (not billing)
- **Featured FAQs** - Curated from Supports (dynamic)
- **Testimonials** - Customer quotes from Supports

## Module Dependencies

```
Sys
   ↓
Messaging
   ↓
Membership
   ↓
Social
   ↓
Supports (source of FAQs, Testimonials)
   ↓
SvcProspectus ← YOU ARE HERE (display layer)
```

## References to Other Modules

Prospectus consumes content from Supports:

```xml
<!-- In SvcProspectus.Application.csproj -->
<ProjectReference Include="..\..\..\BASE.Modules.Supports\SOURCE\App.Modules.Supports.Shared\App.Modules.Supports.Shared.csproj" />
<ProjectReference Include="..\..\..\BASE.Modules.Supports\SOURCE\App.Modules.Supports.Interfaces.Models\App.Modules.Supports.Interfaces.Models.csproj" />
```

## Key Insight

**Prospectus is a VIEW into Supports, not a separate data store.**

- FAQs live in Supports → Prospectus displays featured ones
- Testimonials live in Supports → Prospectus displays approved ones
- Usage stats determine relevance → stale content naturally fades

## Key Entities

- `Feature` - Service capability with description, icon
- `TeamMember` - Profile for About page
- `PricingTier` - Tier description (display only)
- `RoadmapItem` - Timeline/release item
 

