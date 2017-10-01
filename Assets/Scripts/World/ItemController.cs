using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other)
	{		
		if(other.gameObject.tag == "Player" && this.gameObject.tag == "healthPickup"){
			Debug.Log(this.gameObject.tag);
			GameControl.control.ItemPickup("healthRegain", true);
			Destroy(this.gameObject);
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "projectileItem"){
			GameControl.control.ItemPickup("projectile", true);
			Destroy(this.gameObject);
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "phaseItem"){
			GameControl.control.ItemPickup("phase", true);
			Destroy(this.gameObject);
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "chargeAttackItem"){
			GameControl.control.ItemPickup("chargeAttack", true);
			Destroy(this.gameObject);
		}
	}

}
