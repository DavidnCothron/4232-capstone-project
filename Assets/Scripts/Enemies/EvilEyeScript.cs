using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeScript : MonoBehaviour {

	// Use this for initialization
	public GameObject particleSystem;
	public Transform playerPosition;
	public Animator eyeAnimator;
	public bool isActive = false;
	public float attackCooldown;
	public float attackCooldownMax = 2f;
	public float attackChargeUp;
	public float attackChargeUpMax = 1f;
	//public bool isDead = false;
	//public int health = 5;

	//public Animator eyeLidAnimator;

	void Awake () {
		particleSystem.SetActive(false);
		attackChargeUp = attackChargeUpMax;
		attackCooldown = attackCooldownMax;
	}
	
	// Update is called once per frame
	void Update () {
		eyeAnimator.SetBool("looking", isActive);
		if(isActive){
			transform.right = (Vector2)(playerPosition.position - transform.position);
			attackCooldown -= Time.deltaTime;
			if(attackCooldown <= 0f){
				if(!particleSystem.active)
					particleSystem.SetActive(true);
				attackChargeUp -= Time.deltaTime;
				if(attackChargeUp <= 0){
					attackCooldown = attackCooldownMax;
					attackChargeUp = attackChargeUpMax;
					FireEyeBall();
					particleSystem.SetActive(false);
				}
			}
		}
	}

	void FireEyeBall(){
		Debug.Log("Die, player!");
		return;
	}
}
