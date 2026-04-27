# Spreadsheet Schema

A row in your authoring spreadsheet maps to one calendar event. Column headers can use any of the canonical names or aliases listed below; column ordering is preserved on round-trip via metadata stored in the calendar's description.

> The **Event field columns** table below is auto-generated from `EnumEventFieldType` and its `[Alias]` attributes. To refresh it after editing the enum, run:
>
> ```
> UPDATE_SCHEMA_DOCS=1 dotnet test --filter SchemaDocsAreUpToDate
> ```
>
> CI runs the same test without the env var; it fails if this file drifts from the enum.

## Authoring conventions

- One row per event. Empty rows (no Summary) are skipped.
- The first row is the header row by default. Pass a higher `headerRowsCount` if your sheet has multiple title/heading rows above the column headers.
- Unknown column names round-trip via the calendar event's description as custom-field key/value pairs (e.g. a `Booking` column becomes `Booking: ABC123` inside the description).

## Event field columns

<!-- BEGIN AUTO: fields -->
| Field | Aliases |
|---|---|
| `None` | _(none)_ |
| `Unknown` | _(none)_ |
| `Summary` | `Title`, `Event`, `Name` |
| `StartDate` | `Start Date`, `Date`, `Date1` |
| `StartTime` | `Start Time`, `Time`, `Time1` |
| `EndTime` | `End Time`, `Time2` |
| `EndDate` | `End Date`, `Date2` |
| `From` | `Start`, `Location`, `Location1` |
| `Description` | `Notes` |
| `Reminders` | `Reminder`, `Minutes`, `Notifications` |
| `Category` | `Type`, `Event Type`, `EventType` |
| `Status` | _(none)_ |
| `To` | `End`, `Location2` |
| `Via` | _(none)_ |
| `Via2` | _(none)_ |
| `FromAddress` | _(none)_ |
| `ToAddress` | _(none)_ |
| `ViaAddress` | _(none)_ |
| `ViaAddress2` | _(none)_ |
| `FlightNumber` | `Flight`, `Flight Number`, `Flight Nr`, `Flight No`, `FlightNo`, `FlightNr` |
| `FlightCarrier` | `Carrier`, `Flight Carrier` |
| `ContactableAttendees` | `Who` |
| `PriceDollars` | `$`, `USD` |
| `PricePounds` | `£`, `GBP` |
| `PriceEuros` | `E`, `EUR` |
<!-- END AUTO: fields -->

## Status column / annotation prefix

The `Status` column accepts one of the values below. On the calendar, status is reflected via a single-character prefix on the event summary and a colour:

| Status      | Prefix    | Calendar colour |
|-------------|-----------|-----------------|
| Idea        | `(i)`     | Orange          |
| Planned     | `(p)`     | Yellow          |
| Confirmed   | _(none)_  | Green           |
| Reserved    | `(r)`     | Green           |
| Paid        | `(£)`     | Cyan            |
| Cancelled   | `(x)`     | Red             |

When reading a calendar event back, the prefix is the source of truth; ColorId is a fallback (and is ambiguous between Confirmed and Reserved).

## Category column

The `Category` column dispatches to specialised event types with their own per-type defaults:

| Category                | Event type           | Default reminders |
|-------------------------|----------------------|-------------------|
| _(blank or unrecognised)_ | BasicEvent         | 1 hour            |
| `Drive`, `Train`, `Taxi` | TravelEvent         | 1h, 2h            |
| `Flight`                | FlightEvent          | 2h, 4h            |
| `Accommodation`         | AccommodationEvent   | 12h               |

Per-row reminders in a `Reminders` column override the per-type defaults. Comma-separated minutes, e.g. `60, 120`.

## Locations and IATA expansion

Three-letter IATA codes (e.g. `LHR`, `JFK`) in `From` / `To` columns are auto-expanded to full airport names via the OpenFlights dataset. Flight events use the IATA codes in their route summary (`LHR - JFK`); non-flight events use the full airport name. When a flight has one airport endpoint and one non-airport endpoint, the airport address gets ` Arrivals` or ` Departures` appended automatically.

Names registered via `KnownLocations.Register("Home", "<address>")` (or the `KNOWN_LOCATION_HOME` environment variable) are resolved to their registered address whenever they appear in a location column.

## Map URLs

The calendar event's `Location` field gets a Google Maps URL composed from the address column(s):

- One address → `https://www.google.com/maps/search/<addr>` (renders a pin)
- Multiple → `https://www.google.com/maps/dir/<a>/<b>/...` (renders a route)

Spaces are encoded as `+`; other reserved characters (`?`, `#`, `&`, non-ASCII, etc.) are percent-encoded so they survive the URL.

## Round-trip

`WorksheetToCalendarAsync` writes the original column ordering into the calendar's description as a `Headers*/*<col1>*/*<col2>*/*...` line. `CalendarToWorksheetAsync` reads it back and reproduces the same column order; if the line is absent (e.g. the calendar wasn't authored by this library), it falls back to a default minimal header set.
