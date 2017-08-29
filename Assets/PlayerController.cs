using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Input Variables
	float verticalVelocity;
	float horizontalVelocity;
	public float horizontalSpeed = 10.0f;
	public float verticalSpeed = 10.0f;
	#endregion

	#region Physics/Movement Variables
	Rigidbody2D rigidBody;
	Collider2D playerHurtBox;
	#endregion

	#region Flow Control Variables
	bool isBusy = false; //bool that controls actions during FixedUpdate
	bool isJumping = false;
	#endregion

	#region Ability Variables
	public int numberOfJumpsMax = 1;
	int numberOfJumps;
	#endregion

	// Use this for initialization
	void Awake () {
		//Control movement through the rigidBody object *only*
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		playerHurtBox = gameObject.GetComponent (typeof(Collider2D)) as Collider2D;
		numberOfJumps = numberOfJumpsMax;
	}
	
	// check for input and assign variables here
	void Update () {
		checkMoveInput ();
	}

	//Do movement and actions here
	void FixedUpdate()
	{
		if (!isBusy) Move ();
	}

	//Adjust movement modifiers here based on input
	void checkMoveInput()
	{
		horizontalVelocity = Input.GetAxis ("Horizontal") * horizontalSpeed;
		verticalVelocity = 1 * verticalSpeed;

		if (!isJumping)
		{
			numberOfJumps = numberOfJumpsMax;
			Debug.Log ("Not Jumping");
		}
	}

	void Move()
	{
		rigidBody.velocity = new Vector2 (horizontalVelocity, rigidBody.velocity.y);

		if (Input.GetKeyDown (KeyCode.Space))
		{
			if (numberOfJumps > 0)
			{
				numberOfJumps--;
				Jump ();
			}
		}
	}

	void Jump()
	{
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, verticalVelocity);
			Debug.Log ("Jump! " + numberOfJumps + " " + isJumping);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground")
			isJumping = false;
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground")
			isJumping = true;
	}
}
