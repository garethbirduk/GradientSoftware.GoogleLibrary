# Product vision and architecture notes

> Hand-written notes capturing brainstorming and strategic decisions. Not auto-generated.
> The git history records what was built; this records what we figured out.

## The product in one sentence

The structured plan as the through-line of a trip's life cycle — authored in a spreadsheet, lived through with a mobile companion, and remembered as an AI-generated memoir — with all three surfaces sharing one data model and the principle that **code does the doing, free AI does the deciding, remote AI is purely an opt-in bonus**.

## Three surfaces, one data spine

```
                  [BasicEvent — typed plan model]
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
  spreadsheet          calendar UI           mobile companion
  (authoring)        (mobile editing,            (during-trip
                      bidirectional)              triggers + AI)
                              │
                              ▼
                    photos + GPS + actuals
                              │
                              ▼
                  [Narrator — memoir generator]
                              │
                              ▼
            book / blog / slideshow / audiobook
```

### 1. Authoring (now — library shipped)

- Google Sheet, one row per event, columns map to a typed schema via attribute-aliased enum (`EnumEventFieldType`).
- Library compiles a row into `BasicEvent` (or typed subclasses: `FlightEvent`, `TravelEvent`, `AccommodationEvent`).
- Bidirectional sync to Google Calendar — edits in calendar UI flow back to the sheet via `WorksheetCalendarMapping` and the `---` description sentinel.
- Power user wins: copy-paste-row, fill-down, copy-tab-as-variant.
- Spreadsheet stays canonical. Don't replace with a web app — see *Why not a web app* below.

### 2. Companion (during — designed, not built)

- Mobile app that listens for triggers (geofence, time, status change) and proposes contextual actions grounded in the structured plan.
- Reads the calendar that the library writes — so it gets the structured plan for free.
- Status flips written back to calendar (and thence to sheet).
- Photos taken near a Confirmed event get tagged with `EventId` for the narrator's bucket.
- Works offline, no API key, no subscription. Travel-specific surface.

### 3. Narrator (after — designed, not built)

- Plan-vs-actual delta engine.
- Inputs: the *expected* (the plan) plus the *actual* (photo metadata, GPS, optional flight tracking, weather, news).
- Outputs: book / blog / slideshow / audiobook.
- Voice anchored on a user-supplied writing sample — must read as the user, not as ChatGPT.
- Recurring-callback effect ("the gang's third trip together — last time, in Chamonix...") compounds across trips and is the moat.
- Critical: every claim grounded in plan/EXIF/source. Hallucination is unrecoverable for a memoir.

## Architectural principles

### Three-tier intelligence dispatch

| Tier | Job | Cost | Required |
|---|---|---|---|
| **Code** | Triggers, lookups, executions, status writes, schema enforcement | Free | Always |
| **Free on-device AI** (Apple Intelligence / Gemini Nano / bundled small model) | Intent ranking, language composition, classification | Free | When available; falls back to templates |
| **Remote LLM** | Personalised prose, cross-trip synthesis, narrator memoir | Paid | Never for core; deferrable to wifi |

### The product principle (write this on the wall)

> **AI is for cleverness, never for correctness. Core flows must work offline, free, and without an API key.**

Implications:
- Onboarding is one tap (no BYO API key, no subscription wall).
- Foreign-country usability is real (the killer differentiator for travel).
- Privacy is genuine, not marketing.
- No vendor lock-in.
- Quality improves for free as Apple/Google ship better foundation models.

### Two architectural seams worth being explicit about

- **Seam A — "Code dispatches, AI composes":** code owns triggers + intent + execution; AI owns language (compose SMS to mum, caption photo). AI never decides side-effects.
- **Seam B — "Code triggers, AI ranks":** code owns triggers + execution; AI ranks which actions to surface. User is the agency gate.
- Real implementation: both seams layered. Code triggers → AI ranks suggestions → user picks → code executes → AI composes any free text. User stands between AI and any observable action.

### User agency is its own axis

| Posture | When |
|---|---|
| Always propose, never act | Default for new triggers / unfamiliar contexts |
| User-promoted automation | "Always send 'landed ok' to mum on arrival, no confirmation" — opt in per trigger |
| Implicit autonomous action | Don't. Trust is fragile. |

## The library is a *generic event-series engine*, not a trip planner

The spine doesn't care if rows describe flights, fixtures, or oven steps. `BasicEvent` is just "an event"; travel-specific stuff lives in subclasses and extension methods.

### Other domains that fit cleanly

- **Football fixtures** (`MatchEvent`): `Opponent`, `HomeAway`, `Competition`, `Stadium`, `TVChannel`, `TicketStatus`. Reuses dates/times/status/reminders/maps URL.
- **School calendars**: `EventType`, `Year`, `Class`. Plain `BasicEvent` mostly. Pain point solved: schools publish horrid PDFs that someone has to retype.
- **Christmas dinner cook plan** (`CookingStepEvent`): `OvenTemp`, `Equipment`, `Dependency`. Calendar reminders ARE the kitchen timers. No companion, no narrator needed — just the engine. Demonstrates the engine's value standalone.
- **Project schedules / chore rotas / exercise plans** — all natural fits.

### Pack architecture (recommended near-term refactor)

```
Core              ← BasicEvent, dates, status, reminders, locations
└── Travel pack   ← FlightEvent, TravelEvent, AccommodationEvent, IATA, KnownLocations, maps URLs
    (existing — already structured this way, just not labelled)
└── Sport pack (future)
└── Domestic pack (future)        ← cooking, chores
└── Education pack (future)       ← school, courses
```

A 30-min refactor splits travel-flavoured defaults out of the core (e.g., default reminders), making it explicit that other packs slot in the same way.

### Templates as a marketable asset

The library reads any compatible sheet — so community-contributed sample sheets become the onboarding for non-spreadsheet-natives:

- "Premier League 2025–26 (Liverpool)"
- "Year 7 calendar — Manchester Grammar School"
- "Christmas dinner for 8 — Delia's recipe"

Users clone, customise, push to calendar. WordPress-shaped: core engine + domain themes + community marketplace.

## Why *not* a web app

The spreadsheet is the right input format because:
- Insert-row, fill-down, copy-paste-row, copy-tab-as-variant are unbeatable primitives for power planners.
- Formulas, real-time multi-edit, version history come for free.
- Mobile-poor, desktop-excellent — and Google Calendar IS the mobile editor (round-trip handles it).

A web app *replaces* the sheet only for the narrator-buyer audience (non-power-users), and only via:
- Email/confirmation parsing (TripIt-style)
- Or a guided wizard

These are *separate products consuming the same library*. Don't conflate authoring power-users (the user) with narrator buyers (everyone else) — one tool for both is a worse tool for both.

### Thin layers that *augment* without replacing

- **Apps Script bound to the sheet** — "Publish to calendar" button, header validation, IATA autocomplete. Highest payoff per hour.
- **CLI / batch tool** — `dotnet run -- publish itinerary.xlsx --variant=LHR-EWR`. Power users prefer this over a button.
- **Read-only web/PDF view for sharing** — already converges with the narrator's output surface.

## Audience segmentation

| Audience | Primary surface | Buys what |
|---|---|---|
| Power-user planners (the user) | Spreadsheet | Free; engine for own use |
| Trip-takers in the user's circle | Companion app | Free with optional premium |
| Memoir buyers | Narrator output | Pays for the memoir |

Shared engine, no paywalled core, deliberate upgrade path.

## Differentiation vs incumbents

- **Google Travel / Calendar / Maps** — passive (show you stuff). No structured plan that's actually authored, no acting on it.
- **TripIt / Wanderlog** — has the plan, doesn't act on it during the trip, doesn't generate narrative output.
- **Photo apps (Apple Memories, Google Photos Memories)** — only have actuals, no plan. Have to guess at structure.
- **AI travel apps (Roam Around, Mindtrip)** — generate plans but require subscription, no offline, no companion, no narrative.

The plan-as-structured-data is the moat. Every existing tool either has the plan and doesn't act on it, or acts on context but has no plan. Only this design has both.

## Open questions / parking-lot

- **`Variant` column + per-variant calendar.** Small implementation. Unblocks "3 routings to USA" what-if comparison. Probably the next functional commit.
- **Date-shift simulation.** `WorksheetToCalendarAsync(..., dateOffset: TimeSpan.FromDays(7))`. Tiny. Useful for "same plan, next week".
- **Comparison report.** Pure-functional helper outputting Markdown. Cost / overlap / "could-see-who" analysis. Console runner first; UI later if needed.
- **"Who's nearby" detection.** Generalise `KnownLocations` to contacts' addresses, filter by event-location proximity. Strong differentiator for travel.
- **Title vs Summary still has FlightEvent's "always overwrite" inconsistency** vs Travel/Accommodation's "fall back when empty". `UserTitle` preserves user input but the *consistency* question is open. Decided to ship as-is; revisit if anyone trips on it.
- **Repositioning README** to reflect the generic-engine framing (not just trip planning). Wider TAM, clearer architectural story.
- **Companion MVP scope.** Single trigger (geofence at airport) → single action (suggest pre-filled SMS to first `ContactableAttendees`). Weekend project. Validates whether contextual-action UX feels good before investing further.
- **Narrator MVP scope.** Single past trip, photos with EXIF, output Markdown. Show to 5 friends from the trip. If they laugh / want to share / would pay £30 — proceed.
- **Plan acquisition for non-power-users.** Email/confirmation parsing. Future product question; only relevant after narrator validates.
- **Service-account integration test fixture** so `GoogleServices.Test` runs in CI. Currently `[TestCategory("Manual")]`-gated. Real value but not blocking anything.

## Library state (snapshot at this point)

- `GoogleLibrary` + `GoogleServices` NuGet packages on branch `42-__breaking-rewrite` (13 commits ahead of `main`).
- 179/179 unit tests passing. `WarningsAsErrors=nullable`. Build is clean modulo PostSharp aspect-ordering warnings (12) and one `SYSLIB0051` (none from our code).
- Bidirectional sheet⇄calendar with metadata-preserving description format (`---` sentinel).
- Schema doc auto-generated, drift-detected via unit test.
- `KnownLocations` registry (env-var or `Register()`, no hardcoded addresses).
- `EventBuilder.Create(Event)` Google→Basic round-trip implemented.
- Title/Summary collapsed to Title only; `UserTitle` custom-field preserves user input on FlightEvent.
- `TrimRepetitive` (typo fixed); `RequiredScopes` tightened to `IReadOnlyList<string>`.

## Remaining technical debt worth flagging

- PostSharp `RequiredAttribute` ↔ `Gradient.Utils PreconditionAttribute` aspect-ordering — pick one or assign explicit priorities.
- `SYSLIB0051` obsolete-API in `GoogleEventBuilderException.cs`.
- Service-account integration test fixture (above).
- Pack architecture refactor (above) — ~30 min for the core/travel split.
