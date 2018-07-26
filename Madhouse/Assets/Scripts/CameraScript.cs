using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
	public bool inInventory = false;
	private bool inTransition = false;
	[Range(-20,80)]
	private float sanity;
	private float targetSanity;
    private PostProcessVolume PostProcess;
	private Vector3 temp1;

    private float currentBloomIntensity;
    private float currentDirtIntensity;
    public float PulseThreshhold = 0.7f;
    public Texture Insanity_Effect;

    private float currentDistortion;
    private float targetDistortion;
    private float distortionThreshhold = 0.1f;
    private float distortionGrowth = 1f;
    private Animator animator;

    void Start () {
		offset = this.transform.position - player.transform.position;
        PostProcess = this.GetComponent<PostProcessVolume>();
        offset = this.transform.position - player.transform.position;
        temp1 = this.transform.localPosition;
        currentBloomIntensity = PostProConstants.bloom_IntesityNormal;
        currentDirtIntensity = PostProConstants.dirt_IntesityNormal;
        animator = GetComponent<Animator>();
    }

	void Update() {

        sanity = Mathf.Lerp(sanity, targetSanity, Time.deltaTime);
        //Debug.Log("Insanity at " + sanity);
        //Debug.Log("Target Sanity at " + targetSanity);
        distortImage();
        // moveCameraBehindPlayer();
	}

    void FixedUpdate(){
        moveCameraBehindPlayer();
    }

	private void moveCameraBehindPlayer(){
        if(inInventory) return;
        // Debug.Log("moveCam");
        RaycastHit hit;
        Vector3 hitPoint;
        
        Vector3 rayDirection = (player.transform.forward * -1);
        Ray ray = new Ray(player.transform.position + Vector3.up * 1.4f + player.transform.right, Quaternion.AngleAxis(15, player.transform.right) * rayDirection);
        // Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);
        // Debug.Log(ray.direction);
        // Debug.Log(Physics.Raycast(ray, out hit, 2.5f));
        if (Physics.Raycast(ray, out hit, 2.5f)){
            hitPoint = hit.point;
            // Debug.Log("hit");
            // Debug.Log(hitPoint);
            // worldposition auf lokalposition setzen
            // this.transform.position = hitPoint;
            // Debug.Log(this.transform.localPosition);
            this.transform.localPosition = player.transform.InverseTransformPoint(hitPoint);
            //Debug.Log(player.transform.localPosition);
        }
        // camera auf standard setzen
        else { 
            this.transform.localPosition = temp1; 
            // Debug.Log("no hit"); 
        }
    }

	public void transitionToState(bool toInventory){
		if(toInventory && !inInventory){
			// inTransition = true;
			animator.SetBool("inInventory", true);
            animator.applyRootMotion = false;
			inInventory = true;
		} else if (!toInventory && inInventory) {
			// inTransition = true;
			animator.SetBool("inInventory", false);
			inInventory = false;
		}
	}

    public void inwardDone(){
        inTransition = false;
        //TODO: enable inventory here
    }

    public void outwardsDone(){
        inTransition = false;
        animator.applyRootMotion = true;
    }

	private void distortImage(){
        float sanityPerc;
		if(sanity != 0)
        {
            if(sanity < 0)
            {
                sanityPerc = (sanity / -20);
            }
            else
            {
                sanityPerc = (sanity / 80);
            }
        }
        else
        {
            sanityPerc = 0;
        }
        //Setting Depth of Field
        DepthOfField depthOfField = ScriptableObject.CreateInstance <DepthOfField>();
        depthOfField.enabled.Override(true);
        depthOfField.focalLength.Override((PostProConstants.insaneFocusLength - PostProConstants.normalFocusLength) * sanityPerc + PostProConstants.normalFocusLength);
        depthOfField.focusDistance.Override(PostProConstants.focusDistance);
        depthOfField.aperture.Override(PostProConstants.aperature);
        if(PostProcess.profile.HasSettings<DepthOfField>())
            PostProcess.profile.RemoveSettings<DepthOfField>();
        PostProcess.profile.AddSettings(depthOfField);

        //Setting Color Grading
        ColorGrading colorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        colorGrading.enabled.Override(true);
        colorGrading.gradingMode.Override(PostProConstants.gradingMode);
        colorGrading.contrast.Override(PostProConstants.contrast);
        if(sanity > 0)
        {
            colorGrading.temperature.Override(PostProConstants.temperature * sanityPerc);
            colorGrading.saturation.Override(PostProConstants.saturation * sanityPerc);
        }
        else if( sanity < 0)
        {
            colorGrading.temperature.Override(PostProConstants.temperature * -sanityPerc);
            colorGrading.saturation.Override(PostProConstants.saturation * -sanityPerc);
        }
        else
        {
            colorGrading.temperature.Override(0);
            colorGrading.saturation.Override(0);
        }
        if (PostProcess.profile.HasSettings<ColorGrading>())
            PostProcess.profile.RemoveSettings<ColorGrading>();
        PostProcess.profile.AddSettings(colorGrading);

        //Setting Bloom 
        float maxBloomIntensity = (PostProConstants.bloom_IntensityInsane - PostProConstants.bloom_IntesityNormal) * sanityPerc + PostProConstants.bloom_IntesityNormal;
        float maxDirtIntensity = (PostProConstants.dirt_IntensityInsane - PostProConstants.dirt_IntesityNormal) * sanityPerc + PostProConstants.dirt_IntesityNormal;
        if(sanityPerc >= PulseThreshhold)
        {
            currentBloomIntensity = maxBloomIntensity * Mathf.Abs(Mathf.Sin(Time.time * (sanityPerc * 2)));
            currentDirtIntensity = maxDirtIntensity * Mathf.Abs(Mathf.Sin(Time.time * (sanityPerc * 2)));
        }
        else
        {
            currentBloomIntensity = maxBloomIntensity;
            currentDirtIntensity = maxDirtIntensity;
        }
        Bloom bloom = ScriptableObject.CreateInstance<Bloom>();
        bloom.enabled.Override(true);
        bloom.dirtTexture.Override(Insanity_Effect);
        bloom.threshold.Override(PostProConstants.bloom_Threshold);
        if(sanity != 0)
        {
            bloom.dirtIntensity.Override(currentDirtIntensity);
            if(sanity > 0)
            {
                bloom.intensity.Override(currentBloomIntensity);
            }
        }
        else
        {
            bloom.dirtIntensity.Override(PostProConstants.dirt_IntesityNormal);
            bloom.intensity.Override(PostProConstants.bloom_IntesityNormal);
        }
        if (PostProcess.profile.HasSettings<Bloom>())
            PostProcess.profile.RemoveSettings<Bloom>();
        PostProcess.profile.AddSettings(bloom);

        //Setting Vignette
        Vignette vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.mode.Override(PostProConstants.vignetteMode);
        vignette.opacity.Override(sanityPerc * 100);
        vignette.mask.Override(Insanity_Effect);
        if (PostProcess.profile.HasSettings<Vignette>())
            PostProcess.profile.RemoveSettings<Vignette>();
        PostProcess.profile.AddSettings(vignette);

        //Setting Grain
        Grain grain = ScriptableObject.CreateInstance<Grain>();
        grain.enabled.Override(true);
        grain.size.Override(PostProConstants.grain_size);
        grain.intensity.Override((PostProConstants.grain_IntensityInsane - PostProConstants.grain_IntensityNormal) * sanityPerc + PostProConstants.grain_IntensityNormal);
        float currentLuminance = PostProConstants.luminance_ContributionMax * Mathf.Abs(Mathf.Sin(Time.time * (sanityPerc * 2)));
        grain.lumContrib.Override(currentLuminance);
        if (PostProcess.profile.HasSettings<Grain>())
            PostProcess.profile.RemoveSettings<Grain>();
        PostProcess.profile.AddSettings(grain);

        //Lense Distortion
        LensDistortion distortion = ScriptableObject.CreateInstance<LensDistortion>();
        if (currentDistortion < targetDistortion)
        {
            currentDistortion = currentDistortion + distortionGrowth;
        }
        else
        { 
            targetDistortion = 0;
            if(currentDistortion > targetDistortion)
            {
                currentDistortion = currentDistortion - Time.deltaTime * 2;
            }
            if(currentDistortion <= 0)
            {
                currentDistortion = 0;
            }
        }
        distortion.enabled.Override(true);
        distortion.intensity.Override(-currentDistortion);
        if (PostProcess.profile.HasSettings<LensDistortion>())
            PostProcess.profile.RemoveSettings<LensDistortion>();
        PostProcess.profile.AddSettings(distortion);
    }

	public void moveSanityTo(float newSanity){
		targetSanity = Mathf.Clamp(newSanity, -20, 80);
	}

}
