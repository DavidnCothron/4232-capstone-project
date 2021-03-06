﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

	public float minGroundNormalY = 0.65f;
	public float gravityModifier = 1f;

	[SerializeField] protected float maxYVelocity;
	[SerializeField] protected bool velocityUncapped = false;
	[SerializeField] protected Vector2 targetVelocity;
	[SerializeField] protected bool grounded;
	[SerializeField] protected Vector2 groundNormal;
	[SerializeField] protected Rigidbody2D rb2d;
	[SerializeField] protected Vector2 velocity;
	[SerializeField] protected Vector2 move;
	[SerializeField] protected ContactFilter2D contactFilter;
	[SerializeField] protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	[SerializeField] protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	protected const float minMoveDistance = 0.001f;
	protected const float shellRadius = 0.01f;

	void Awake(){
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	public void SetGravity(float value){
		gravityModifier = value;
	}

	/// <summary>
	/// Sets the maxYVelocity for this PhysicsObject.
	/// This is applied as both positive and negative y velocity.
	/// </summary>
	/// <returns></returns>
	/// <param name="value">a float point number</param>
	public void SetMaxYVelocity(float value) {
		maxYVelocity = value;
	}

	/// <summary>
	/// Returns the maximum Y velocity set for this PhysicsObject
	/// </summary>
	/// <returns>(float)maxYVelocity</returns>
	public float getMaxYVelocity() {
		return maxYVelocity;
	}

	/// <summary>
	/// Returns the Vector2 velocity of a given PhysicsObject
	/// </summary>
	/// <returns>(Vector2)velocity</returns>
	public Vector2 getVelocity() {
		return velocity;
	}

	public void SetVelocityOverride(Vector2 newVelocity){
		velocity = newVelocity;
	}

	// Use this for initialization
	void Start () {
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	void Update () {
		targetVelocity = Vector2.zero;
		ComputeVelocity ();	
	}

	protected virtual void ComputeVelocity(){
	}

	void FixedUpdate(){
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		grounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;
		Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

		move = moveAlongGround * deltaPosition.x;

		Movement (move, false);

		move = Vector2.up * deltaPosition.y;

		Movement (move, true);
	}

	void Movement(Vector2 move, bool yMovement){
		float distance = move.magnitude;

		if (distance > minMoveDistance)
		{
			int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
			hitBufferList.Clear ();

			for (int i = 0; i < count; i++)
			{
				hitBufferList.Add (hitBuffer [i]);
			}

			for (int i = 0; i < hitBufferList.Count; i++)
			{
				Vector2 currentNormal = hitBufferList [i].normal;
				if (currentNormal.y > minGroundNormalY)
				{
					grounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
					/* if maxYVelocity, velocity.y is capped to a signed velocity at maxYVelocity value */
					if ((maxYVelocity != 0 && Mathf.Abs(velocity.y) > maxYVelocity) && !velocityUncapped)
					{
						velocity.y = Mathf.Sign(velocity.y) * maxYVelocity;
					}
				}

				float projection = Vector2.Dot (velocity, currentNormal);
				if (projection < 0)
				{
					velocity = velocity - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList [i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb2d.position = rb2d.position + move.normalized * distance;
	}
}
