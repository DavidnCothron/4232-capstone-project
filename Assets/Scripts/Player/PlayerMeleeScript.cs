using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour {
	public Transform playerTransform;
	public int meleeDamage = 1;
	public int chargeAttackDamage = 4;
	public float knockbackBasic = 1f;
	public float knockbackCharged = 4f;
	public List<GameObject> enemiesHit;


	// Use this for initialization
	void Awaken () {
		enemiesHit = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0))
		{
			MeleeAttack ();
		}
	}

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}

	void FixedUpdate(){
		transform.right = (GetWorldPositionOnPlane (Input.mousePosition, 0f) - playerTransform.position);
	}

	void MeleeAttack(){
		foreach (GameObject enemyObject in enemiesHit)
		{
			Enemy enemyScript = enemyObject.GetComponent (typeof(Enemy)) as Enemy;
			Rigidbody2D enemyRB2D = enemyObject.GetComponent (typeof(Rigidbody2D)) as Rigidbody2D;
			enemyScript.health -= meleeDamage;
			enemyRB2D.AddForce ((Vector2)((playerTransform.position - enemyRB2D.transform.position).normalized * knockbackBasic));

		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			enemiesHit.Add (coll.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			if (enemiesHit.Contains (coll.gameObject))
			{
				enemiesHit.Remove (coll.gameObject);
			}
		}
	}
}
