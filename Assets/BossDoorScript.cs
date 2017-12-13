using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorScript : MonoBehaviour {

	//public BossKeyScript bossKey;
	public bool isUnlocked = false;
	public Animator doorAnimator;
	void Awake () {
		isUnlocked = false;
		doorAnimator.SetBool("isOpening", false);
		doorAnimator.SetBool("isClosing", true);
	}
	
	// Update is called once per frame
	void Update () {
		if(isUnlocked){
			doorAnimator.SetBool("isOpening", true);
			doorAnimator.SetBool("isClosing", false);
		}
		else{
			doorAnimator.SetBool("isOpening", false);
			doorAnimator.SetBool("isClosing", true);
		}
	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player"){
			if(other.GetComponent<BossKeyScript>().isInPlayerPossession){
				isUnlocked = true;
			}
		}
	}
}
