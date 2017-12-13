using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour {

	Rigidbody2D rigidBody;
	Transform playerTrans;
	public Transform spawn;
	public string batHangState;
	public string batFlyingState;
	public enum state { hang, flying}
	public state State;
	public bool onAlert = false;
	private float baseSpeed = 2f;
	public bool haltInput = false;
	public float sightDistance = 25f;
	public float arc = 15f;	
	Vector3 enemyFacingDirection = new Vector3(1,0,0);
	Vector3 scale;
	public LayerMask playerLayer;
	Vector3 target;
	bool moving;
	public bool targetInRange = false;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	void Awake () {
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
		//spawn = gameObject.GetComponentInParent(typeof(Transform)) as Transform;
	}

	// Use this for initialization
	void Start () {
		//baseSpeed += Random.value;
		//Debug.Log("bat speed " +baseSpeed);
		
	}
	
	void Update() {
		//Vector2 move = Vector2.zero;
		scale = transform.localScale;		
		if (!haltInput)
		{
			playerTrans = GameControl.control.GetPlayerTransform();
			target = playerTrans.position - transform.position;
			#region Enemy Movement

			targetInRange = Physics2D.OverlapCircle(spawn.position, sightDistance, playerLayer);

			if(transform.position != spawn.position){
				moving = true;
			}else
				moving = false;

			if(targetInRange){
				transform.position = Vector2.MoveTowards(transform.position, playerTrans.position  + new Vector3(0, 1f, 0), baseSpeed * Time.deltaTime);
			}else{
				transform.position = Vector2.MoveTowards((Vector2)transform.position, spawn.position, baseSpeed * Time.deltaTime);
			}
			if(moving){
				State = state.flying;
				if ((target.x + transform.position.x) < transform.position.x)//if moving left face left
				{ //Flip to left
			 		flipLeft();
			 	}
			 	else if ((target.x + transform.position.x) > transform.position.x)//if moving right face right
			 	{ //Flip to right
			 		flipRight();
			 	}
			}else
				State = state.hang;
			

			 //TODO: implement patrol and chase
			//  if(Vector3.SqrMagnitude(target) < sightDistance){//if withing sight distance
			//  	if(canSee(target))//if can see target
			//  		onAlert = true;		//put enemy on alert
			//  }else{
			//  	onAlert = false;//if not withing sight distance
			//  }

			 if(onAlert){
			// 	//Debug.Log("target: " + targetVelocity);
			// 	State = state.flying;
			// 	//canAttack = false;

			// 	if ((target.x + transform.position.x) < transform.position.x)//if moving left face left
			// 	{ //Flip to left
			// 		flipLeft();
			// 	}
			// 	else if ((target.x + transform.position.x) > transform.position.x)//if moving right face right
			// 	{ //Flip to right
			// 		flipRight();
			// 	}

			 }
						
			#endregion			
		}
		
		UpdateAnimations();//update animation state
	}

	void flipLeft(){
		scale.x = Mathf.Abs(scale.x);
		transform.localScale = scale;
		enemyFacingDirection = -transform.right;	
	}
	void flipRight(){
		scale.x = -Mathf.Abs(scale.x);
		transform.localScale = scale;
		enemyFacingDirection = transform.right;
	}

	public void Die(){
		haltInput = true;
		gameObject.SetActive(false);	
	}
	
	bool canSee(Vector3 targetVector){
		playerTrans = GameControl.control.GetPlayerTransform();
		if(Vector3.SqrMagnitude(targetVector) < sightDistance){
			//Debug.Log(Vector3.SqrMagnitude(targetVector));
			if(Vector3.SqrMagnitude(targetVector) < 4.1f || (Vector3.Dot(enemyFacingDirection, targetVector) > 0 && Vector3.Angle(targetVector, enemyFacingDirection) < arc)){

				RaycastHit2D hit = new RaycastHit2D();
				LayerMask mask = ~(1 << LayerMask.NameToLayer("AttackLayer") | 1 << LayerMask.NameToLayer("RoomBackground") | 1 << LayerMask.NameToLayer("Enemy"));//ignore self (attack layer) and roombackground layer
				
				hit = Physics2D.Raycast(transform.position, (targetVector+ new Vector3(0, .5f, 0)), sightDistance, mask);//raycast towards player
				//Debug.DrawRay(transform.position, (targetVector + new Vector3(0, .5f, 0)), Color.green);
				//Debug.Log(hit.collider.tag);
				if(hit.collider.tag == "Player"){//if hit player then return true. hit anything else return false.
					return true;					
				}
			}
		}
		return false;
	}

	void UpdateAnimations()
    {
		//Debug.Log(State);
        if (State == state.flying)
        {
            animator.CrossFade(batFlyingState, 0f);
        } else
        if (State == state.hang)
        {
            animator.CrossFade(batHangState, 0f);
        }
    }

	void OnCollision2D(Collision2D coll)
	{
		Debug.Log(coll.gameObject.tag);
		Debug.Log(coll.gameObject.name);
	}
}
