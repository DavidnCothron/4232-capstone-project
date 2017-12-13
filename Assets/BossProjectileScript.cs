using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileScript : MonoBehaviour {

	public Transform playerTransform;
	public float maxSpeed;
	public Rigidbody2D rigidbody;
	public int damage = 2;
	private Vector2 direction;

	private void Awake() {
		playerTransform = GameControl.control.GetPlayerTransform();
	}
	
	
	// Update is called once per frame
	void Update () {
		direction = playerTransform.position - gameObject.transform.position;
		rigidbody.AddForce(direction * maxSpeed);
		Destroy(gameObject, 2f);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player"){
			other.GetComponent<PlayerHealthAndMagicController>().LoseHealth(damage);
		}
	}
}
