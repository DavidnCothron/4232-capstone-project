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
	Vector3 attackDirection;
	Vector3 mousePos;
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

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}

	void checkInput()
	{
		#region Left Mouse Button Flow Control
		if (Input.GetMouseButton (0))
		{
			mousePos = GetWorldPositionOnPlane(Input.mousePosition, 0f);
			attackDirection = (mousePos - transform.position).normalized;
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
		RaycastHit2D[] hit = Physics2D.CircleCastAll 
			(playerController.trans.position, 1.0f, attackDirection, 0.5f, 0, 0f);
		//Debug
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

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position + (attackDirection * 1f), 0.25f);
	}
}
