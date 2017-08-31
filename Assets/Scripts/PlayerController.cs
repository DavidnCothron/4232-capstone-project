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
	Vector2 direction;
	Vector3 worldMousePos;
	#endregion

	#region Flow Control Variables
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public bool isRolling = false;
	public bool isJumping = false;
	public bool isPhasing = false;
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
	}
	
	// check for input and assign variables here
	void Update () {
		checkMoveInput ();
	}

	//Do movement and actions here
	void FixedUpdate()
	{
		//Debug.Log (isBusy + " " + isRolling + " " + remainingRolltime);

		if (!isBusy)
			Move ();
		else if (isRolling)
			Roll ();
		else if (isPhasing)
			Phase ();
	}

	//Adjust movement modifiers here based on input
	void checkMoveInput()
	{
		horizontalVelocity = Input.GetAxis ("Horizontal") * horizontalSpeed;
		verticalVelocity = 1 * verticalSpeed;

		if (isGrounded) {
			numberOfJumps = numberOfJumpsMax;
		}

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

		worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
			//Debug.Log (numberOfJumps + " " + isGrounded);
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
		phaseTime -= Time.deltaTime;

		if (phaseTime <= 0) {
			//Debug.Log (direction);
			isBusy = false;
			isPhasing = false;
			phaseTime = phaseMaxTime;
		} else {
			direction = (Vector2)((worldMousePos - Camera.main.WorldToScreenPoint(transform.position)));
			direction.Normalize();

			rigidBody.velocity = direction * phaseSpeed;
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
