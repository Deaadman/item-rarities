# How it works

Every item in the game with a use has a rarity. Notes, keys, postcards and other story items don't, and show no label or colour. Which rarity each item has is in [`src/Resources/ItemRarities.ir`](../src/Resources/ItemRarities.ir).

Decorations you can carry, like picture frames, signs, rugs and mounted antlers, have a rarity too, in [`src/Resources/Decorations.ir`](../src/Resources/Decorations.ir). They show it in the inventory and containers, and sort with items.

## The rarities

From the most common to the rarest, with their colours:

| Rarity | Colour | For |
| --- | --- | --- |
| Common | Grey | Items that are easy to find. |
| Uncommon | Green | Items that are easy to find, but worth more. |
| Rare | Blue | Items that are harder to come across. |
| Epic | Purple | Items that are harder to come across, and worth more. |
| Legendary | Orange | Items that are worth a lot and hard to find. |
| Mythic | Gold | The rarest items, nearly impossible to find. Their label glows. |

The rarities' names are translated into English, German, Italian, Japanese, Polish, Russian, Spanish, Traditional Chinese, Turkish and Ukrainian.

## Where rarities show

- **Inventory**: the selected item's rarity above its name in the details, and its square and highlight in the rarity's colour. Hovering over a square colours it too.
- **Sorting**: a star button beside the inventory's and container panel's sort buttons sorts by rarity, the rarest first. Press it again to put the rarest last.
- **Containers**: the selected item's highlight in its rarity's colour.
- **Clothing**: each slot in its item's rarity's colour, when selected or hovered over.
- **Radial menu**: the hovered item's rarity above its name, and its segment in the rarity's colour.
- **Inspecting**: the rarity fades in with the rest of the item's details.
- **In the world**: the name of the item under your crosshair in its rarity's colour.
- **Crafting, cooking, milling and examining**: the selected item's rarity above its name. Decorations have none.

## Settings

To change them, open **Options**, then **Mod Settings**, then **Item Rarities**. They're saved in `Mods\ItemRarities.json`, and take effect straight away.

| Setting | Default | What it does |
| --- | --- | --- |
| Custom Colours | Off | Use your own colour for each rarity. Shows a red, green and blue slider for each one. |
| Red, Green, Blue | The colours above | Each rarity's colour, from 0 to 255, with Custom Colours on. |

Colours set in Item Rarities 2.0.5 and earlier, on a 0 to 1 scale, move to the 0 to 255 sliders the first time the game starts with a newer version.

## Giving other mods' items a rarity

Items from other mods have no rarity until their mod gives them one. There are two ways.

### With the ModComponent SDK

Mods made with the [ModComponent SDK](https://github.com/Deaadman/mod-component-sdk) set rarities in the SDK, as its [wiki](https://github.com/Deaadman/mod-component-sdk/wiki/External-Mod-Support#item-rarities) explains. The SDK writes them to a `rarities.ir` file inside the `.modcomponent` file, which Item Rarities reads when the main menu first loads:

```ini
# Lines starting with # are skipped.
[Rare]
GEAR_ExampleItem
GEAR_ExampleItem2

[Mythic]
GEAR_ExampleItem3
```

Each name gets the rarity in brackets above it: `Common`, `Uncommon`, `Rare`, `Epic`, `Legendary` or `Mythic`.

### With code

Reference the [ItemRarities NuGet package](https://www.nuget.org/packages/ItemRarities), then give each item a rarity when your mod starts:

```csharp
using ItemRarities.Enums;
using ItemRarities.Managers;

internal sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        RarityManager.AddGearItemAndRarity("GEAR_ExampleItem", Rarities.Mythic);
        RarityManager.AddGearItemAndRarity("GEAR_ExampleItem2", Rarities.Legendary);
    }
}
```

Players then need Item Rarities installed for your mod to load.

Item Rarities' own list loads after other mods start, and replaces their rarity for any of the game's own items. The `.modcomponent` files load last, and can replace anything.
