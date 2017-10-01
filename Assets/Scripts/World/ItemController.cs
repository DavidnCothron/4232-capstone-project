using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Player"){
			//GameControl.ItemPickupEvent(<"healthRegen"><"Projectiles"><"Phase"><"ChargeAttack">);
			Destroy(this.gameObject);
		}
	}

}
