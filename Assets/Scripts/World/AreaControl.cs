using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AreaControl : MonoBehaviour {


	private int areaEntry;
	private Door roomToEnter;
	
	private PlayerManager player;

	
	void Start(){
		Debug.Log("test on start");
		areaEntry = GameControl.control.GetAreaEntryID();
		if(areaEntry == 1){
			roomToEnter = GameObject.Find("Room1").GetComponentInChildren<Door>();//gets room to enter based on value passed from GetAreaEntryId in Game control
			player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager; //gets player reference
			StartCoroutine(roomToEnter.areaTransitionIn(player.GetComponent<Collider2D>()));//start area transition in cooroutine
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
