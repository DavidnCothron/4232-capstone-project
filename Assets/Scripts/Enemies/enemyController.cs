using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class enemyController : PhysicsObject {

	Rigidbody2D rigidBody;
	Transform playerTrans;
	Collider ownCollider;
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 3.5f;
	public bool haltInput = false;
	public int health = 5;
	public float sightDistance = 10f;
	public float arc = 45f;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
		ownCollider = gameObject.GetComponent<Collider>();
	}

	protected override void ComputeVelocity () {
		Vector2 move = Vector2.zero;
		if (!haltInput)
		{
			canSee();
			// #region Enemy Movement
			// Vector3 target = GameControl.control.GetPlayerTransform().position - this.transform.position;
			// if(target.magnitude < 5){
			// 	move = target;
			// 	targetVelocity = move * maxSpeed;
			// }
			// #endregion
		}
	}

	bool canSee(){
		playerTrans = GameControl.control.GetPlayerTransform();
		Vector3 target =  transform.position - playerTrans.position;
		if(Vector3.Distance(transform.position, playerTrans.position) < sightDistance){
			//Debug.Log("ang: " + Vector3.Angle(transform.right, playerTrans.position));
			//Debug.Log("dot: " + Vector3.Dot(transform.right, playerTrans.position));
				if(Vector3.Dot(transform.right, playerTrans.position) > 0 && Vector3.Angle(transform.right, playerTrans.position) < arc){
					//RaycastHit2D[] hits = new RaycastHit2D[10];
					//Physics2D.RaycastNonAlloc(transform.position, playerTrans.position - transform.position, hits);
					// foreach(var hit in hits){
					// 	Debug.Log(hit.collider.tag);
					// 	if(hit.collider.tag == "Player"){				
					// 		Debug.Log("player in sight");
					// 		return true;					
					// 	}					
					// }
					RaycastHit2D hit = new RaycastHit2D();
					Debug.Log(LayerMask.LayerToName(13));
					hit = Physics2D.Raycast(transform.position, (playerTrans.position- transform.position)+ new Vector3(0, .5f, 0), sightDistance, 0);
					Debug.DrawRay(transform.position, (playerTrans.position- transform.position)+ new Vector3(0, .5f, 0), Color.green);
					Debug.Log(hit.collider.tag);
					if(hit.collider.tag == "Player"){				
							Debug.Log("player in sight");
					 		return true;					
					 	}

					
				}
			}
			return false;
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
