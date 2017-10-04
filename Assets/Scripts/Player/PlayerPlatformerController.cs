using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 7f;
	public bool haltInput = false;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;

		if (!haltInput) //Stop input if we need to halt it
		{
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
			if (flipSprite)
				spriteRenderer.flipX = !spriteRenderer.flipX;

			//Handle animation state logic here
			animator.SetBool ("grounded", grounded);
			animator.SetFloat ("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

			//Apply target velocity here, which is utilized in the physics base
			targetVelocity = move * maxSpeed;
		}
	}
}
