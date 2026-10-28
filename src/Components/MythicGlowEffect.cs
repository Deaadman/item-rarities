namespace ItemRarities.Components;

/// <summary>
/// Makes a <see cref="Enums.Rarities.Mythic"/> rarity label, or the inspect view's star, slowly pulse brighter and
/// dimmer. <see cref="Utilities.RarityLabels"/> adds it, and removes it when they show another rarity.
/// </summary>
[RegisterTypeInIl2Cpp(false)]
internal sealed class MythicGlowEffect : MonoBehaviour
{
    /// <summary>The dimmest the label or star gets, as a multiple of its colour.</summary>
    private const float GlowMin = 0.9f;

    /// <summary>The brightest the label or star gets, as a multiple of its colour.</summary>
    private const float GlowMax = 1.3f;

    /// <summary>How fast the glow pulses.</summary>
    private const float GlowSpeed = 2f;

    /// <summary>A slight warm tint over the glow.</summary>
    private static readonly Color ShiftColor = new(1.1f, 1.1f, 0.9f);

    /// <summary>The label or star, or <see langword="null"/> before <see cref="Start"/>.</summary>
    private UIWidget? m_Widget;

    /// <summary>The label or star's colour before the glow, put back when the glow stops.</summary>
    private Color m_OriginalColor;

    /// <summary>How far through the pulse the glow is.</summary>
    private float m_Time;

    /// <summary>Puts the label or star's colour back.</summary>
    private void OnDisable()
    {
        if (m_Widget != null)
            m_Widget.color = m_OriginalColor;
    }

    /// <summary>Finds the label or star and remembers its colour.</summary>
    private void Start()
    {
        m_Widget = GetComponent<UIWidget>();
        m_OriginalColor = m_Widget.color;
    }

    /// <summary>Brightens or dims the label or star for this frame.</summary>
    private void Update()
    {
        if (m_Widget is null)
            return;

        m_Time += Time.deltaTime * GlowSpeed;
        var glow = Mathf.Lerp(GlowMin, GlowMax, (Mathf.Sin(m_Time) + 1f) * 0.5f);

        m_Widget.color = new Color(
            m_OriginalColor.r * glow * ShiftColor.r,
            m_OriginalColor.g * glow * ShiftColor.g,
            m_OriginalColor.b * glow * ShiftColor.b,
            m_OriginalColor.a);
    }
}
