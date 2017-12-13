using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour {
	public Transform playerTransform;
	public int meleeDamage = 1;
	public int chargeAttackDamage = 4;
	public float attackHitDelay = .4f;
	private float knockbackBasic = 5f;
	private float knockbackCharged = 10f;
	public float chargeTime = 1.5f;
	public float chargeTimeRemaining;
	public float attackCooldown = 0.5f; 
	float attackCooldownRemaining;
	public List<GameObject> enemiesHit;
	public List<GameObject> bossHit;
	public List<GameObject> bossControllerHit;
	public bool hasChargeAttack = false;
	public bool isAttackingEnabled = false;
	bool isAttacking = false;
	bool canAttack = true;
	private Vector3 currentAttackDirection;
	private string attackDirection = "none";
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

	[SerializeField]private PlayerPlatformerController ppc;

	[SerializeField]private PlayerAnimation playerAnimation;

	private IEnumerator airAttackCo, groundAttackCo;

	private int wallsHit;
	private bool isChargeAttacking = false;


	// Use this for initialization
	void Awaken () {
		enemiesHit = new List<GameObject> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		chargeTimeRemaining = chargeTime;
		attackCooldownRemaining = attackCooldown;
		contactFilter.useTriggers = true;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		wallsHit = 0;
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
						attacking = true;
						StartCoroutine(MeleeAttack());
					}
					else
					{
						isChargeAttacking = true;
						attacking = true;
						StartCoroutine(ChargeAttack ());
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

	float getKnockbackBasic(){
		return Random.Range(knockbackBasic -2, knockbackBasic +2);
	}

	float getKnockbackCharged(){
		return Random.Range(knockbackCharged -5, knockbackCharged +5);
	}

	void LateUpdate() {
		if (!isAttacking && !ppc.haltInput && !attacking){
			if(Input.GetAxis("Horizontal") > 0) {
				currentAttackDirection = new Vector3(1f, 0f, 0f);
				attackDirection = "right";
			}
			else if(Input.GetAxis("Horizontal") < 0) {
				currentAttackDirection = new Vector3(-1f, 0f, 0f);
				attackDirection = "left";
			}
		
			if(Input.GetKey(KeyCode.W)){
				transform.right = new Vector3(0f, 1f, 0f);
				attackDirection = "up";
			}
			else
				transform.right = currentAttackDirection;
		}
	}

	public string GetAttackDirection(){
		return attackDirection;
	}
	//this method lol
	public int GetCurrentAttackPower(){
		if(isChargeAttacking){
			return chargeAttackDamage;
		}
		else{
			return meleeDamage;
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
		
	}

	public bool getAttacking() {
		return isAttacking;
	}
	
	IEnumerator MeleeAttack(){
		attacking = true;
		if(ppc.getGrounded()) {
			if (groundAttackCo != null) StopCoroutine(groundAttackCo);
			groundAttackCo = playerAnimation.groundAttackAnimation();
			StartCoroutine(groundAttackCo);
		} else {
			if(airAttackCo != null) StopCoroutine(airAttackCo);
			airAttackCo = playerAnimation.airAttackAnimation();
			StartCoroutine(airAttackCo);
		}
		
		yield return new WaitForSeconds(attackHitDelay);
		foreach (GameObject enemyObject in enemiesHit)
		{
			EnemyHealth enemy_Health = enemyObject.GetComponent (typeof(EnemyHealth)) as EnemyHealth;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemyController enemyCont = enemyObject.GetComponent (typeof(enemyController)) as enemyController;
			enemy_Health.LoseHealth(meleeDamage);
			var knockback = getKnockbackBasic();
			StartCoroutine(enemyCont.setKnockbackVec((new Vector2 (knockback, knockback/2))));
			//enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackBasic));
		}

		foreach (GameObject bossObject in bossHit)
		{
			EvilEyeScript enemy_Health = bossObject.GetComponent(typeof(EvilEyeScript)) as EvilEyeScript;
			if(enemy_Health != null) enemy_Health.HitEye(this); 
			//Debug.Log(enemy_Health);
		}

		foreach (GameObject bossObject in bossControllerHit)
		{
			MouthOfEvilController boss = bossObject.GetComponent(typeof(MouthOfEvilController)) as MouthOfEvilController;
			if(boss.isInPhase){
				boss.hitsLeftInPhase--;
			}
		}
		attacking = false;
	}

	IEnumerator ChargeAttack(){
		GameControl.control.phmc.LoseMana(2);
		attacking = true;
		if(ppc.getGrounded()) {
			if (groundAttackCo != null) StopCoroutine(groundAttackCo);
			groundAttackCo = playerAnimation.groundAttackAnimation();
			StartCoroutine(groundAttackCo);
		} else {
			if(airAttackCo != null) StopCoroutine(airAttackCo);
			airAttackCo = playerAnimation.airAttackAnimation();
			StartCoroutine(airAttackCo);
		}
		
		yield return new WaitForSeconds(attackHitDelay);
		foreach (GameObject enemyObject in enemiesHit)
		{
			EnemyHealth enemy_Health = enemyObject.GetComponent (typeof(EnemyHealth)) as EnemyHealth;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;			
			enemyController enemyCont = enemyObject.GetComponent (typeof(enemyController)) as enemyController;
			enemy_Health.LoseHealth(chargeAttackDamage);
			var knockback = getKnockbackCharged();
			StartCoroutine(enemyCont.setKnockbackVec(new Vector2 (knockback, knockback/2)));
			//enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackCharged));
		}

		foreach (GameObject bossObject in bossHit)
		{
			EvilEyeScript enemy_Health = bossObject.GetComponent(typeof(EvilEyeScript)) as EvilEyeScript;
			enemy_Health.HitEye(this);
		}

		foreach (GameObject bossObject in bossControllerHit)
		{
			MouthOfEvilController boss = bossObject.GetComponent(typeof(MouthOfEvilController)) as MouthOfEvilController;
			if(boss.isInPhase){
				boss.hitsLeftInPhase -= 2;
			}
		}

		chargeTimeRemaining = chargeTime;
		//yield return new WaitForSeconds(.5f);
		attacking = false;
		//animator.SetBool("groundAttack", attacking);
		//animator.SetBool("airAttack", attacking);
		particleSystem.SetActive(false);
		isChargeAttacking = false;
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){		
			enemiesHit.Add (coll.gameObject.GetComponentInParent<Rigidbody2D>().gameObject);
		} else if (coll.gameObject.CompareTag("ground")) {
			wallsHit++;
		} else if(coll.gameObject.tag == "mouthOfEvil"){
			bossHit.Add(coll.gameObject);
		} else if(coll.gameObject.tag == "mouthOfEvilController"){
			bossControllerHit.Add(coll.gameObject);
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			GameObject enemy = coll.gameObject.GetComponentInParent<Rigidbody2D>().gameObject;
			if (enemiesHit.Contains (enemy))
			{
				enemiesHit.Remove (enemy);
			}
		} else if (coll.gameObject.CompareTag("ground")){
			wallsHit--;
		} else if(coll.gameObject.tag == "mouthOfEvil"){
			GameObject enemy = coll.gameObject;
			if (bossHit.Contains (enemy))
			{
				bossHit.Remove(coll.gameObject);
			}
		} else if(coll.gameObject.tag == "mouthOfEvilController"){
			GameObject enemy = coll.gameObject;
			if (bossControllerHit.Contains (enemy))
			{
				bossControllerHit.Remove(coll.gameObject);
			}
		}
	}

	public int getWallsHit() {
		return wallsHit;
	}
}
