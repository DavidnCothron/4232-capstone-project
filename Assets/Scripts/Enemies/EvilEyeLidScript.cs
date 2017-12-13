using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeLidScript : MonoBehaviour {

	public Animator eyeLidAnimator;
	public bool isDead = true;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		eyeLidAnimator.SetBool("isDead", isDead);
	}
}
