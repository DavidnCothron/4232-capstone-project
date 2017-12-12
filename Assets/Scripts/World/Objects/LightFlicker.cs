using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
	private Light thisLight;
	private Color startColor;
	private float timeGone, intensity, randomMult, randomAdd, randomFrequency, range;
	private float changeValue;
	// Use this for initialization
	void OnEnable () {
		randomize();
		thisLight = GetComponent<Light>();
		if (thisLight != null) {
			startColor = thisLight.color;
			intensity = thisLight.intensity;
			range = thisLight.range;
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeGone = Time.time/2;
		timeGone = timeGone - Mathf.Floor(timeGone);
		thisLight.color = startColor * changeCalc();
		thisLight.intensity = intensity * changeCalc();
		thisLight.range = range * changeCalc();
	}

	float changeCalc(){
		changeValue = -Mathf.Sin(timeGone * randomFrequency * Mathf.PI) * randomMult + randomAdd;
		return changeValue;
	}

	void randomize(){
		randomAdd = Random.Range(55,95)/100f;
		randomMult = Random.Range(5,10)/100f;
		randomFrequency = Random.Range(5, 12);
	}
}
