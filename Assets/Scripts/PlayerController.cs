using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Physics/Movement Variables
	Rigidbody2D rigidBody;
	//Collider2D playerHurtBox;
	public float rollTime;
	float remainingRolltime;
	public float rollSpeed;
	float verticalVelocity;
	float horizontalVelocity;
	public float horizontalSpeed = 10.0f;
	public float verticalSpeed = 10.0f;
	public float phaseSpeed = 5.0f;
	public float phaseDistance = 5.0f;
	float phaseTime = 0.5f;
	public float phaseMaxTime = 0.5f;
	float phaseHangTime = 0.25f;
	public float phaseMaxHangTime = 0.25f;
	Vector3 dir;
	Vector3 worldMousePos;
	public Transform trans;
	Vector3 playerToScreenSpace;
	float phaseMovedDistance;
	#endregion

	#region Flow Control Variables
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public bool isRolling = false;
	public bool isJumping = false;
	public bool isPhasing = false;
	public bool isPhaseHanging = false;
	#endregion

	#region Ability Variables
	public int numberOfJumpsMax = 1; //0 is one extra jump, 1 is 2 extra jumps, etc,.
	int numberOfJumps;
	public bool hasPhase = true;
	#endregion

	// Use this for initialization
	void Awake () {
		//Control movement through the rigidBody object *only*
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		//playerHurtBox = gameObject.GetComponent (typeof(Collider2D)) as Collider2D;
		numberOfJumps = numberOfJumpsMax;
		remainingRolltime = rollTime;
		phaseTime = phaseMaxTime;
		phaseHangTime = phaseMaxHangTime;
	}
	
	// check for input and assign variables here
	void Update () {
		checkMoveInput ();
		updateMoveModifiers ();
	}

	//Do movement and actions here
	void FixedUpdate()
	{
		//Debug.Log (isBusy + " " + isRolling + " " + remainingRolltime);

		if (!isBusy && !isPhasing)
		{
			Move ();
		}
		else
			if (isRolling && !isPhasing)
		{
			Roll ();
		}
		else
		if (isPhasing)
		{
			if (!isPhaseHanging)
			{
				Phase ();
			}
			else
			{
				PhaseHang ();
			}
		}
	}

	void updateMoveModifiers(){
		phaseMovedDistance = (playerToScreenSpace.x - Camera.main.WorldToScreenPoint (trans.position).x) + (playerToScreenSpace.y - Camera.main.WorldToScreenPoint (trans.position).y);
		playerToScreenSpace = Camera.main.WorldToScreenPoint(transform.position);

		if (isGrounded) {
			numberOfJumps = numberOfJumpsMax;
		}
	}

	//Adjust movement modifiers here based on input
	void checkMoveInput()
	{
		horizontalVelocity = Input.GetAxis ("Horizontal") * horizontalSpeed;
		verticalVelocity = 1 * verticalSpeed;

		if (Input.GetKeyDown (KeyCode.LeftShift) && !isRolling) {
			isBusy = true;
			isRolling = true;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			isJumping = true;
		}

		if (Input.GetMouseButtonDown (1) && hasPhase)
		{
			isBusy = true;
			isPhasing = true;
		}
	}

	void Move()
	{
		rigidBody.velocity = new Vector2 (horizontalVelocity, rigidBody.velocity.y);
		if (isJumping)
			Jump ();
	}

	void Jump()
	{
		if (!isGrounded) {
			numberOfJumps--;
		}

		if (numberOfJumps > 0) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, verticalVelocity);
		}

		isJumping = false;
	}

	void Roll()
	{
		remainingRolltime -= Time.deltaTime;

		rigidBody.velocity = new Vector2 (horizontalVelocity * rollSpeed, rigidBody.velocity.y);

		if (remainingRolltime <= 0) {
			isRolling = false;
			isBusy = false;
			remainingRolltime = rollTime;
		}
	}

	void Phase(){
		if (phaseMovedDistance > phaseDistance)
		{
			isBusy = false;
			phaseTime = phaseMaxTime;
			isPhaseHanging = true;

			return;
		}

		if (phaseTime <= 0){
			isBusy = false;
			phaseTime = phaseMaxTime;
			isPhaseHanging = true;
		} else {
			phaseTime -= Time.deltaTime;
			dir = (Input.mousePosition - playerToScreenSpace).normalized;
			rigidBody.velocity = (Vector2)(dir * phaseSpeed);
		}
	}

	void PhaseHang(){
		Debug.Log ("Hanging!");
		rigidBody.velocity = Vector3.zero;
		phaseHangTime -= Time.deltaTime;

		if (phaseHangTime <= 0){
			phaseHangTime = phaseMaxHangTime;
			isPhasing = false;
			isPhaseHanging = false;
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground") {
			isGrounded = true;
		} else if (coll.gameObject.tag == "damage" && !isRolling) {
			//Do damage
		}
		//Debug.Log (coll.gameObject.tag + " enter");
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground")
			isGrounded = false;
		//Debug.Log (coll.gameObject.tag + " exit");
	}
}
