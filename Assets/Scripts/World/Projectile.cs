using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

	public bool hostileDamage = false;
	public float damage = 1f;
	public float speed = 1f;
	public float size = 0.25f;
	public float manaCost = 1f;
	public Vector2 direction;
	public float lifeTime = 2f;
	public Rigidbody2D rigidBody;
	public Animator animator;

	void FixedUpdate(){
		rigidBody.AddForce (direction * speed);
	}

	void OnEnable(){
		Invoke ("Destroy", lifeTime);
		animator.Play ("Fired", 0);
	}

	void Awake(){
		rigidBody.velocity = direction * speed * 60;
	}

	void OnDisable(){
		CancelInvoke ();
	}

	public void Destroy(){
		this.gameObject.SetActive (false);
	}
}
