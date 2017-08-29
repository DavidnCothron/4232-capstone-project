using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Input Variables
	float upVelocity = 0.0f;
	float downVelocity = 0.0f;
	float leftVelocity = 0.0f;
	float rightVelocity = 0.0f;
	public float upMaxVelocity;
	public float downMaxVelocity;
	public float leftMaxVelocity;
	public float rightMaxVelocity;
	public float upAcceleration = 1f;
	public float downAcceleration = 1f;
	public float rightAcceleration = 1f;
	public float leftAcceleration = 1f;
	#endregion

	Rigidbody2D rigidBody;

	// Use this for initialization
	void Awake () {
		//Control movement through the rigidBody object *only*
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		//rigidBody.velocity += new Vector2 (1f, .0f);
	}
	
	// check for input and assign variables here
	void Update () {
		checkMoveInput ();
	}

	//Do movement and actions here
	void FixedUpdate()
	{
		Move ();
	}

	//Adjust movement modifiers here based on input
	void checkMoveInput()
	{
		if (Input.GetKeyDown (KeyCode.A))
		{
			if (leftVelocity >= leftMaxVelocity)
				leftVelocity = leftMaxVelocity;
			else
				leftVelocity += leftAcceleration;
		}
		if (Input.GetKeyDown (KeyCode.W))
		{
			if (upVelocity >= upMaxVelocity)
				upVelocity = upMaxVelocity;
			else
				upVelocity += upAcceleration;
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			if (downVelocity >= downMaxVelocity)
				downVelocity = downMaxVelocity;
			else
				downVelocity += downAcceleration;
		}
		if (Input.GetKeyDown (KeyCode.D))
		{
			if (rightVelocity >= rightMaxVelocity)
				rightVelocity = rightMaxVelocity;
			else
				rightVelocity += rightAcceleration;
		}
		Debug.Log (rightVelocity + " " + leftVelocity + " " + downVelocity + " " + upVelocity);
	}

	void Move()
	{
		rigidBody.AddForce (new Vector2((rightVelocity - leftVelocity), (upVelocity - downVelocity)));
	}
}
