using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthOfEvilController : MonoBehaviour {

	public List<EvilEyeScript> evilEyes;
	public int mouthOfEvilHealth = 12;
	private int hitsLeftInPhase;
	public int maxHitsPerPhase = 4;
	public bool isInPhase = false;

	void Awake () {
		foreach(EvilEyeScript eye in evilEyes){
			eye.isActive = false;
		}
		hitsLeftInPhase = 4;
		isInPhase = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
