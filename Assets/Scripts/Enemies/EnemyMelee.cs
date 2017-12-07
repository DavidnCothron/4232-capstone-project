using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour {

	public int attackDamage = 1;
	public float attackCooldown = 0.5f; 
	float attackCooldownRemaining;
	bool isAttacking = false;
	bool canAttack = true;
	private bool attacking;

	private PlayerHealthAndMagicController phmc;
	
	// Update is called once per frame
	void Update () {
		// if(canAttack){
		// 	if (isAttacking)
		// 	{
		// 		attackCooldownRemaining -= Time.deltaTime;
		// 		//Debug.Log (attackCooldownRemaining);
		// 		if (attackCooldownRemaining < 0f)
		// 		{
		// 			isAttacking = false;
		// 			attackCooldownRemaining = attackCooldown;
					
		// 			//Debug.Log ("Cooldown Ended");
		// 		}
		// 	}
		// 	else
		// 	{ //Check for attack input
		// 		attacking = false;

		// 		if (attackCooldownRemaining > 0)
		// 		{
		// 			MeleeAttack ();
		// 		}
		// 		else
		// 		{
		// 			ChargeAttack ();
		// 		}
		// 		isAttacking = true;
								
		// 	}
		// }
	}

	public bool Hit(){
		if(phmc != null){
			phmc.LoseHealth(attackDamage);
			return true; 
		}
		return false;
	}
	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "Player"){		
			phmc = coll.gameObject.GetComponent<PlayerHealthAndMagicController>();
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "Player"){
			phmc = null;
		}
	}
}
