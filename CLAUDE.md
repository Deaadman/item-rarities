# CLAUDE.md

Item Rarities is a MelonLoader mod for The Long Dark that gives each item a rarity, from Common to Mythic, and shows it in the game's menus. It patches the game with Harmony and ships as one `ItemRarities.dll`. Other mods use its public API to give their items a rarity.

## Build and check

```powershell
dotnet build -c Release
dotnet test --project tests/ItemRarities.Tests
dotnet format whitespace ItemRarities.slnx
.\scripts\release-notes.ps1 -Check
```

- MelonLoader is pinned to **0.7.2** (`MelonLoaderVersion` in the csproj's `GenerateBuildInfo` target), which runs mods on .NET 6. Keep `TargetFramework` at `net6.0` and don't bump MelonLoader incidentally. Any newer SDK (10, 11) is fine to build with.
- Warnings are errors, including missing XML comments and broken `cref`s. Every type and member has an XML comment.
- `docs/development.md` has the full layout, CI, nightly and release process.

## Game code

- `game-refs/` (git-ignored, never commit it or paste large parts of it: it's Hinterland's code) is the game decompiled with Cpp2IL and call analysis. Method bodies are mostly stubs listing the calls they make, not real logic. Use it for type, field and method names, signatures, and what calls what; infer behaviour carefully and say when you're guessing.
- The compiled interop assemblies (`STBlade.Modding.TLD.Il2CppAssemblies.Windows`, in the NuGet cache) are what the mod builds against. If they and `game-refs` disagree, they win.
- Game types are in the `Il2Cpp` namespace. Unity objects need `== null`, not `?.` or `is null`, to detect destroyed objects.

## Conventions

- Patches live in `src/Patches/<GameType>Patches.cs`, as private nested classes named for what they do. The instance parameter must be `__instance`. Prefer `Postfix` unless the original must be blocked or replaced. Anything with more to it than a patch goes in `src/Utilities`, and its patches call into it.
- Rules go in `src/Core`: plain C#, no Unity, game or MelonLoader types, so `tests/ItemRarities.Tests` can test it (it compiles those files directly, with `src/Enums/Rarities.cs`). Game-facing code calls into it.
- `Rarities` (`src/Enums`) and `RarityManager.AddGearItemAndRarity` (`src/Managers`) are the public API other mods compile against, through the ItemRarities NuGet package. Never rename, move or change them, or reorder the rarities. Everything else is `internal`.
- The game's items' rarities are in `src/Resources/ItemRarities.ir`, in the same `.ir` format ModComponent mods use for the `rarities.ir` inside their `.modcomponent` file. Keep reading that format as it is: mods already ship it.
- Settings are in `src/Settings.cs` and read where they're used, so changes apply straight away. To change a setting's type or scale, add a new field and carry saved values over once (see `CarryOverOldColours`); keep the old field, hidden.
- Translated text goes in `src/Resources/Localization.json` and is read with `Localization.Get`.
- Log with `Mod.Logger.Log(message, FlaggedLoggingLevel.X)` (Complex Logger, from `src/Dependencies`). Don't use `MelonLogger` directly.
- Fields are `m_Name` (instance) and `s_Name` (static), constants and static readonly fields `PascalCase`, matching the game. The exception is the settings fields: their names are what the settings file saves, so never rename them. Global usings are in the csproj.
- Change gameplay only when asked; refactors keep behaviour identical. Check changes in game where possible: tests can't cover patches.

## Changes and releases

- Work happens on `develop`; every push publishes a nightly. `main` has the latest release, published by pushing a tag like `2.1.0` (no `v`) that matches `VersionPrefix`. The release also publishes the NuGet package, to nuget.org and GitHub Packages.
- Add each player-visible change to `## [Unreleased]` in `CHANGELOG.md` as one plain sentence for players. CI checks the format.
- Never add Claude or any AI as a co-author: no `Co-Authored-By` trailer or attribution footer in commits or pull requests.
