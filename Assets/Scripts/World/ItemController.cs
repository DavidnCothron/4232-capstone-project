using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	public AudioSource pickupSound;
	void OnTriggerEnter2D(Collider2D other)
	{		
		if(other.gameObject.tag == "Player" && this.gameObject.tag == "healthPickup"){
			Debug.Log(this.gameObject.tag);
			GameControl.control.ItemPickup("healthRegain", true);
			Destroy(this.gameObject);
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "projectileItem"){
			GameControl.control.ItemPickup("projectile", true);
			Destroy(this.gameObject);
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "phaseItem"){
			Debug.Log("Picked up");
			pickupSound.Play();
			other.gameObject.GetComponent<PlayerPlatformerController>().hasPhase = true;
			//gameObject.SetActive(false);
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
		} else if(other.gameObject.tag == "Player" && this.gameObject.tag == "chargeAttackItem"){
			GameControl.control.ItemPickup("chargeAttack", true);
			Destroy(this.gameObject);
		}
	}



}
