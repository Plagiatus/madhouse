using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	private bool inInventory = false;
	private bool inTransition = false;
	[Range(-20,80)]
	private float sanity;
	private float targetSanity;
    private PostProcessVolume PostProcess;
	private Vector3 temp1;


    void Start () {
		offset = this.transform.position - player.transform.position;
        PostProcess = this.GetComponent<PostProcessVolume>();
        offset = this.transform.position - player.transform.position;
        temp1 = this.transform.localPosition;
    }

	void Update() {
        if (Input.GetKeyDown(KeyCode.S))
        {
            targetSanity = -20;
        }
            
        if (Input.GetKeyDown(KeyCode.W))
        {
            targetSanity = 80;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetSanity = 0;
        }
        sanity = Mathf.Lerp(sanity, targetSanity, Time.deltaTime);
        Debug.Log("Insanity at " + sanity);
        Debug.Log("Target Sanity at " + targetSanity);
        distortImage();
	}

	private void moveCameraBehindPLayer(){
			RaycastHit hit;
			Vector3 hitPoint;
			
			Ray ray = new Ray(player.transform.position + Vector3.up * 0.5f, player.transform.forward * -1);
			Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);
			// Debug.Log(Physics.Raycast(ray, out hit, 2.5f));
			if (Physics.Raycast(ray, out hit, 2.5f)){
				hitPoint = hit.point;
				// Debug.Log("hit");
				// Debug.Log(hitPoint);
				// worldposition auf lokalposition setzen
				player.transform.localPosition = this.transform.InverseTransformPoint(hitPoint);
				Debug.Log(player.transform.localPosition);
			}
			// camera auf standard setzen
			else { 
				player.transform.localPosition = temp1; 
				// Debug.Log("no hit"); 
			}
		}

	public void transitionToState(bool toInventory){
		if(toInventory && !inInventory && !inTransition){
			inTransition = true;
			//TODO: move to Inventory view, maybe a Coroutine or Animation.
			inInventory = true;
		} else if (!toInventory && inInventory && !inTransition) {
			inTransition = true;
			//TODO: move back to normal view, maybe via Coroutine or Animation
			inInventory = false;
		}
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


    }

	public void moveSanityTo(float newSanity){
		targetSanity = Mathf.Clamp(newSanity, -20, 80);
	}

}
