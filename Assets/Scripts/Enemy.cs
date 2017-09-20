using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform trans;
	public int health = 10;

	// Use this for initialization
	void Start () {
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "projectile")
		{
			Projectile proj = coll.gameObject.GetComponent (typeof(Projectile)) as Projectile;
			health -= (int)proj.damage;
			proj.Destroy ();
		}
	}
}
