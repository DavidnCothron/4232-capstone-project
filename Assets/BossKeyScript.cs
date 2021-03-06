﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossDoorScript))]
public class BossKeyScript : MonoBehaviour {
	
	public BossDoorScript bossDoor;
	public bool isInPlayerPossession = false;
	public SpriteRenderer sprite;
	public Animator sparkleAnimator;
	public Collider2D coll;
	public AudioSource keyJingle;

	// Use this for initialization
	void Awake () {
	}
	
	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Player Picked Up Key!");
		if(other.tag == "Player"){
			isInPlayerPossession = true;
			KeyPickup();
		}
	}

	void KeyPickup(){
		sprite.enabled = false;
		sparkleAnimator.enabled = false;
		coll.enabled = false;
		keyJingle.Play();
	}
}
