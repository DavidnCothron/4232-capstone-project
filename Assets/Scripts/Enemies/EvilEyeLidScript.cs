using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeLidScript : MonoBehaviour {

	public Animator eyeLidAnimator;
	public bool isDead = false;
	public int health = 5;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		eyeLidAnimator.SetBool("isDead", isDead);

		if(health <= 0){
			isDead = true;
		}
	}
}
