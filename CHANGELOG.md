# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/2.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!--
A version's section is its GitHub release description, so write it for
players: one plain sentence each, about what changed for them.

- Add entries under "## [Unreleased]" as you go, grouped under Added,
  Changed, Deprecated, Removed, Fixed or Security.
- To release, rename it to "## [2.1.0] - YYYY-MM-DD" (no v), add a fresh
  "## [Unreleased]" above it, and update the links at the bottom.

See docs/development.md, "Releasing". CI checks this file's format.
-->

## [Unreleased]

### Added

- **Breaking:** The mod logs through [Complex Logger](https://github.com/Arkhorse/Complex-Logger), which it now needs. Turn on its debug levels when reporting a bug.
- Traditional Chinese translations, thanks to [WPNsmglr](https://github.com/WPNsmglr) ([#21](https://github.com/Deaadman/item-rarities/pull/21)).
- Ukrainian translations, thanks to [ERRORES241](https://github.com/ERRORES241) ([#24](https://github.com/Deaadman/item-rarities/pull/24)).
- Rarities for the deerskin coat, gloves and hat (Epic) and the broken manufactured arrow (Rare).
- Decorations you can carry, like picture frames, signs, rugs and mounted antlers, now have rarities, shown and sorted in the inventory and containers like items.
- Rarities for Wintermute's gear, like Will's and Astrid's clothes, the Hunting Lodge rifle and the machine parts, and for the Tall Tales and Halloween event items, like the Tall Tales books and cape and the pumpkin pies.
- Rarities for the Iron Road items: the whale oil heater and the train's brake lever, brake pad and throttle lever (Epic), and the whale oil can and the new bear and wolf meat (Rare).
- A star in the rarity's colour beside the rarity when inspecting an item, like the icons beside its condition and weight.

### Changed

- The custom colour sliders go from 0 to 255 instead of 0 to 1. Colours you set before carry over ([#20](https://github.com/Deaadman/item-rarities/issues/20)).
- 27 items have a new rarity, so cooked food matches the raw food, teas match their ingredients and everyday food like jerky and crackers is no longer Epic or Legendary.

### Fixed

- The rarity in the inspect view lines up on the left with the item's other details, instead of sitting off to the side.
- A container keeps the selected item's highlight in its rarity's colour after you move the last item off the other side, instead of turning white until you select it again.
- Item Rarities loads alongside Disable Breath Effects, TLD_NonPotableToiletWater, Unlimited Rock Caches and Container Tweaks, instead of refusing to.

## [2.0.5] - 2026-01-09

### Added

- The Festive Sweater is Rare.

### Changed

- Updated for The Long Dark 2.51.
- The mod needs [ModComponent](https://github.com/dommrogers/ModComponent/releases) for its translations, instead of LocalizationUtilities.

## [2.0.4] - 2024-12-17

### Added

- Rarities for the items added since The Long Dark 2.33.

### Changed

- Updated for The Long Dark 2.36.

### Fixed

- Selecting a decoration in the inventory or a container no longer causes errors.

## [2.0.3] - 2024-10-16

### Added

- Russian translations, thanks to [Lev234](https://github.com/Lev234).

## [2.0.2] - 2024-08-11

### Added

- Spanish translations, thanks to [Mezav23](https://github.com/mezav23).
- German translations, thanks to LordKai1102.

## [2.0.1] - 2024-07-20

### Added

- The sort-by-rarity button in containers.
- The selected item in a container is coloured with its rarity.

### Removed

- The black outline around the rarity labels.

### Fixed

- Clothing without a rarity can be selected again after it's put on.
- The rarity label no longer disappears for good after selecting an item without a rarity.
- The inventory grid shows properly when an item without a rarity is hovered over or selected.
- The radial menu's highlight shows when an item without a rarity is hovered over.
- Several errors in the console.

## [2.0.0] - 2024-07-13

A rewrite of the whole mod.

### Added

- Selected items in the inventory and clothing panels are coloured with their rarity.
- Hovering over an item in the inventory, the clothing panel or the radial menu colours it with its rarity.
- Settings to choose each rarity's colour.
- Other mods can give their items a rarity, with code or the [ModComponent SDK](https://github.com/Deaadman/mod-component-sdk).
- The Mythic label glows.
- Polish translations, thanks to Laki.

### Changed

- The rarity shows with the rest of an item's details when you inspect it.

### Removed

- The colour-blindness options.

## [1.1.4] - 2023-11-17

### Added

- Japanese translations, thanks to [deepsnowland](https://github.com/deepsnowland).

### Fixed

- The rarity label no longer shows twice when switching between panels.

## [1.1.3] - 2023-10-26

### Fixed

- The rarity label no longer says there's no translation.

## [1.1.2] - 2023-10-12

### Changed

- Updated for The Long Dark 2.24.

## [1.1.1] - 2023-09-27

### Added

- Italian translations, thanks to [LettereUniche](https://github.com/LettereUniche).

### Changed

- The rarity label's text when there's no translation for it.

## [1.1.0] - 2023-09-26

### Added

- Translations, starting with Turkish, thanks to [Elderly-Emre](https://github.com/Elderly-Emre).
- Colour-blindness options, which change the rarities' colours.

### Changed

- The rarities' colours are slightly different.

### Fixed

- The rarity label shows for items in the cooking menu.

## [1.0.0] - 2023-09-15

### Added

- The first release.

[Unreleased]: https://github.com/Deaadman/item-rarities/compare/2.0.5...HEAD
[2.0.5]: https://github.com/Deaadman/item-rarities/compare/2.0.4...2.0.5
[2.0.4]: https://github.com/Deaadman/item-rarities/compare/2.0.3...2.0.4
[2.0.3]: https://github.com/Deaadman/item-rarities/compare/2.0.2...2.0.3
[2.0.2]: https://github.com/Deaadman/item-rarities/compare/2.0.1...2.0.2
[2.0.1]: https://github.com/Deaadman/item-rarities/compare/2.0.0...2.0.1
[2.0.0]: https://github.com/Deaadman/item-rarities/compare/1.1.4...2.0.0
[1.1.4]: https://github.com/Deaadman/item-rarities/compare/1.1.3...1.1.4
[1.1.3]: https://github.com/Deaadman/item-rarities/compare/1.1.2...1.1.3
[1.1.2]: https://github.com/Deaadman/item-rarities/compare/1.1.1...1.1.2
[1.1.1]: https://github.com/Deaadman/item-rarities/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/Deaadman/item-rarities/compare/1.0.0...1.1.0
[1.0.0]: https://github.com/Deaadman/item-rarities/releases/tag/1.0.0
