using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour {
	public Transform playerTransform;
	public int meleeDamage = 1;
	public int chargeAttackDamage = 4;
	public float knockbackBasic = 1f;
	public float knockbackCharged = 4f;
	public float chargeTime = 1.5f;
	public float chargeTimeRemaining;
	public float attackCooldown = 0.5f; 
	float attackCooldownRemaining;
	public List<GameObject> enemiesHit;
	public bool hasChargeAttack = false;
	public bool isAttackingEnabled = false;
	bool isAttacking = false;


	// Use this for initialization
	void Awaken () {
		enemiesHit = new List<GameObject> ();
		chargeTimeRemaining = chargeTime;
		attackCooldownRemaining = attackCooldown;
	}
	
	// Update is called once per frame
	void Update () {
		//Prevent the player from click spamming attacks. May replace with animation state value later.
		if (isAttacking)
		{
			attackCooldownRemaining -= Time.deltaTime;
			//Debug.Log (attackCooldownRemaining);
			if (attackCooldownRemaining < 0f)
			{
				isAttacking = false;
				attackCooldownRemaining = attackCooldown;
				//Debug.Log ("Cooldown Ended");
			}
		}
		else
		{ //Check for attack input
			if (Input.GetMouseButtonUp (0))
			{
				if (chargeTimeRemaining > 0)
				{
					MeleeAttack ();
				}
				else
				{
					ChargeAttack ();
				}
				isAttacking = true;
				//Debug.Log ("Cooldown Started");
			}
			else if (Input.GetMouseButton (0) && hasChargeAttack)
			{
				chargeTimeRemaining -= Time.deltaTime;
				if (chargeTimeRemaining < 0)
				{
					//Debug.Log ("Charged!");
				}
			}
			else
			{
				chargeTimeRemaining = chargeTime;
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

	void FixedUpdate(){
		transform.right = (GetWorldPositionOnPlane (Input.mousePosition, 0f) - playerTransform.position);
	}

	void MeleeAttack(){
		foreach (GameObject enemyObject in enemiesHit)
		{
			Enemy enemyScript = enemyObject.GetComponent (typeof(Enemy)) as Enemy;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemyScript.health -= meleeDamage;
			enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackBasic));
		}
	}

	void ChargeAttack(){
		foreach (GameObject enemyObject in enemiesHit)
		{
			Enemy enemyScript = enemyObject.GetComponent (typeof(Enemy)) as Enemy;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemyScript.health -= chargeAttackDamage;
			enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackCharged));
			chargeTimeRemaining = chargeTime;
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			enemiesHit.Add (coll.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			if (enemiesHit.Contains (coll.gameObject))
			{
				enemiesHit.Remove (coll.gameObject);
			}
		}
	}
}
