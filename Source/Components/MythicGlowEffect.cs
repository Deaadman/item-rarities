namespace ItemRarities.Components;

[RegisterTypeInIl2Cpp(false)]
internal class MythicGlowEffect : MonoBehaviour
{
    private UILabel? labelMythic;
    private Color originalColor;
    private float time;
    
    private const float glowMin = 0.9f;
    private const float glowMax = 1.3f;
    private const float glowSpeed = 2f;
    
    private static readonly Color shiftColor = new(1.1f, 1.1f, 0.9f);
    
    private void OnDisable()
    {
        if (labelMythic != null)
        {
            labelMythic.color = originalColor;
        }
    }
    
    private void Start()
    {
        labelMythic = GetComponent<UILabel>();
        originalColor = labelMythic.color;
    }

    private void Update()
    {
        if (labelMythic is null)
            return;
        
        time += Time.deltaTime * glowSpeed;
        var glow = Mathf.Lerp(glowMin, glowMax, (Mathf.Sin(time) + 1f) * 0.5f);
        
        var newColor = new Color(
            originalColor.r * glow * shiftColor.r,
            originalColor.g * glow * shiftColor.g,
            originalColor.b * glow * shiftColor.b,
            originalColor.a
        );
        
        labelMythic.color = newColor;
    }
}