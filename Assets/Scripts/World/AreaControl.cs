using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AreaControl : MonoBehaviour {

	private AreaTransTuple areaEntry; 
	[SerializeField] private Door roomToEnter;
	private PlayerManager player;

	
	void Start(){
		if(roomToEnter != null){
			areaEntry = GameControl.control.GetNextArea();
			player = GameObject.Find ("Player").GetComponent(typeof(PlayerManager)) as PlayerManager; //gets player reference
			StartCoroutine(roomToEnter.areaTransitionIn(player.GetComponent<Collider2D>()));//start area transitionIn cooroutines
		}else{
			Debug.Log("No starting Room");
		}
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
