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
	private bool inRange = false;
	[SerializeField] protected ContactFilter2D contactFilter;

	public PlayerHealthAndMagicController phmc;

	void Awake()
	{
		contactFilter.useTriggers = true;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;	
	}

	void Start()
	{
		phmc = GameControl.control.GetPlayerTransform().gameObject.GetComponent<PlayerHealthAndMagicController>();
	}
	
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
		if(inRange){
			//Debug.Log("trying attack");
			phmc.LoseHealth(attackDamage);
			return true; 
		}
		//Debug.Log("false");
		return false;
	}
	void OnTriggerEnter2D(Collider2D coll){	
		if(coll.gameObject.tag == "melee"){		
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "melee"){
			inRange = false;
		}
	}
}
