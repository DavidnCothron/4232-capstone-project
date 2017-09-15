using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeScript : MonoBehaviour {
	public Transform meleeTransform;
	public int meleeDamage = 1;
	public int chargeAttackDamage = 4;
	Collider2D[] enemiesHit;


	// Use this for initialization
	void Start () {
		
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
		//Multiplied by negative 1 because wizard shit
		transform.right = (-1f)*(GetWorldPositionOnPlane (Input.mousePosition, 0f) - meleeTransform.position);
	}

	void MeleeAttack(){
		foreach (Collider2D coll in enemiesHit)
		{
			var enemyScript = coll.GetComponent (typeof(Enemy)) as Enemy;
			enemyScript.health -= meleeDamage;
		}
	}

	void OnCollisionEnter(Collider2D coll){
		
	}
}
