using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour {
	public Transform playerTransform;
	public int meleeDamage = 1;
	public int chargeAttackDamage = 4;
	public List<Enemy> enemiesHit;


	// Use this for initialization
	void Awaken () {
		enemiesHit = new List<Enemy> ();
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
		foreach (Enemy enemy in enemiesHit)
		{
			enemy.health -= meleeDamage;
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			enemiesHit.Add (coll.gameObject.GetComponent(typeof(Enemy)) as Enemy);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if(coll.gameObject.tag == "enemy"){
			Enemy exitingColl = coll.gameObject.GetComponent (typeof(Enemy)) as Enemy;
			if (enemiesHit.Contains (exitingColl))
			{
				enemiesHit.Remove (exitingColl);
			}
		}
	}
}
