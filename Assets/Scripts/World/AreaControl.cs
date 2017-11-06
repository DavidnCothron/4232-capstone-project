using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AreaControl : MonoBehaviour {


	private AreaTransTuple areaEntry;
	private Door roomToEnter;	
	private PlayerManager player;

	
	void Start(){
		areaEntry = GameControl.control.GetNextArea();
		player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager; //gets player reference
		if(areaEntry.accessPointID == 0){
			roomToEnter = GameObject.Find("EntryRoom1").GetComponentInChildren<Door>();//this need to change to look for a specific door			
			StartCoroutine(roomToEnter.areaTransitionIn(player.GetComponent<Collider2D>()));//start area transitionIn cooroutines
		}
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
