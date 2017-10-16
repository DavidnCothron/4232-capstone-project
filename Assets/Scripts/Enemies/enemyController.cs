using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(EnemyAILogic))]
public class enemyController : PhysicsObject {

	public float jumpTakeOffSpeed = 7f;
	public float maxSpeed = 6.5f;
	public bool haltInput = false;
	public EnemyAILogic AILogic;
	public EnemyAILogic.IntelligenceType intType;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
	}


	void ComputeVelocity () {
		
		Vector2 move = Vector2.zero;

		if (!haltInput)
		{
			#region Enemy Movement
			move = AILogic.AIMove ((int)intType);
			move = move - (Vector2)transform.position;
			targetVelocity = move * maxSpeed;
			#endregion
		}
	}
}
