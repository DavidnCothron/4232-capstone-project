using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	[SerializeField]private PlayerPlatformerController ppc;
	[SerializeField]private Animator animator;

	public float airAttackAnimTime, groundAttackAnimTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetBool("dead", !PlayerManager.control.getAlive());
	}

	public IEnumerator airAttackAnimation() {
		animator.SetBool("groundAttack", false);
		animator.SetBool("airAttack", true);
		yield return new WaitForSeconds(airAttackAnimTime);
		animator.SetBool("airAttack", false);
		yield return null;
	}

	public IEnumerator groundAttackAnimation() {
		animator.SetBool("groundAttack", true);
		animator.SetBool("airAttack", false);
		yield return new WaitForSeconds(groundAttackAnimTime);
		animator.SetBool("groundAttack", false);
		yield return null;
	}
}
