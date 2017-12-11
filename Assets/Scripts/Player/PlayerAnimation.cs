using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	[SerializeField]private PlayerPlatformerController ppc;
	[SerializeField]private Animator animator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetBool("dead", !PlayerManager.control.getAlive());
	}
}
