<div align="center">

<img src="https://raw.githubusercontent.com/Deaadman/item-rarities/main/Images/Banner.webp" alt="Item Rarities Banner">

[![Latest Release](https://img.shields.io/github/v/release/Deaadman/item-rarities?style=for-the-badge&label=Latest%20Release)](https://github.com/Deaadman/item-rarities/releases/latest)

[![All Downloads](https://img.shields.io/github/downloads/Deaadman/item-rarities/total?style=for-the-badge&label=All%20Downloads)](https://github.com/Deaadman/item-rarities/releases)
[![Latest Downloads](https://img.shields.io/github/downloads/Deaadman/item-rarities/latest/total?style=for-the-badge&label=Latest%20Downloads)](https://github.com/Deaadman/item-rarities/releases/latest)

[![Nightly Workflow](https://img.shields.io/github/actions/workflow/status/Deaadman/item-rarities/release_nightly.yml?style=for-the-badge&label=Nightly%20Build)](https://github.com/Deaadman/item-rarities/actions/workflows/release_nightly.yml)
[![Nightly Downloads](https://img.shields.io/github/downloads/Deaadman/item-rarities/nightly/total?style=for-the-badge&label=Nightly%20Downloads)](https://github.com/Deaadman/item-rarities/releases/tag/nightly)

</div>

## Features

Item Rarities is a modification that gives each item within [**The Long Dark**](https://www.hinterlandgames.com/the-long-dark/) a sense of exclusivity.

<details>
    <summary>General Features</summary>

- **Rarity Classifications:** Each item is given a 'rarity' based on several different factors.
- **Exclusivity:** Exclusive items will now 'feel' rarer once obtained.
</details>

<details>
    <summary>UI Features</summary>

- **Sort By Rarity:** You can now sort by the rarity of items in your inventory or containers!
- **Container Grid:** Whenever an item is selected or hovered over in the container grid, then the colour of that grid item changes.
- **Inventory Grid:** Whenever an item is selected or hovered over, it's grid colour changes, and it displays a label above the item.
- **Clothing Grid:** If a clothing item is selected or hovered over, the grid colour changes, and it displays a label above the item.
- **Radial Menu:** The radial menu changes colour based on what item is hovered over, while displaying a label of what rarity it is.
- **Inspect:** The inspect label is now integrated with the rest of the information that fades in whenever an item is inspected.
- **Hovering Label:** Whenever an item is hovered over before picking up, the label changes to the colour of that item's rarity.
- **Crafting Menu:** When a craftable item is selected in the crafting menu, a label displays what rarity it is.
- **Cooking Menu:** If a cookable item is selected within the cooking menu, a label displays what rarity it is.
- **Milling Menu:** Once a millable item is selected within it's menu, a label displays the current rarity of that item.
</details>

<details>
    <summary>Customisable Options</summary>

- **Custom Colours:** Players can now choose any colours for each rarity, to suit them to your liking.
</details>

<details>
    <summary>Modding Support</summary>

- **ModComponent SDK:** Now compatible with any custom items made with the **[ModComponent SDK](https://github.com/Deaadman/mod-component-sdk)**.
- **Programming Support:** Give your custom items custom rarities through programming. Visit the **[developers](https://github.com/Deaadman/item-rarities?tab=readme-ov-file#developers)** section for more information.
</details>

## Showcase

Coming Soon™

## Compatibility

### Required Dependencies
For this mod to work, ensure you have the following mods below:

- [**ModComponent**](https://github.com/dommrogers/ModComponent/releases)
- [**ModSettings**](https://github.com/DigitalzombieTLD/ModSettings)

## Installation

1. [**Download**](https://github.com/LavaGang/MelonLoader/releases/latest/download/MelonLoader.Installer.exe) MelonLoader and install it into your game.
2. [**Download**](https://github.com/Deaadman/item-rarities/releases/latest/download/ItemRarities.dll) the latest version of this mod.
3. Navigate to where your game's directory.
4. Cut the `ItemRarities.dll` you downloaded and paste it into the `mods` folder of your game's directory.
5. Launch the game.

---

## **Contributions**

### Translations
- [**deepsnowland**](https://github.com/deepsnowland) - For providing Japanese translations.
- [**Elderly-Emre**](https://github.com/Elderly-Emre) - For providing Turkish translations.
- **Laki** - For providing Polish translations.
- [**LettereUniche**](https://github.com/LettereUniche) - For providing Italian translations.
- **LordKai1102** - For providing German translations.
- [**Mezav23**](https://github.com/mezav23) - For providing Spanish translations.

---

## **Developers**

This section is for anybody who would like to add a rarity to any of their custom items through code, it's pretty simple - you just have to follow what's below. However, if you are currently using the **[ModComponent SDK](https://github.com/Deaadman/mod-component-sdk)** then you'll be better off using the integrated functionality as seen **[here](https://deaadman.github.io/mod-component-sdk/reference/modsupport)**.

If you have a mod that already contains some code, you'll need to install the **[NuGet](https://www.nuget.org/packages/ItemRarities)** package into your project. After doing so, it's as simple as including this within your project.

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
