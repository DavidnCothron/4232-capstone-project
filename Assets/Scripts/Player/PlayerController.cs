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
	float phaseHangTime = 0.25f;
	public float phaseMaxHangTime = 0.25f;
	Vector2 dir;
	public Transform trans;
	Vector2 targetVector;
	#endregion

	#region Flow Control Variables
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public bool isRolling = false;
	public bool isJumping = false;
	public bool isPhasing = false;
	public bool isPhaseHanging = false;
	public bool lockCameraX = false;
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

		if (Input.GetMouseButtonDown (1) && hasPhase && !isPhasing)
		{
			isBusy = true;
			isPhasing = true;
			Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
			dir = (Vector2)(Camera.main.ScreenToWorldPoint(mousePos) - trans.position).normalized;
			targetVector = (Vector2)trans.position + (phaseDistance * dir);
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
		rigidBody.gravityScale = 0f;
		rigidBody.velocity = (dir * phaseSpeed);

		if (Vector2.Distance ((Vector2)trans.position, (Vector2)targetVector) <= 0.5f) {
			isBusy = false;
			isPhaseHanging = true;
			rigidBody.gravityScale = 1f;

			return;
		}
	}

	void PhaseHang(){
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
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "CameraLockX"){
			lockCameraX = true;
		}
	}

	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "CameraLockX"){
			lockCameraX = false;
		}
	}
}
