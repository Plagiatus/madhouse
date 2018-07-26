using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugGUIScript : MonoBehaviour {

	public Player player; 
	public Slider stabilitySlider;
	public Slider sanitySlider;
	public Text values;

	void Start () {
	}
	
	void Update () {
		stabilitySlider.value = player.getStability();
		sanitySlider.value = player.getSanity();
		values.text = sanitySlider.value + "\n" + player.getHungerAndSleep().x + " - " + player.getHungerAndSleep().y + "\n" + stabilitySlider.value;
	}

	public void OnStabilityChanged(){
		player.setStability(stabilitySlider.value);
	}
	public void OnSanityChanged(){
		player.setSanity(sanitySlider.value);
	}
}
