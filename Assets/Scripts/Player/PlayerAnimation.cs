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
		//Handle animation state logic here
			animator.SetBool ("grounded", ppc.getGrounded());
			animator.SetFloat ("velocityX", Mathf.Abs (ppc.getVelocity().x) / ppc.maxSpeed);
			animator.SetFloat ("velocityY", ppc.getVelocity().y / ppc.maxSpeed);

			if (ppc.getVelocity().x!= 0 && ppc.getGrounded())
			{
				animator.SetBool ("startRun", true);
				if (Mathf.Abs(ppc.getVelocity().x) > 0.01f)
					animator.SetBool ("isRunning", true);
			} 
			else 
			{
				animator.SetBool ("startRun", false);
				animator.SetBool ("isRunning", false);
			}
			animator.SetBool("dead", !PlayerManager.control.getAlive());
	}
}
