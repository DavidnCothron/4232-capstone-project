﻿using System.Collections;
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
	bool canAttack = true;
	private Vector3 currentAttackDirection;
	[SerializeField] protected ContactFilter2D contactFilter;
	[SerializeField] protected Rigidbody2D rb2d;
	[SerializeField] protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	[SerializeField] protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	protected const float minMoveDistance = 0.001f;
	protected const float shellRadius = 0.01f;

	private bool attacking;
	[SerializeField]private SpriteRenderer spriteRenderer;
	[SerializeField]private Animator animator;
	public GameObject particleSystem;


	// Use this for initialization
	void Awaken () {
		enemiesHit = new List<GameObject> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		chargeTimeRemaining = chargeTime;
		attackCooldownRemaining = attackCooldown;
		contactFilter.useTriggers = true;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}
	
	void Start(){
		particleSystem.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		//Prevent the player from click spamming attacks. May replace with animation state value later.
		//Debug.Log(particleSystem.isPlaying + "\r\n" + particleSystem.isEmitting);
		if(canAttack){
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
				attacking = false;
				animator.SetBool("groundAttack", attacking);

				if (Input.GetMouseButtonUp (0) && !Input.GetKey(KeyCode.LeftShift))
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
				}
				else if (Input.GetMouseButton (0) && hasChargeAttack && !Input.GetKey(KeyCode.LeftShift))
				{
					chargeTimeRemaining -= Time.deltaTime;
					if (chargeTimeRemaining < 0)
					{
					    particleSystem.SetActive(true);
					}
				}
				else
				{
					chargeTimeRemaining = chargeTime;
				}
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
		// Vector3 pos = GetWorldPositionOnPlane (Input.mousePosition, 0f) - playerTransform.position;
		// float angle = Vector3.Angle(transform.right, pos);
		// angle = angle < 180 ? angle:angle-360;
		// Debug.Log(angle);
		// transform.RotateAround(playerTransform.position, transform.forward, angle * Time.deltaTime);
		//transform.right = (GetWorldPositionOnPlane (Input.mousePosition, 0f) - gameObject.transform.position);

		if(Input.GetAxis("Horizontal") > 0) {
			currentAttackDirection = new Vector3(1f, 0f, 0f);
		}
		else if(Input.GetAxis("Horizontal") < 0) {
			currentAttackDirection = new Vector3(-1f, 0f, 0f);
		}
		
		if(Input.GetKey(KeyCode.W))
			transform.right = new Vector3(0f, 1f, 0f);
		else
			transform.right = currentAttackDirection;
	}

	
	void MeleeAttack(){

//		int count = rb2d.Cast (Vector2.zero, contactFilter, hitBuffer, shellRadius);
//		hitBufferList.Clear ();
//
//		for (int i = 0; i < count; i++)
//		{
//			hitBufferList.Add (hitBuffer [i]);
//		}
		attacking = true;
		animator.SetBool("groundAttack", attacking);

		foreach (GameObject enemyObject in enemiesHit)
		{
			EnemyHealth enemy_Health = enemyObject.GetComponent (typeof(EnemyHealth)) as EnemyHealth;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemy_Health.LoseHealth(meleeDamage);
			enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackBasic));
		}
	}

	void ChargeAttack(){
		GameControl.control.phmc.LoseMana(2);
		attacking = true;
		animator.SetBool("groundAttack", attacking);
		foreach (GameObject enemyObject in enemiesHit)
		{
			EnemyHealth enemy_Health = enemyObject.GetComponent (typeof(EnemyHealth)) as EnemyHealth;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemy_Health.LoseHealth(chargeAttackDamage);
			enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackCharged));
			chargeTimeRemaining = chargeTime;
		}

		particleSystem.SetActive(false);
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){		
			enemiesHit.Add (coll.gameObject.GetComponentInParent<Rigidbody2D>().gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			GameObject enemy = coll.gameObject.GetComponentInParent<Rigidbody2D>().gameObject;
			if (enemiesHit.Contains (enemy))
			{
				enemiesHit.Remove (enemy);
			}
		}
	}
}
