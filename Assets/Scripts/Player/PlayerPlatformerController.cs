using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 7f;
	public bool haltInput = false;
	private int direction;
	private bool attacking;
	
	[SerializeField]private SpriteRenderer spriteRenderer;
	[SerializeField]private Animator animator;

	// Use this for initialization
	void Awake () {
		//spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		//animator = gameObject.GetComponent<Animator> ();
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

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;
		
		if (!haltInput) //Stop input if we need to halt it
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
			
			bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
			//if (flipSprite)
			//	spriteRenderer.flipX = !spriteRenderer.flipX;
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
			animator.SetBool("dead", !PlayerManager.control.getAlive());

			//Apply target velocity here, which is utilized in the physics base
			targetVelocity = move * maxSpeed;
		}
	}
}
