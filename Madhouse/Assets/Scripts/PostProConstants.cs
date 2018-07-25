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
}