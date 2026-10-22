namespace ItemRarities.Components;

/// <summary>
/// Makes a <see cref="Enums.Rarities.Mythic"/> rarity label slowly pulse brighter and dimmer.
/// <see cref="Utilities.RarityLabels"/> adds it to the label, and removes it when the label shows another rarity.
/// </summary>
[RegisterTypeInIl2Cpp(false)]
internal sealed class MythicGlowEffect : MonoBehaviour
{
    /// <summary>The dimmest the label gets, as a multiple of its colour.</summary>
    private const float GlowMin = 0.9f;

    /// <summary>The brightest the label gets, as a multiple of its colour.</summary>
    private const float GlowMax = 1.3f;

    /// <summary>How fast the glow pulses.</summary>
    private const float GlowSpeed = 2f;

    /// <summary>A slight warm tint over the glow.</summary>
    private static readonly Color ShiftColor = new(1.1f, 1.1f, 0.9f);

    /// <summary>The label, or <see langword="null"/> before <see cref="Start"/>.</summary>
    private UILabel? m_Label;

    /// <summary>The label's colour before the glow, put back when the glow stops.</summary>
    private Color m_OriginalColor;

    /// <summary>How far through the pulse the glow is.</summary>
    private float m_Time;

    /// <summary>Puts the label's colour back.</summary>
    private void OnDisable()
    {
        if (m_Label != null)
            m_Label.color = m_OriginalColor;
    }

    /// <summary>Finds the label and remembers its colour.</summary>
    private void Start()
    {
        m_Label = GetComponent<UILabel>();
        m_OriginalColor = m_Label.color;
    }

    /// <summary>Brightens or dims the label for this frame.</summary>
    private void Update()
    {
        if (m_Label is null)
            return;

        m_Time += Time.deltaTime * GlowSpeed;
        var glow = Mathf.Lerp(GlowMin, GlowMax, (Mathf.Sin(m_Time) + 1f) * 0.5f);

        m_Label.color = new Color(
            m_OriginalColor.r * glow * ShiftColor.r,
            m_OriginalColor.g * glow * ShiftColor.g,
            m_OriginalColor.b * glow * ShiftColor.b,
            m_OriginalColor.a);
    }
}
