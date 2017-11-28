using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AreaControl : MonoBehaviour {


	private AreaTransTuple areaEntry; 
	[SerializeField] private Door roomToEnter;
	private PlayerManager player;

	
	void Start(){
		areaEntry = GameControl.control.GetNextArea();
		player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager; //gets player reference
		Debug.Log("trans from area control");			
		StartCoroutine(roomToEnter.areaTransitionIn(player.GetComponent<Collider2D>()));//start area transitionIn cooroutines
		
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
