using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeScript : MonoBehaviour {

	// Use this for initialization
	public GameObject particleSystem;
	public SpriteRenderer spriteFlash;
	public Transform playerPosition;
	public Animator eyeAnimator;
	public EvilEyeLidScript eyeLid;
	public bool isActive;
	public float attackCooldown;
	public float attackCooldownMax = 2f;
	public float attackChargeUp;
	public float attackChargeUpMax = 1f;
	public int eyeHealth;
	//public bool isDead = false;
	//public int health = 5;

	//public Animator eyeLidAnimator;

	void Awake () {
		particleSystem.SetActive(false);
		attackChargeUp = attackChargeUpMax;
		attackCooldown = attackCooldownMax;
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		eyeAnimator.SetBool("looking", isActive);
		if(isActive){
			if(eyeHealth <= 0){
				isActive = false;
				eyeLid.isDead = true;
				return;
			}
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

	public bool GetIfDead(){
		return eyeLid.isDead;
	}

	void FireEyeBall(){
		//Debug.Log("Die, player!");
		return;
	}

	public void HitEye(PlayerMeleeScript meleeScript) {
		//Debug.Log("I've Been Hit!");
		int damage = meleeScript.GetCurrentAttackPower();
		eyeHealth -= damage;
		//FlashSprite(12, 0.1f);
	}

	IEnumerator FlashSprite(int numTimes, float delay) {
        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++) {            
            
			spriteFlash.enabled = true;
            // delay specified amount
            yield return new WaitForSeconds(delay);
 
            spriteFlash.enabled = false;
            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }
}
