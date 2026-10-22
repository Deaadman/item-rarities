# Building from source

You need the [.NET SDK](https://dotnet.microsoft.com/download) 10 or newer, which CI uses too. The game's assemblies come from NuGet, so you don't need the game to build.

```powershell
dotnet build -c Release
dotnet test --project tests/ItemRarities.Tests
```

The mod is `src/bin/Release/net6.0/ItemRarities.dll`.

To copy it into the game after every Debug build, set `TheLongDarkPath` to the game's folder, as an environment variable or in `Directory.Build.props`:

```powershell
$env:TheLongDarkPath = 'C:\Program Files (x86)\Steam\steamapps\common\TheLongDark'
dotnet build
```

## How it's laid out

- `src/Core`: the parts with rules but nothing from Unity, the game or MelonLoader: the table of item rarities, converting the custom colours between 0 to 255 and the old 0 to 1 scale, reading `.ir` files like `ItemRarities.ir` and the `rarities.ir` in ModComponent mods, and the order sorting by rarity puts items in. It can be [tested](#testing) without the game.
- `src/Enums/Rarities.cs` and `src/Managers/RarityManager.cs`: the mod's public API, which other mods use through the [ItemRarities NuGet package](https://www.nuget.org/packages/ItemRarities) to give their items a rarity. Keep their names and namespaces, or those mods stop working. `RarityManager` also loads the rarities when the main menu first loads.
- `src/Patches`: the [Harmony](https://harmony.pardeike.net) patches on the game, one file for each game type, like `PanelInventoryPatches.cs` for `Panel_Inventory`. Each patch is a private nested class named for what it does.
- `src/Utilities`: what the patches call. `RarityLabels` shows the rarity label, `RarityColours` has each rarity's colour and colours the grids with it, and `RaritySorting` is the sort-by-rarity button. `MissingRarities` logs the game's items without a rarity, for after a game update: uncomment its call in `Mod.cs`.
- `src/Components/MythicGlowEffect.cs`: the glow on Mythic labels.
- `src/Settings.cs`: the mod's page in the game's Mod Settings menu, from [ModSettings](https://github.com/DigitalzombieTLD/ModSettings). The settings are read where they're used, so they take effect straight away.
- `src/Resources`: `ItemRarities.ir`, the rarity of each of the game's items, and `Localization.json`, the rarities' names in each language. Both are embedded in the DLL.
- `src/Dependencies`: DLLs that aren't on NuGet. [Complex Logger](https://github.com/Arkhorse/Complex-Logger), which the mod logs through with `Mod.Logger`, [ModComponent](https://github.com/dommrogers/ModComponent), for its localization, and [ModSettings](https://github.com/DigitalzombieTLD/ModSettings) 2.2.5, for the settings menu, since NuGet only has 1.9.0.
- `src/Properties/AssemblyInfo.cs`: what MelonLoader reads about the mod, including the MelonLoader version it needs.
- `tests/ItemRarities.Tests`: unit tests for `src/Core`.
- `docs`: the pages linked from the README, and `nuget.md`, the NuGet package's page. `assets` has the Photoshop files for the banner and thumbnail.

Every type and member has an XML comment. Keep it that way: the build checks them (see [CI](#ci)).

The settings' field names are what the settings files save, so renaming one resets it for every player. Leave them as they are.

When a setting has to change type or scale, add a new field and move players' saved values to it once, as `Settings.CarryOverOldColours` does for the 0 to 1 colours of 2.0.5 and earlier. The old fields stay, always hidden, so the settings file can still be read.

## Giving an item a rarity

Add its gear item name on its own line under its rarity's `[Rarity]` heading in [`src/Resources/ItemRarities.ir`](../src/Resources/ItemRarities.ir), the same format ModComponent mods use. Lines starting with `#` are comments. An item can only be listed once: the tests fail if one is listed twice, or under a heading that isn't a rarity. After a game update, uncomment `OnSceneWasInitialized` in `src/Mod.cs`, load a save, and `MelonLoader\Latest.log` lists every new item without one. Items that shouldn't have a rarity, like notes and keys, go in the list in `src/Utilities/MissingRarities.cs` instead.

## Versions

The version is `VersionPrefix` in [`src/ItemRarities.csproj`](../src/ItemRarities.csproj). It's the next release's version, so a release changes nothing but the changelog.

Nightly builds add a suffix with the date and commit, like `2.1.0-nightly.20261022+g1a2b3c4`. The game's console shows the version when the mod loads, so a bug report says exactly which build it came from.

## Packages

[`src/packages.lock.json`](../src/packages.lock.json) and the test project's lock file pin every NuGet package, including the ones they depend on. After changing a package version, run `dotnet restore ItemRarities.slnx` and commit the lock file with it.

The first two numbers of the `STBlade.Modding.TLD.Il2CppAssemblies.Windows` version are the game version its assemblies come from. So a new version of it usually means a game update: build against it, then check the mod in the game.

### The NuGet package

Other mods reference the [ItemRarities NuGet package](https://www.nuget.org/packages/ItemRarities) for the public API. The [release workflow](#releasing) publishes it with every release, at the release's version, to nuget.org and to the repository's [GitHub Packages](https://github.com/Deaadman?tab=packages&repo_name=item-rarities). It carries the XML comments, so other mods see them in their editor, and [`docs/nuget.md`](nuget.md) is its page on nuget.org.

To see what it will hold, run `dotnet pack src -c Release` and open `src/bin/Release/ItemRarities.<version>.nupkg` as a zip.

nuget.org lets the workflow publish through [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing), so there's no API key to keep or renew. It needs a policy on nuget.org, set up once by the package's owner: in **Trusted Publishing** under the account menu, add a GitHub Actions policy with the repository owner `Deaadman`, the repository `item-rarities` and the workflow file `release.yml`. GitHub Packages needs nothing: the workflow uses its own token.

## Branches

- `develop` is where work happens. Every push to it publishes a [nightly build](#nightly-builds).
- `main` has the newest release. Merge `develop` into it to release.

## CI

[CI](../.github/workflows/ci.yml) runs on Windows for every push to `main` and every pull request. It checks:

- that `CHANGELOG.md` follows the format below
- formatting, with `dotnet format whitespace --verify-no-changes`. Run `dotnet format whitespace ItemRarities.slnx` to fix it.
- that each `packages.lock.json` matches its project
- that no package has a security advisory: restoring checks them all
- that a Release build works with warnings as errors, and shows the DLL's version and size. That includes the XML comments: public types and members need one, and every `<see cref="..."/>` must point at something that exists.
- that the [unit tests](#testing) pass

## Testing

Most of the mod can only be tested in the game. Everything it touches, down to Unity's `Color` and NGUI's labels, calls into the running game through Il2CppInterop, so it can't run in a test.

The exception is `src/Core`, with nothing from Unity, the game or MelonLoader. [`tests/ItemRarities.Tests`](../tests/ItemRarities.Tests) tests it with [xUnit](https://xunit.net), including that `ItemRarities.ir` loads, lists each item once and has only real rarities as headings:

```powershell
dotnet test --project tests/ItemRarities.Tests
```

The test project compiles the files in `src/Core` itself rather than referencing the mod, along with `src/Enums/Rarities.cs`, which they use. So if something in `src/Core` uses a game type, the tests stop building, and it has to move out. When you add a rule, put it in `src/Core` and test it there, and keep the game-facing part in `src/Utilities` or `src/Patches`.

### In the game

Before a release, and after a game update, load a save and check:

- The inventory shows the selected item's rarity above its name, in its colour, and colours its square and highlight. A Mythic item's label glows.
- The star button sorts the inventory by rarity, and pressing it again flips the order. The same in a container, for both sides.
- A container colours the selected item's highlight.
- The clothing panel colours each slot, including after putting clothing on and taking it off.
- The radial menu shows the hovered item's rarity and colours its segment.
- Inspecting an item shows its rarity with its details.
- The crafting, cooking, milling and examine menus show the selected item's rarity. A decoration in the crafting menu shows none.
- The name of an item under the crosshair is in its rarity's colour.
- With **Custom Colours** on, the colour sliders show, and moving one changes that rarity's colour straight away.
- A settings file from 2.0.5 with custom colours keeps them after updating, on the 0 to 255 sliders.
- An item from a `.modcomponent` mod with a `rarities.ir` shows its rarity.
- The rarities' names are translated after switching the game's language.
- The console shows the mod's version when the game starts, with no errors in `MelonLoader\Latest.log`.

Once a week, [Dependabot](../.github/dependabot.yml) opens a pull request against `develop` for NuGet packages and actions with a newer version, with the minor and patch updates together. Merge it when CI passes.

## Nightly builds

The [nightly workflow](../.github/workflows/nightly.yml) runs on every push to `develop`. It runs CI, builds the mod with the nightly version, and replaces the [`nightly`](https://github.com/Deaadman/item-rarities/releases/tag/nightly) pre-release with it. Its notes are the `## [Unreleased]` section of `CHANGELOG.md`, so testers see what's new, then a link to every commit since the latest release. To preview them, run `.\scripts\release-notes.ps1 -Version Unreleased -OutDir notes`.

## Releasing

The [release workflow](../.github/workflows/release.yml) runs when a version tag is pushed:

1. On `develop`, in [`CHANGELOG.md`](../CHANGELOG.md), rename `## [Unreleased]` to the new version and date, for example `## [2.1.0] - 2026-11-02`. Add a new empty `## [Unreleased]` above it, and update the links at the bottom.
2. Check `VersionPrefix` in `src/ItemRarities.csproj` is the same version. Commit, and merge `develop` into `main`.
3. Tag the commit on `main` and push the tag: `git tag 2.1.0`, then `git push origin 2.1.0`. Tags don't start with `v`.
4. Set `VersionPrefix` on `develop` to the next version, so nightlies after the release are numbered after it.

The workflow then runs CI, checks the tag matches `VersionPrefix`, builds the mod, publishes the [NuGet package](#the-nuget-package) and publishes the release with `ItemRarities.dll`. It stops if `CHANGELOG.md` has no section for that version.

### Writing the changelog

`CHANGELOG.md` follows [Keep a Changelog 2.0.0](https://keepachangelog.com/en/2.0.0/), but without a `v` in front of versions. CI checks the format and says what to fix.

Each version's section is the release's description on GitHub, so write for players:

- Add each change under `## [Unreleased]` as you make it.
- One plain sentence per change, about what's different in the game. "The Festive Sweater is Rare", not "Add GEAR_ChristmasSweater to the Rare list".
- Leave out changes players can't see, like refactors and CI.
- Use full `https://` links. Links like `docs/features.md` break on the release page.

The format rules:

- Use only these headings, in this order, and skip empty ones: `### Added`, `### Changed`, `### Deprecated`, `### Removed`, `### Fixed`, `### Security`.
- Every line under a heading starts with `- `. A long entry can continue on the next line if it's indented.
- A version can start with a short summary before its first heading.
- Start a breaking change with `**Breaking:**`.
- Add ` [YANKED]` after the date of a pulled release. The workflow won't publish it.

The links at the bottom make each heading clickable. `[Unreleased]` compares the newest release with `HEAD`, each version compares with the one before, and the first links to its tag.

To preview a release's notes, run `.\scripts\release-notes.ps1 -Version 2.1.0 -OutDir notes`. It writes the GitHub description to `notes/release-notes.md`. Add `-Check` instead to only check the format, like CI does. The script is shared with [Abalone](https://github.com/Deaadman/abalone): change it there, not here.
