# Item Rarities

[Item Rarities](https://github.com/Deaadman/item-rarities) is a MelonLoader mod for The Long Dark that gives each item a rarity, from Common to Mythic, and shows it in the game's menus. This package lets your mod give its own items a rarity.

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

Players then need [Item Rarities](https://github.com/Deaadman/item-rarities/releases/latest) installed for your mod to load. Mods made with the [ModComponent SDK](https://github.com/Deaadman/mod-component-sdk) can set rarities in the SDK instead, without this package.

[How it works](https://github.com/Deaadman/item-rarities/blob/main/docs/features.md#giving-other-mods-items-a-rarity) has the details, and the [changelog](https://github.com/Deaadman/item-rarities/blob/main/CHANGELOG.md) what's changed in each version.
