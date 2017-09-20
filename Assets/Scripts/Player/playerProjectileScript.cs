using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileScript : MonoBehaviour {

	Vector2 direction;
	public Transform playerPosition;
	public ObjectPooler projectilePooler;

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}

	public void Fire(){
		direction = (Vector2)(GetWorldPositionOnPlane(Input.mousePosition, 0f) - playerPosition.position);
		GameObject projObj = projectilePooler.PopFromPool ();
		projObj.transform.position = (Vector2)playerPosition.position + (direction.normalized * 1f);
		Projectile proj = projObj.GetComponent (typeof(Projectile)) as Projectile;
		proj.direction = direction.normalized;
		projObj.SetActive (true);
	}

	void Update(){
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetMouseButtonDown (0))
		{
			Fire ();
		}
	}
}
