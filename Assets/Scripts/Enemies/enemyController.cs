using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(EnemyAILogic))]
public class enemyController : PhysicsObject {

	Rigidbody2D rigidBody;
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 3.5f;
	public bool haltInput = false;
	public EnemyAILogic AILogic;
	public EnemyAILogic.IntelligenceType intType;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
	}


	protected override void ComputeVelocity () {
		
		Vector2 move = Vector2.zero;
		Debug.Log("before if");
		if (!haltInput)
		{
			Debug.Log("in if");
			#region Enemy Movement
			Vector3 target = GameControl.control.GetPlayerTransform().position - this.transform.position;
			if(target.magnitude < 5){
				move = target;
				targetVelocity = move * maxSpeed;
			}
			#endregion
		}
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		rigidBody.gravityScale = 1f;

		if(!isGrounded) {isGrounded = true;}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground")
			isGrounded = false;
	}

}
