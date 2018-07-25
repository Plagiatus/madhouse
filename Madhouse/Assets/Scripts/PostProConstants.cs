using UnityEngine.Rendering.PostProcessing;

static class PostProConstants
{
    //Depth of Fieled
    public const float focusDistance = 1f;
    public const float aperature = 5.5f;
    public const float normalFocusLength = 35f;
    public const float insaneFocusLength = 50f;

    //Color Grading
    public const GradingMode gradingMode = GradingMode.LowDefinitionRange;
    public const float temperature = 40f;
    public const float saturation = 75f;
    public const float contrast = 10f;

    //Bloom
    public const float bloom_Threshold = 0.8f;
    public const float bloom_IntesityNormal = 5f;
    public const float bloom_IntensityInsane = 10f;
    public const float dirt_IntensityInsane = 3f;
    public const float dirt_IntesityNormal = 0f;

    //Vignette
    public const VignetteMode vignetteMode = VignetteMode.Masked;

    //Grain
    public const float grain_IntensityNormal = 0f;
    public const float grain_IntensityInsane = 0.5f;
    public const float grain_size = 1.3f;
    public const float luminance_ContributionMin = 0f;
    public const float luminance_ContributionMax = 0.2f;

    //Lense Distortion
    public const float maxDistortion = 50f;
}