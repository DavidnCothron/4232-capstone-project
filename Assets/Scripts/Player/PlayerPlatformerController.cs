using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 7f;
	public bool haltInput = false;
	private int direction;
	private bool attacking;
	private bool phasing;
	private Vector2 phaseDirection;
	private float phaseTime;
	public float phaseUpModifier = 1f;
	public float phaseTimeMax;
	public float phaseSpeed;
	[SerializeField]private SpriteRenderer spriteRenderer;
	public GameObject phaseParticleSystem;
	public ParticleSystem particleSystem;
	[SerializeField]private Animator animator;
	[SerializeField]private PlayerMeleeScript playerMelee;
	[SerializeField]private PlayerHealthAndMagicController playerHealthAndMagicController;

	// Use this for initialization
	void Awake () {
		//spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		//animator = gameObject.GetComponent<Animator> ();
		if(playerHealthAndMagicController == null){
			gameObject.GetComponent<PlayerHealthAndMagicController>();
		}
		phaseTime = phaseTimeMax;
		phaseParticleSystem.SetActive(false);
	}
	public bool getPhasing() {
		return phasing;
	}
	public int getBufferListCount() {
		return (hitBufferList.Count);
	}

	public int getDirection() {
		return direction;
	}

	public void setDirection(int d) {
		direction = d;
	}

	public void flipSprite() {
		spriteRenderer.flipX = !spriteRenderer.flipX;
	}

	public bool getGrounded() {
		return grounded;
	}

	public void setAttacking(bool b) {
		attacking = b;
	}

	public bool getAttacking() {
		return attacking;
	}

	public void setPhaseTime(float f) {
		phaseTime = f;
	}

	public void setPhasing(bool b) {
		phasing = b;
	}

	private void Phase(){
		switch(playerMelee.GetAttackDirection()){
			case "right":
				phaseDirection = new Vector2(1f, 0f);
				break;
			case "left":
				phaseDirection = new Vector2(-1f, 0f);
				break;
			case "up":
				phaseDirection = new Vector2(0f, 1f);
				phaseTime *= phaseUpModifier; //This keeps upward movement feeling similar to lateral movement
				break;
			default:
				break;
		}

		spriteRenderer.enabled = false;
		phasing = true;
		velocityUncapped = true;
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;
		
		if (!haltInput && !phasing) //Stop input if we need to halt it, also stop input if we are phasing
		{
			//Possibly temporarily halt input when attacking? (no direction switch?)
			move.x = Input.GetAxis ("Horizontal"); //Get left or right input

			if (Input.GetButtonDown ("Jump") && grounded)
			{
				velocity.y = jumpTakeOffSpeed;
			}
			else
			if (Input.GetButtonUp ("Jump")) //If canceling jump mid air
			{
				if (velocity.y > 0)
				{
					velocity.y = velocity.y * 0.5f;
				}
			}

			if(Input.GetMouseButtonDown(1)){
				gravityModifier = 0f;
				Phase();
			}
			
			bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
			//if (flipSprite)
			//	spriteRenderer.flipX = !spriteRenderer.flipX;
			if (!playerMelee.getAttacking() && !haltInput){
				if (Input.GetAxis("Horizontal") < -0.1) 
				{
					direction = -1; //When -1, player is facing left
					spriteRenderer.flipX = true;
				} 
				else if (Input.GetAxis ("Horizontal") > 0.1) 
				{
					direction = 1; //When 1, player is facing right
					spriteRenderer.flipX = false;
				}
			}

			if(Input.GetMouseButtonDown(1)){
				Phase();
			}

			//Handle animation state logic here
			animator.SetBool ("grounded", grounded);
			animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
			animator.SetFloat ("velocityY", velocity.y / maxSpeed);

			if (velocity.x!= 0 && grounded)
			{
				animator.SetBool ("startRun", true);
				if (Mathf.Abs(velocity.x) > 0.01f)
					animator.SetBool ("isRunning", true);
			} 
			else 
			{
				animator.SetBool ("startRun", false);
				animator.SetBool ("isRunning", false);
			}

			//Apply target velocity here, which is utilized in the physics base
			targetVelocity = move * maxSpeed;
		}
		else if(phasing){
			phaseTime -= Time.deltaTime;
			if(phaseTime >= 0){
				targetVelocity = phaseDirection * phaseSpeed;
				if(!phaseParticleSystem.active) phaseParticleSystem.SetActive(true);
				particleSystem.Play();
				velocity.y = targetVelocity.y;
			}
			else{
				phasing = false;
				velocityUncapped = false;
				phaseTime = phaseTimeMax;
				gravityModifier = 4;
				particleSystem.Stop();
				phaseParticleSystem.SetActive(false);
				spriteRenderer.enabled = true;
			}
		}
	}

	public void forcePhaseStop(){
		phaseTime = 0f;
		targetVelocity = Vector2.zero;
		phasing = false;
		velocityUncapped=false;
		phaseTime = phaseTimeMax;
		gravityModifier = 4;
		particleSystem.Stop();
		phaseParticleSystem.SetActive(false);
		spriteRenderer.enabled = true;
	}
}
