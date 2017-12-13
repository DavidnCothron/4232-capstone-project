using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthOfEvilController : MonoBehaviour {

	public List<EvilEyeScript> evilEyes;
	public int mouthOfEvilHealth = 12;
	public int hitsLeftInPhase;
	public int maxHitsPerPhase = 4;
	public bool isInPhase = false;
	public int totalPhases;
	public int currentPhase;
	public bool isBossFightActive = false;
	public bool startBossFight = false;
	private bool allEyesDead = false;
	public bool bossIsDead = false;

	void Awake () {
		foreach(EvilEyeScript eye in evilEyes){
			eye.isActive = false;
			eye.eyeLid.isDead = true;
		}
		hitsLeftInPhase = 4;
		isInPhase = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(isBossFightActive + " " + startBossFight);
		if(isBossFightActive){
			if(startBossFight) StartBossFight();

			if(!isInPhase){
				foreach(EvilEyeScript eye in evilEyes){
					if(eye.GetIfDead() != true){
						allEyesDead = false;
					}
					else {
						allEyesDead = true;
					}
				}

				if(allEyesDead){
					isInPhase = true;
				}
			}else if(hitsLeftInPhase <= 0){
				
				if(mouthOfEvilHealth > 0){
					IteratePhase();
					Debug.Log("Iterating Phase");
				}
				else{
					foreach(EvilEyeScript eye in evilEyes){
						eye.isActive = false;
						eye.eyeLid.isDead = true;
					}
					bossIsDead = true;
				}
			}
		}
	}

	private void StartBossFight(){
		startBossFight = false;
		foreach(EvilEyeScript eye in evilEyes){
			eye.isActive = true;
			eye.eyeLid.isDead = false;
		}
	}

	private void IteratePhase(){
		currentPhase++;
		hitsLeftInPhase = 4;
		foreach(EvilEyeScript eye in evilEyes){
			eye.isActive = true;
			eye.eyeLid.isDead = false;
			eye.eyeHealth = 4;
		}
		mouthOfEvilHealth -= 4;
		isInPhase = false;
		return;
	}

	// private void OnTriggerEnter2D(Collider2D other) {
	// 	if(isInPhase){
	// 		if(other.tag == "melee"){
	// 			//var meleeScript = other.GetComponent<PlayerMeleeScript>();
	// 			hitsLeftInPhase--;
	// 		}
	// 	  }	
	// }
}
