using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileScript : MonoBehaviour {

	public int damage;
	public float speed;
	public float trackingIntensity;
	public Vector3 targetTrans;
	Vector3 closestEnemy;
	PlayerActionController playerActionController;

	public void Fire(Vector3 mousePos){
		targetTrans = mousePos;
	}

	void FindClosestEnemy(){
//		float maxDistance = float.MaxValue;
//		float temp;
//		foreach (Enemy enemy in playerActionController.enemies)
//		{
//			if ((temp = Vector2.Distance ((Vector2)transform.position, (Vector2)enemy.transform.position)) < maxDistance)
//			{
//				maxDistance = temp;
//				closestEnemy = enemy.transform.position;
//			}
//		}
	}

	void Update(){
		FindClosestEnemy ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		targetTrans = targetTrans + (closestEnemy * trackingIntensity);
		Vector2.MoveTowards (transform.position, targetTrans, speed);
	}

	void OnEnable(){
		//transform.position = playerActionController.transform.position;
		//Invoke ("Destroy", 2f);
	}

	void OnDisable(){
		CancelInvoke ();
	}

	void Destroy(){
		gameObject.SetActive (false);
	}
}
