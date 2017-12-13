using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class enemyController : PhysicsObject {

	Rigidbody2D rigidBody;
	Transform playerTrans;

	//Animaitons
	public float CurrentAttackTime;
    public string standAnimationState;
    public string RunAnimationState;
    public string JumpAnimationState;
    public string FallAnimationState;
    public string Attack1AnimationState;
    public float Attack1AnimationDuration;
    public string Attack2AnimationState;
    public float Attack2AnimationDuration;
    public string Attack3AnimationState;
    public float Attack3AnimationDuration;
    public string DefAnimationState;
    public string DeathAnimationState;
	public enum state { stand, jump, fall, run, attack1, attack2, attack3, Def, Death}
	public state State;
	private bool canAttack = false;
	private bool attacking = false;
	private float attackCooldown = .75f;
	//public float attackTime = .75f;
	private float attackCooldownRemaining;
	private bool inKnockback;
	private Vector2 knockbackVec;
	public bool onAlert = false;
	public bool isBusy = false; //bool that controls actions during FixedUpdate
	public bool isGrounded = true;
	public float jumpTakeOffSpeed = 10f;
	private float baseSpeed = .5f;
	public bool haltInput = false;
	public float sightDistance = 25f;	
	public float arc = 15f;
	public bool isFacingRight;
	Vector3 enemyFacingDirection = new Vector3(1,0,0);
	Vector3 scale;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private EnemyMelee enemyMelee;

	// Use this for initialization
	void Awake () {
		rigidBody = gameObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
		enemyMelee = gameObject.GetComponentInChildren<EnemyMelee>();
			
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		baseSpeed += Random.value;
		//Debug.Log(baseSpeed);
		// if(!isFacingRight)
		// 	flipLeft();
		// scale = transform.localScale;
	}


	//TODO: Add logic to path towards last known position if can no longer see target
	protected override void ComputeVelocity () {
		Vector2 move = Vector2.zero;
		scale = transform.localScale;
		

		if(State != state.Death){
			if(!attacking)
				State = state.stand;

			if (!haltInput)
			{
				playerTrans = GameControl.control.GetPlayerTransform();
				Vector3 target = playerTrans.position - transform.position;
				#region Enemy Movement

				//TODO: implement patrol and chase
				if(Vector3.SqrMagnitude(target) < sightDistance){//if withing sight distance
					if(canSee(target))//if can see target
						onAlert = true;		//put enemy on alert
				}else{
					onAlert = false;//if not withing sight distance
				}

				if(onAlert){
					//Debug.Log(Mathf.Abs(target.x));
					if(inKnockback){
							updateKnockbackFall();
							//velocity.x = knockbackVec.x;
							//Debug.Log(knockbackVec.y);
							velocity.y = knockbackVec.y;
							targetVelocity = knockbackVec;
					}else{
						if(Mathf.Abs(target.x) > .8f && !attacking){//move towards target 
							move = target;
							if((checkForJump() && grounded) || (playerTrans.position.y > (transform.position.y + 1.5)) && grounded){
								jump();
							}

							targetVelocity = move * baseSpeed;
							//Debug.Log("target: " + targetVelocity);
							State = state.run;//play run animation
							//canAttack = false;

						}else{//attack target if reasonably close
							//canAttack = true;
							attackCooldownRemaining -= Time.deltaTime;
							if(attackCooldownRemaining < 0f){
								attackCooldownRemaining = attackCooldown;
								StartCoroutine(attack());
							}
						}

						if ((target.x + transform.position.x) < transform.position.x)//if moving left face left
						{ //Flip to left
							flipLeft();
							//arc = 
						}
						else if ((target.x + transform.position.x) > transform.position.x)//if moving right face right
						{ //Flip to right
							flipRight();
						}
					}
				}
				//Debug.Log(grounded);
				if(!grounded){//if falling play falling animation
					State = state.fall;
				}				
				#endregion			
			}
		}
		UpdateAnimations();//update animation state
	}

	void jump(){
		velocity.y = jumpTakeOffSpeed;
	}

	void flipLeft(){
		scale.x = -Mathf.Abs(scale.x);
		transform.localScale = scale;
		enemyFacingDirection = -transform.right;	
	}
	void flipRight(){
		scale.x = Mathf.Abs(scale.x);
		transform.localScale = scale;
		enemyFacingDirection = transform.right;
	}

	bool checkForJump(){
		LayerMask mask = ~(1 << LayerMask.NameToLayer("AttackLayer") | 1 << LayerMask.NameToLayer("RoomBackground") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Player"));
		RaycastHit2D hit = new RaycastHit2D();
		hit = Physics2D.Raycast(transform.position - new Vector3(0, .5f, 0), enemyFacingDirection, 1f, mask);
		Debug.DrawRay(transform.position- new Vector3(0, .7f, 0), enemyFacingDirection * 1f, Color.red);
		//Debug.Log(hit.collider.tag);
		if(hit){
			if(hit.collider.tag == "ground"){
				return true;			
			}else{
				return false;
			}
		}else{
			return false;
		}
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

	public IEnumerator attack(){
		State = state.attack1;
		attacking = true;
		haltInput = true;
		enemyMelee.Hit();
		yield return new WaitForSeconds(attackCooldown);		
		haltInput = false;
		attacking = false;
		State = state.stand;
		//Debug.Log("finished routine");
	}

	public IEnumerator setKnockbackVec(Vector3 force){
		inKnockback = true;
		knockbackVec = (enemyFacingDirection == transform.right ? new Vector2(-force.x, force.y) : new Vector2(force.x, force.y));
		Debug.DrawRay(gameObject.transform.position, knockbackVec, Color.red);
		//Debug.Log("starting value: " + knockbackVec);
		yield return new WaitForSeconds(.3f);
		inKnockback = false;
	}

	void updateKnockbackFall(){
		knockbackVec.y = Mathf.Lerp(knockbackVec.y, 0, .1f);
		//Debug.Log(knockbackVec.y);
	}

	public void Die(){
		haltInput = true;
		State = state.Death;	
	}

 void UpdateAnimations()
    {
		//Debug.Log(State);
        if (State == state.stand)
        {
            animator.CrossFade(standAnimationState, 0f);
        } else
        if (State == state.run)
        {
            animator.CrossFade(RunAnimationState, 0f);
        }
        else
        if (State == state.jump)
        {
            animator.CrossFade(JumpAnimationState, 0f);
        }
        else if (State == state.fall)
        {
            animator.CrossFade(FallAnimationState, 0f);
        }
        else
        if (State == state.attack1)
        {
            animator.CrossFade(Attack1AnimationState, 0f);
        }
        else
        if (State == state.attack2)
        {
            animator.CrossFade(Attack2AnimationState, 0f);
        }
        else
        if (State == state.attack3)
        {
            animator.CrossFade(Attack3AnimationState, 0f);
        } else
        if (State == state.Death)
        {
            animator.CrossFade(DeathAnimationState, 0f);
            //print("morra");
        }
        else if (State == state.Def)
        {
            animator.CrossFade(DefAnimationState, 0f);
        }
    }
	/*
	void OnCollisionStay2D(Collision2D coll)
	{
		rigidBody.gravityScale = 1f;

		if(!isGrounded) {isGrounded = true;}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ground"){
			isGrounded = false;
			State = state.fall;
		}
	}*/

}
