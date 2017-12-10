using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeScript : MonoBehaviour {

	// Use this for initialization
	public GameObject particleSystem;
	public Transform playerPosition;
	public Animator eyeAnimator;
	public bool isActive = false;
	//public bool isDead = false;
	//public int health = 5;

	//public Animator eyeLidAnimator;

	void Awake () {
		particleSystem.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		eyeAnimator.SetBool("looking", isActive);
		if(isActive){
			transform.right = (Vector2)(playerPosition.position - transform.position);
		}
	}
}
