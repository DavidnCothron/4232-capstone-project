using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour {

	#region Flow Control Variables
	public float maxChargeTime = 2.0f;
	public float attackDuration = 0.1f;
	float attackDurationRemaining;
	float chargeTime;
	bool hasHolySword = false;
	//public bool isHitboxActive = false;
	bool isAttacking = false;
	bool isChargeAttacking = false;
	#endregion

	#region Ability Damage
	public float basicAttackDamage;
	public float rangedAttackDamage;
	public float chargedAttackDamage;
	#endregion

	#region direction vector
	Vector2 attackDirection;
	#endregion

	#region Other Controllers
	public PlayerController playerController;
	#endregion

	#region Components
	//public Collider2D attackTrigger;
	#endregion

	// Use this for initialization
	void Awake () {
		chargeTime = maxChargeTime;
		attackDurationRemaining = attackDuration;

	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	void FixedUpdate(){
		if (isAttacking) {
			if (attackDurationRemaining == attackDuration) {
				BasicAttack ();
			} else {
				attackDurationRemaining -= Time.deltaTime;
			}
		}
	}

	void checkInput()
	{
		#region Left Mouse Button Flow Control
		if (Input.GetMouseButton (0))
		{
			Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
			attackDirection = (Vector2)(Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;

			chargeTime -= Time.deltaTime;

			if (chargeTime > 0)
			{
				isAttacking = true;
			}
			else if (chargeTime <= 0 && hasHolySword)
			{
				isChargeAttacking = true;
			}
		}
		else
		{
			chargeTime = maxChargeTime;
		}
		#endregion

		if (attackDurationRemaining <= 0)
		{
			isAttacking = false;
			attackDurationRemaining = attackDuration;
		}
	}

	void BasicAttack(){
		//Do attack stuff
		Debug.Log("Attack!");
		RaycastHit2D[] hit = Physics2D.CircleCastAll 
			(playerController.trans.position, 1.0f, attackDirection * 5f, 0.5f, 0, 0f);
		Debug.DrawLine (transform.position, attackDirection * 1.5f, Color.red, 0.5f, depthTest: true);
		for (int i = 0; i < hit.Length; i++) {
			if (hit [i].transform != playerController.trans && hit [i].collider.tag == "enemy") {
				//hit [i].collider.GetComponent ();
				//Call method and send in damage
				Debug.Log(basicAttackDamage + " Damage!");
			}
		}
		attackDurationRemaining -= Time.deltaTime;
	}

	void ChargeAttack(){
		//isHitboxActive = false;
	}

	void RollAttack(){
		//isHitboxActive = false;
	}
}
