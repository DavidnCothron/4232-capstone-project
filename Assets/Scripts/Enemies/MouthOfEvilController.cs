using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthOfEvilController : MonoBehaviour {

	public List<EvilEyeScript> evilEyes;
	public int mouthOfEvilHealth = 12;
	private int hitsLeftInPhase;
	public int maxHitsPerPhase = 4;
	public bool isInPhase = false;
	public int totalPhases;
	public int currentPhase;
	public bool isBossFightActive = false;

	void Awake () {
		foreach(EvilEyeScript eye in evilEyes){
			eye.isActive = false;
		}
		hitsLeftInPhase = 4;
		isInPhase = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isBossFightActive){
			if(hitsLeftInPhase <= 0){
				IteratePhase();
			}
		}
	}

	private void IteratePhase(){
		currentPhase++;
		hitsLeftInPhase = 4;
		return;
	}

	//stolen from: https://forum.unity.com/threads/make-a-sprite-flash.224086/
	IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float delay, bool disable = false) {
        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++) {            
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {                
                if (disable) {
                    // for disabling
                    sprites[i].enabled = false;
                } else {
                    // for changing the alpha
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 0.5f);
                }
            }
 
            // delay specified amount
            yield return new WaitForSeconds(delay);
 
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++) {
                if (disable) {
                    // for disabling
                    sprites[i].enabled = true;
                } else {
                    // for changing the alpha
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 1);
                }
            }
 
            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }
}
