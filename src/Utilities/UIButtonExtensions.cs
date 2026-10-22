using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace ItemRarities.Utilities;

/// <summary>Helpers for the game's NGUI buttons.</summary>
internal static class UIButtonExtensions
{
    /// <summary>Adds a sort button to a panel by copying its last one.</summary>
    /// <param name="uiButtonsArray">The panel's sort buttons.</param>
    /// <param name="buttonName">The new button's game object name.</param>
    /// <param name="spriteName">The new button's icon, from the game's atlas.</param>
    /// <param name="uiButtonMethod">What pressing it calls, with the button.</param>
    /// <param name="posX">The new button's x position.</param>
    /// <param name="posY">The new button's y position.</param>
    /// <param name="posZ">The new button's z position.</param>
    internal static void NewSortButton(Il2CppReferenceArray<UIButton> uiButtonsArray, string buttonName, string spriteName, Action<UIButton> uiButtonMethod, float posX = 0f, float posY = 0f, float posZ = 0f)
    {
        UIButton temporaryUIButton = new();

        foreach (var button in uiButtonsArray)
            temporaryUIButton = button;

        var newButton = temporaryUIButton.Instantiate();
        newButton.name = buttonName;
        newButton.SetSpriteName(spriteName);
        EventDelegate.Set(newButton.onClick, new Action(() => uiButtonMethod(newButton)));
        newButton.transform.localPosition = new Vector3(posX, posY, posZ);

        uiButtonsArray.AddItem(newButton);
    }

    /// <summary>Copies a button, beside the original and at the same scale as its parent.</summary>
    /// <param name="original">The button to copy.</param>
    /// <returns>The copy.</returns>
    private static UIButton Instantiate(this UIButton original)
    {
        var copy = UnityEngine.Object.Instantiate(original.gameObject).GetComponent<UIButton>();
        copy.transform.parent = original.transform.parent;
        copy.transform.localPosition = original.transform.localPosition;
        copy.transform.localScale = Vector3.one;
        return copy;
    }

    /// <summary>Changes a button's icon.</summary>
    /// <param name="btn">The button.</param>
    /// <param name="newSpriteName">The icon, from the game's atlas.</param>
    private static void SetSpriteName(this UIButton btn, string newSpriteName)
    {
        var sprite = btn.GetComponent<UISprite>();
        sprite.spriteName = newSpriteName;
        sprite.OnInit();
    }
}
